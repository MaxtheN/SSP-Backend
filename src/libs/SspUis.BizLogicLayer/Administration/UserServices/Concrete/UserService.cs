using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;
//using RestSharp.Extensions;
using SspUis.BizLogicLayer.Edoc.AppointmentOrderService;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.MSPD.GSP;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.UserServices
{
    public class UserService : StatusGenericHandler, IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;
        private readonly IPersonService _personService;
        private readonly IDocumentChangeLogRepository _documentChangeLogRepository;
        private readonly IAppointmentOrderService _appointmentOrderService;
        public UserService(IUnitOfWork unitOfWork,
            IAuthService authService,
            IPersonService personService,
            IStorageService storageService,
            IAppointmentOrderService appointmentOrderService,
            ICultureHelper cultureHelper,
            IDocumentChangeLogRepository documentChangeLogRepository)
        {
            _repository = unitOfWork.UserRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _cultureHelper = cultureHelper;
            _authService = authService;
            _personService = personService;
            _appointmentOrderService = appointmentOrderService;
            this._documentChangeLogRepository = documentChangeLogRepository;
        }

        public PagedResult<UserListDto> GetList(UserSortFilterPageOptions dto)
        {
            return GetQuery<UserListDto>()
                .SortFilter(dto)
                .FilterByCustomFields(dto)
                .AsPagedResult(dto);
        }

        public UserDto Get()
        {
            //SyncEdocUser(null);
            //if (HasErrors)
            //    return null;
            return new UserDto
            {
                OrganizationId = _authService.User.OrganizationId,
                Organization = _authService.Organization?.FullName
            };
        }

        public UserDto Get(int id)
        {
            var dto = GetQuery<UserDto>().FirstOrDefault(a => a.Id == id);
            if (dto == null)
                AddError("По вашему запросу запись не найдено");
            return dto;
        }

        public PagedSelectList<int> AsSelectList(UserSortFilterPageOptions options)
        {
            return _repository.ReadAsNoTracked<UserListDto>()
                              .FilterByIds(options)
                              .FilterByCustomFields(options)
                              .SortFilter(options)
                              .AsPagedResult(options)
                              .AsSelectList();
        }

        public HaveId<int> Create(CreateUserDto dto)
        {
            try
            {
                var canCommit = _unitOfWork.CurrentTransaction == null;
                var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

                Person person = _unitOfWork.PersonRepository.AllAsQueryable
                    .FirstOrDefault(a => !string.IsNullOrEmpty(dto.Person.Pinfl) && a.Pinfl == dto.Person.Pinfl);

                if (person == null)
                {
                    person = _unitOfWork.PersonRepository.Create(dto.Person);
                    CombineStatuses(_unitOfWork.PersonRepository);
                }
                if (IsValid)
                {
                    var entity = _repository.Create(dto, ent =>
                    {
                        var exsist = _repository.Context.Set<User>().Any(x => x.UserName.ToLower().Equals(dto.UserName.ToLower()));
                        var exsistPhoneNumber = _repository.Context.Set<User>().Any(x => x.PhoneNumber.Equals(dto.PhoneNumber));

                        if (exsist)
                        {
                            AddError($"Bu username orqali boshqa foydalanuvchi ro'yxatdan o'tgan username : {dto.UserName}");
                            return;
                        }
                        if(exsistPhoneNumber)
                        {
                            AddError($"Bu Phone Number orqali boshqa foydalanuvchi ro'yxatdan o'tgan Phone Number : {dto.PhoneNumber}");
                            return;
                        }

                        if (dto.EmployeeManageId.HasValue)
                        {
                            var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
                                .Select(a => new
                                {
                                    Id = a.Id,
                                    OrganizationId = a.OrganizationId,
                                    PersonId = a.Employee.PersonId,
                                    PersonPinfl = a.Employee.Person.Pinfl,
                                    DocumentId = a.DocId
                                })
                                .FirstOrDefault(a => a.Id == dto.EmployeeManageId.Value
                                    && a.OrganizationId == dto.OrganizationId
                                );

                            if (employeeManage == null)
                                AddError($"Hodim topilmadi. Id {dto.EmployeeManageId.Value}");
                            else if (employeeManage.PersonId != person.Id)
                                AddError($"Foydalanuvchi va hodim ma'lumotlari mos emas. Foydalanuvchi JShShIR: {person.Pinfl}. Hodim JShShIR: {employeeManage.PersonPinfl}.");

                            dto.DocumentId = employeeManage.DocumentId;
                        }
                    });
                    CombineStatuses(_repository);
                    if (IsValid)
                    {
                        entity.Person = person;
                        entity.PersonId = person.Id;
                        _unitOfWork.Save();
                        if (dto.EmployeeManageId.HasValue && !dto.DocumentId.HasValue)
                            AddError($"Hodim ishga qabul qilish hujjat id topilmadi. Id {dto.EmployeeManageId.Value}");

                        AppointmentOrderDto appointmentOrderDto = null;
                        if (dto.EmployeeManageId.HasValue && dto.DocumentId.HasValue)
                        {
                            var appointmentEmployee = _unitOfWork.Context.Set<AppointEmployee>()
                                                             .Include(x => x.Tables)
                                                             .FirstOrDefault(x => x.Id == dto.DocumentId.Value);
                            appointmentOrderDto = AppointmentOrderMap(appointmentEmployee, dto.EmployeeManageId.Value);
                        }

                        if (HasErrors)
                            return null;

                        ///Edoc schema uchun userni sync qilish.
                        if (dto.EmployeeManageId.HasValue)
                        {
                            CreateUserForEdocSchema(entity, appointmentOrderDto);
                        }
                        _unitOfWork.Save();
                        CreateChangeLog(entity.Id, $"User Created with '{entity.UserName}' - User name, and '{entity.PhoneNumber}' - Phone for '{entity.Person.FullName}' ", StatusIdConst.CREATED);

                        if (IsValid)
                        {
                            if (canCommit)
                                transaction.Commit();
                            return HaveId.Create(entity.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                if (ex.InnerException != null)
                    AddError(ex.InnerException.Message);
                _unitOfWork.Rollback();
            }

            return null;
        }

        public async Task<CreateUserDto> GetByPassportData(GSPPersonInfoRequestForUserDto dto)
        {
            var person = await _personService.GetByPassportData(new GSPPersonInfoRequestDto
            {
                Seria = dto.Seria,
                Number = dto.Number,
                DateOfBirth = dto.DateOfBirth,
            });
            CombineStatuses(_personService);
            if (HasErrors || person == null)
                return null;

            var existingUsers = _repository.AllAsQueryable
                .Where(a => a.OrganizationId == dto.OrganizationId
                    && a.Person.Pinfl == person.Pinfl
                    && a.EmployeeManageId.HasValue
                )
                .Select(a => a.EmployeeManageId)
                .ToList();

            var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
                .Where(a => !a.EndOn.HasValue && !a.IsDeleted && a.OrganizationId == dto.OrganizationId
                    && !existingUsers.Contains(a.Id)
                )
                .Select(a => new
                {
                    Id = a.Id,
                    PersonId = a.Employee.PersonId,
                    PersonPinfl = a.Employee.Person.Pinfl,
                    EmploymentTypeId = a.EmploymentTypeId,
                    DocId = a.DocId
                })
                .OrderBy(a => a.EmploymentTypeId)
                .FirstOrDefault(a => a.PersonPinfl == person.Pinfl);

            return new CreateUserDto
            {
                Person = person,
                OrganizationId = dto.OrganizationId,
                DocumentId = employeeManage?.DocId,
                EmployeeManageId = employeeManage?.Id
            };
        }

        public void Update(UpdateUserDlDto dto)
        {
            try
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    var entity = _repository.Update(dto, ent =>
                    {
                        if (dto.EmployeeManageId.HasValue)
                        {
                            Person person = _unitOfWork.PersonRepository.AllAsQueryable
                                .FirstOrDefault(a => a.Id == ent.PersonId);

                            var exsist = _repository.Context.Set<User>()
                                .FirstOrDefault(x => x.Id != dto.Id && x.EmployeeManageId.HasValue && x.EmployeeManageId == dto.EmployeeManageId);
                            if (exsist != null)
                            {
                                AddError($"Бу EmployeeManage Id бошқа усерга бириктирилган.UserName : {exsist.UserName} Id: {exsist.Id}", nameof(dto.EmployeeManageId));
                                return;
                            }

                            var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
                                .Select(a => new
                                {
                                    Id = a.Id,
                                    OrganizationId = a.OrganizationId,
                                    PersonId = a.Employee.PersonId,
                                    PersonPinfl = a.Employee.Person.Pinfl
                                })
                                .FirstOrDefault(a => a.Id == dto.EmployeeManageId.Value
                                    && a.OrganizationId == dto.OrganizationId
                                );

                            if (employeeManage == null)
                                AddError($"Hodim topilmadi. Id {dto.EmployeeManageId.Value}");
                            else if (employeeManage.PersonId != ent.PersonId)
                                AddError($"Foydalanuvchi va hodim ma'lumotlari mos emas. Foydalanuvchi JShShIR: {person.Pinfl}. Hodim JShShIR: {employeeManage.PersonPinfl}.");
                        }
                    });
                    CombineStatuses(_repository);
                    if (IsValid)
                        _unitOfWork.Save();
                    CreateChangeLog(entity.Id, $"User Updated with '{entity.UserName}' - User name, and '{entity.PhoneNumber}' - Phone for '{entity.Person.FullName}' ", StatusIdConst.MODIFIED);
                    #region Edoc      
                    AppointmentOrderDto appointmentOrderDto = null;

                    if (dto.EmployeeManageId.HasValue)
                    {
                        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
                            .Select(a => new
                            {
                                Id = a.Id,
                                OrganizationId = a.OrganizationId,
                                PersonId = a.Employee.PersonId,
                                PersonPinfl = a.Employee.Person.Pinfl,
                                DocumentId = a.DocId
                            })
                            .FirstOrDefault(a => a.Id == dto.EmployeeManageId.Value
                                && a.OrganizationId == dto.OrganizationId
                            );

                        if (employeeManage == null)
                            AddError($"Hodim topilmadi. Id {dto.EmployeeManageId.Value}");
                        dto.DocumentId = employeeManage.DocumentId;
                    }

                    if (dto.EmployeeManageId.HasValue && dto.DocumentId.HasValue)
                    {
                        var appointmentEmployee = _unitOfWork.Context.Set<AppointEmployee>()
                                                         .Include(x => x.Tables)
                                                         .FirstOrDefault(x => x.Id == dto.DocumentId.Value);
                        appointmentOrderDto = AppointmentOrderMap(appointmentEmployee, dto.EmployeeManageId.Value);
                    }
                    ///Edoc schema uchun userni sync qilish.
                    if (!HasErrors)
                        UpdateUserForEdocSchema(entity, appointmentOrderDto);
                    #endregion

                    if (HasErrors)
                        return;
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message}: {ex.InnerException?.Message}");
                _unitOfWork.Rollback();
            }

        }

        //public void Delete(int id)
        //{
        //    var entity = _repository.ById(id);
        //    if ((!_authService.HasPermission(ModuleCode.AllUserDelete, ModuleCode.BranchesUserDelete)
        //           && entity.OrganizationId != _authService.User.OrganizationId)
        //       || !_authService.HasPermission(ModuleCode.AllUserDelete))
        //    {
        //        AddError("У вас нет разрешения на удаление пользователя другой организации");
        //        return;
        //    }
        //    try
        //    {
        //        //_repository.Delete(id);
        //        entity.StateId = StateIdConst.PASSIVE;
        //        CombineStatuses(_repository);
        //        if (IsValid)
        //            _unitOfWork.Save();
        //        CreateChangeLog(entity.Id, $"User Deleted with '{entity.UserName}' - User name, and '{entity.PhoneNumber}' - Phone for '{entity.Person.FullName}' ", StatusIdConst.DELETED);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        AddError("Пользователь не может быть удален");
        //    }
        //}

        public bool IsUserNameBusy(CheckUserNameDto dto)
        {
            var user = _repository.ByUserName(dto.UserName);
            return user != null && user.Id != dto.Id;
        }

        private IQueryable<TDto> GetQuery<TDto>()
            where TDto : class
        {
            if (_authService.HasPermission(ModuleCode.AllUserView))
                return _repository.ReadAsNoTracked<TDto>();

            if (_authService.HasPermission(ModuleCode.BranchesUserView))
                return _repository.ReadAsNoTracked<TDto>();

            return _repository.ReadAsNoTracked<TDto>(a => a.OrganizationId == _authService.User.OrganizationId);
        }

        public Stream SaveAsExecel(UserSortFilterPageOptions dto)
        {
            var data = GetQuery<UserListDto>()
                        .SortFilter(dto)
                        .Filter(dto.Filters)
                        .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.USER_LIST));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);
                var namerange = excelPackage.Workbook.Names["Organization"];

                namerange = excelPackage.Workbook.Names["ImportRow"];
                var currentrow = namerange.Start.Row;
                var ws = namerange.Worksheet;
                int i = 1;
                foreach (var item in data)
                {
                    ws.InsertRow(currentrow, 1, namerange.Start.Row);
                    ws.Cells[currentrow, 1].Value = i++;
                    ws.Cells[currentrow, 2].Value = item.FullName;
                    ws.Cells[currentrow, 3].Value = item.UserName;
                    ws.Cells[currentrow, 4].Value = item.Organization;
                    //ws.Cells[currentrow, 5].Value = item.ParentOrganization;
                    ws.Cells[currentrow, 6].Value = item.Roles;
                    ws.Cells[currentrow, 7].Value = item.PhoneNumber;
                    ws.Cells[currentrow, 8].Value = item.Email;
                    ws.Cells[currentrow, 9].Value = item.State;
                    currentrow++;
                }
                ws.DeleteRow(namerange.Start.Row, 1);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;


        }
        private HaveId<int> CreateChangeLog(int userId, string message, int statusId)
        {
            var log = _documentChangeLogRepository.Create(new()
            {
                DateAt = DateTime.Now,
                DocId = userId,
                Message = message,
                StatusId = statusId,
                UserId = _authService.User.Id,
                TableId = TableIdConst.SYS_USER,
                UserInfo = _authService.User.ToTextForDocumentLog(),
                IpAddress = _authService.UserIp,
                UserAgent = _authService.UserAgent
            });

            _unitOfWork.Save();
            return HaveId.Create(userId);
        }
        public async Task ImportJusticeUser(List<ImportUserDlDto> listDto)
        {
            var organizationIds = _unitOfWork.OrganizationRepository.AllAsQueryable
                .AsEnumerable()
                .Where(a => listDto.Any(b => a.Inn == b.OrganizationInn))
                .ToDictionary(
                    a => a.Inn,
                    a => a.Id
                );

            var personIds = _unitOfWork.PersonRepository.AllAsQueryable
                .AsEnumerable()
                .Where(a =>
                    (
                        listDto.Any(b => a.PassportSeria == b.DirectorPassportSeria
                        && a.PassportNumber == b.DirectorPassportNumber
                        && a.BirthDate == b.DirectorBirthOn)
                    )
                )
                .ToDictionary(
                    a => $"{a.PassportSeria}-{a.PassportNumber}-{a.BirthDate}",
                    a => new { a.Id, a.Pinfl }
                );

            var userIds = _unitOfWork.Context.Set<User>()
                .AsEnumerable()
                .Where(a =>
                    (
                        listDto.Any(b => a.Person.PassportSeria == b.DirectorPassportSeria
                        && a.Person.PassportNumber == b.DirectorPassportNumber
                        && a.Person.BirthDate == b.DirectorBirthOn)
                    )
                )
                .ToDictionary(
                    a => $"{a.Person.PassportSeria}-{a.Person.PassportNumber}-{a.Person.BirthDate}",
                    a => a.Id
                );

            var defaultRoles = _unitOfWork.RoleRepository.AllAsQueryable
                .IsActive()
                .Where(a => a.IsDefault)
                .Select(a => a.Id)
                .ToList();

            try
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    foreach (var dto in listDto)
                    {
                        if (!organizationIds.ContainsKey(dto.OrganizationInn))
                        {
                            AddError($"Tizimda {dto.OrganizationInn} INN lik tashkilot topilmadi");
                            return;
                        }

                        if (!personIds.ContainsKey($"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"))
                        {
                            var person = await _personService.GetByPassportData(new WEBASE.Integration.MSPD.GSP.GSPPersonInfoRequestDto
                            {
                                DateOfBirth = dto.DirectorBirthOn.AsDateTime(),
                                Number = dto.DirectorPassportNumber,
                                Seria = dto.DirectorPassportSeria
                            });

                            if (person == null)
                            {
                                AddError($"Davlat personallashtirish markazidan ma'lumot topilmadi. Seria: {dto.DirectorPassportSeria}. Raqam: {dto.DirectorPassportNumber}. Tug'ilgan sanasi: {dto.DirectorBirthOn.ToString(Constants.DATE_FORMAT)}");
                                //continue;
                                return;
                            }

                            var mc = new AutoMapper.MapperConfiguration(cfg =>
                            {
                                cfg.CreateMap<PersonDto, CreatePersonDlDto>();
                            });
                            var createPersonDlDto = mc.CreateMapper().Map<CreatePersonDlDto>(person);
                            var personId = _personService.Create(createPersonDlDto).Id;
                            CombineStatuses(_personService);
                            if (HasErrors)
                                return;

                            personIds.Add($"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}", new { Id = personId, person.Pinfl });
                        }

                        if (!userIds.ContainsKey($"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"))
                        {
                            var user = Create(new CreateUserDto
                            {
                                LanguageId = LanguageIdConst.RU,
                                OrganizationId = organizationIds[dto.OrganizationInn],
                                PersonId = personIds[$"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"].Id,
                                Person = new CreatePersonDlDto
                                {
                                    Pinfl = personIds[$"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"].Pinfl
                                },
                                PhoneNumber = Regex.Replace(dto.DirectorPhoneNumber, @"\D", ""),
                                UserName = $"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}",
                                Password = $"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}",
                                Roles = defaultRoles
                            });
                            if (HasErrors)
                                return;

                            userIds.Add($"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}", user.Id);
                        }
                    }

                    if (IsValid)
                        transaction.Commit();
                }

            }
            catch (Exception ex)
            {
                AddError($"{ex.Message}: {ex.InnerException.Message}");
            }
        }

        #region Edoc Schema User sync
        public void SyncEdocUser(IDbContextTransaction outTransaction = null)
        {
            var users = _unitOfWork.Context.Set<User>()
                                    .Include(x => x.Person).OrderBy(x => x.Id).ToList();

            var transaction = outTransaction == null ? _unitOfWork.BeginTransaction() : outTransaction;
            try
            {

                foreach (var item in users)
                {
                    CreateUserForEdocSchema(item);
                    if (HasErrors)
                        return;
                }
                if (HasErrors)
                    return;

                if (outTransaction == null)
                    transaction.Commit();
            }
            catch (Exception e)
            {
                AddError(e.Message);
                _unitOfWork.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private void CreateUserForEdocSchema(User dto, AppointmentOrderDto? appointmentOrderDto = null)
        {
            try
            {
                var exsist = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.User>()
                                        .Any(x => x.Id == dto.Id);

                dto.Person = _unitOfWork.Context.Set<Person>()
                    .FirstOrDefault(a => a.Id == dto.PersonId);

                if (!exsist)
                {
                    var user = new Ssp.DataLayer.EFClasses.Edoc.User()
                    {
                        Id = dto.Id,
                        Avatar = Encoding.UTF8.GetBytes("empty"),
                        AvatarUrl = "empty",
                        AvatarId = Guid.NewGuid(),
                        CreatedUserId = dto.CreatedUserId.HasValue ? dto.CreatedUserId.Value : 1,
                        DateOfCreated = DateTime.Now,
                        DateOfModified = null,
                        DepartmentCode = dto.Id.ToString("000"),
                        Email = !string.IsNullOrEmpty(dto.Email) ? dto.Email : dto.Person.SurnameLatin + "@gmail.com",
                        FirstName = !string.IsNullOrEmpty(dto.Person.NameEng) ? dto.Person.NameEng : dto.Person.NameLatin,
                        LastName = !string.IsNullOrEmpty(dto.Person.SurnameEng) ? dto.Person.SurnameEng : dto.Person.SurnameLatin,
                        MiddleName = !string.IsNullOrEmpty(dto.Person.PatronymLatin) ? dto.Person.PatronymLatin : "",
                        Inn = dto.Person.Inn,
                        InPhoneNumber = !string.IsNullOrEmpty(dto.PhoneNumber) ? dto.PhoneNumber : "+998000000" + dto.Id.ToString(),
                        PhoneNumber = !string.IsNullOrEmpty(dto.PhoneNumber) ? dto.PhoneNumber : "+998000000" + dto.Id.ToString(),
                        Lang = "ru",
                        LastAccessTime = DateTime.Now,
                        ModifiedUserId = null,
                        OrganizationId = dto.OrganizationId,
                        PasswordHash = dto.PasswordHash,
                        PasswordSalt = dto.PasswordSalt,
                        ReplacementId = null,
                        StateId = dto.StateId,
                        UniqueId = Guid.NewGuid(),
                        UserName = dto.UserName,
                        ExternalId = dto.Id,
                        EmployeeManageId = dto.EmployeeManageId
                    };
                    user.SetFIO();
                    _unitOfWork.Context.Add(user);
                    _unitOfWork.Save();
                    if (user.EmployeeManageId.HasValue && appointmentOrderDto != null)
                        _appointmentOrderService.Create(user, appointmentOrderDto);

                }
                else
                {
                    UpdateUserForEdocSchema(dto, appointmentOrderDto);
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                if (ex.InnerException != null)
                    AddError(ex.InnerException.Message);
            }
        }

        public void UpdateUserForEdocSchema(User dto, AppointmentOrderDto? appointmentOrderDto = null)
        {
            try
            {
                var user = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.User>()
                    .FirstOrDefault(x => x.Id == dto.Id);
                dto.Person = _unitOfWork.Context.Set<Person>()
                    .FirstOrDefault(a => a.Id == dto.PersonId);

                if (user is not null)
                {
                    var oldManageId = user.EmployeeManageId;

                    user.Id = dto.Id;
                    user.Email = !string.IsNullOrEmpty(dto.Email) ? dto.Email : dto.Person.SurnameLatin + "@gmail.com";
                    user.FirstName = !string.IsNullOrEmpty(dto.Person.NameEng) ? dto.Person.NameEng : dto.Person.NameLatin;
                    user.FirstName = !string.IsNullOrEmpty(dto.Person.NameEng) ? dto.Person.NameEng : dto.Person.NameLatin;
                    user.LastName = !string.IsNullOrEmpty(dto.Person.SurnameEng) ? dto.Person.SurnameEng : dto.Person.SurnameLatin;
                    user.MiddleName = !string.IsNullOrEmpty(dto.Person.PatronymLatin) ? dto.Person.PatronymLatin : "";
                    user.Inn = dto.Person.Inn;
                    user.InPhoneNumber = !string.IsNullOrEmpty(dto.PhoneNumber) ? dto.PhoneNumber : "+998000000" + dto.Id.ToString();
                    user.PhoneNumber = !string.IsNullOrEmpty(dto.PhoneNumber) ? dto.PhoneNumber : "+998000000" + dto.Id.ToString();
                    user.LastAccessTime = DateTime.Now;
                    user.OrganizationId = dto.OrganizationId;
                    user.PasswordHash = dto.PasswordHash;
                    user.PasswordSalt = dto.PasswordSalt;
                    user.StateId = dto.StateId;
                    user.UserName = dto.UserName;
                    user.EmployeeManageId = dto.EmployeeManageId;
                    user.SetFIO();
                    _unitOfWork.Context.Update(user);
                    if (user.EmployeeManageId.HasValue && appointmentOrderDto != null)
                        _appointmentOrderService.Update(user, appointmentOrderDto);
                    if ((oldManageId.HasValue && oldManageId.Value > 0 && dto.EmployeeManageId == null) || (appointmentOrderDto == null && user.Id != 1))
                        _appointmentOrderService.Delete(user);

                    _unitOfWork.Context.SaveChanges();
                }
                else
                {
                    var entity = _unitOfWork.Context.Set<User>()
                    .FirstOrDefault(x => x.Id == dto.Id);
                    ///Edoc schema uchun userni sync qilish.
                    if (dto.EmployeeManageId.HasValue)
                    {
                        CreateUserForEdocSchema(entity, appointmentOrderDto);
                    }
                }
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message}: {ex.InnerException}");
            }
        }

        public AppointmentOrderDto AppointmentOrderMap(AppointEmployee dto, long employeeManageId)
        {
            var table = dto.Tables.FirstOrDefault(x => x.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL
                ? x.EmployeeManageId == employeeManageId
                : x.FromEmployeeManageId == employeeManageId
            );

            if (table == null)
            {
                AddError($"Сиз танлаган EmployeeManage Ишга қабул ҳужжатин ичида топилмади. EmployeeManage = {employeeManageId}, DocumentId : {dto.Id} ", nameof(employeeManageId));
                return null;
            }
            return new AppointmentOrderDto()
            {
                DocOn = GetDate(dto.DocOn),
                AppointmentTypeId = table.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL ? EmpAppointOrderTypeIdConst.HIRE : table.EmpAppointOrderTypeId,
                DepartmentId = table.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL ? table.DepartmentId.Value : table.FromDepartmentId.Value,
                PositionId = table.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL ? table.PositionId.Value : table.FromPositionId.Value,
                DocNumber = dto.DocNumber,
                EmployeeId = table.EmployeeId,
                EmployeeRate = table.EmployeeRate.Value,
                EndateOn = table.EndOn.HasValue ? GetDate(table.EndOn.Value) : null,
                StartOn = GetDate(table.StartOn),
                OrganizationId = dto.OrganizationId,
                StatusId = dto.StatusId
            };
        }

        private DateTime GetDate(DateOnly date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }



        #endregion
    }
}
