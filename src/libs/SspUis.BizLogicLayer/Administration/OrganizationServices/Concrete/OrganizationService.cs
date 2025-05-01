using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
//using RestSharp.Extensions;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.BizLogicLayer.PositionServices;
using SspUis.BizLogicLayer.UserServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.Integration.Soliq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WbAccessControl.Sdk;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.Manuals.Services;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationService
        : BaseEntityService<int, Organization, OrganizationListDto, OrganizationDto, CreateOrganizationDlDto, UpdateOrganizationDlDto, IOrganizationRepository, OrganizationSortFilterPageOptions>
        , IOrganizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ISoliqContractorService _soliqContractorService;
        private readonly IUOWIntegrationManuals _uowIntegrationManuals;
        private readonly SystemConf _systemConf;
        private readonly ICultureHelper _cultureHelper;
        private readonly IStorageService _storageService;
        private readonly IUserService _userService;
        private readonly IPersonService _personService;
        private readonly IDepartmentService _departmentService;
        private readonly IPositionService _positionService;
        private readonly IWbacClientService _wbacClientService;

        public OrganizationService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            ISoliqContractorService soliqContractorService,
            IUOWIntegrationManuals uowIntegrationManuals,
            IStorageService storageService,
            SystemConf systemConf,
            ICultureHelper cultureHelper,
            IUserService userService,
            IDepartmentService departmentService,
            IPositionService positionService,
            IPersonService personService,
            IWbacClientService wbacClientService)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _soliqContractorService = soliqContractorService;
            _uowIntegrationManuals = uowIntegrationManuals;
            _systemConf = systemConf;
            _cultureHelper = cultureHelper;
            _storageService = storageService;
            _personService = personService;
            _userService = userService;
            _departmentService = departmentService;
            _positionService = positionService;
            _wbacClientService = wbacClientService;
        }

        protected override IQueryable<OrganizationListDto> SortFilter(IQueryable<OrganizationListDto> query, OrganizationSortFilterPageOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options);
        }

        public override PagedResult<OrganizationListDto> GetList(OrganizationSortFilterPageOptions options)
        {
            var result = SortFilter(GetQuery<OrganizationListDto>(), options)
                                    .ToTableData(options);
            return result;
        }

        public override OrganizationDto Get()
        {
            //SyncEdocOrganization();

            var uzbName = _unitOfWork.CountryRepository.AllAsQueryable
                .Include(a => a.Translates)
                .Where(a => a.TextCode == CountryCodeConst.UZB)
                .Select(a => a.Translates.AsQueryable().FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName)
                .FirstOrDefault();

            return new OrganizationDto()
            {
                CountryId = CountryIdConst.UZB,
                Country = uzbName
            };
        }

        public override OrganizationDto Get(int id)
        {
            var dto = GetQuery<OrganizationDto>().FirstOrDefault(a => a.Id == id);
            if (dto == null)
                AddError("По вашему запросу запись не найдено");
            if (HasErrors)
                return null;

            return dto;
        }

        new public async Task<HaveId<int>> Create(CreateOrganizationDlDto dto)
        {
            if (!_authService.HasPermission(ModuleCode.AllOrganizationCreate))
            {
                AddError("У вас нет разрешения на создание других организации");
                return null;
            }

            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {

                var entity = Repository.Create(dto, ent => Validation(dto, ent));
                CombineStatuses(Repository);
                UnitOfWork.Save();
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null!;
                }
                if (dto.Files is not null)
                    _storageService.MoveToPersistent(
                        DocumentStorageConst.INFO_ORGANIZATION_FILES,
                        entity.Id.ToString(),
                        dto.Files.Select(f => f.Id).ToArray());

                var defaultRoles = _unitOfWork.RoleRepository.AllAsQueryable
                    .IsActive()
                    .Where(a => a.IsDefault)
                    .Select(a => a.Id)
                    .ToList();

                foreach (var signer in dto.Signs)
                {
                    bool existUser = _unitOfWork.UserRepository.AllAsQueryable
                        .Any(a => a.Person.Pinfl == signer.Pinfl);

                    if (!existUser)
                    {
                        int personId = 0;

                        var personEntity = _unitOfWork.PersonRepository.AllAsQueryable
                            .FirstOrDefault(a => !string.IsNullOrEmpty(signer.Pinfl) && a.Pinfl == signer.Pinfl);

                        if (personEntity == null)
                        {

                            var person = await _personService.GetByPassportData(new WEBASE.Integration.MSPD.GSP.GSPPersonInfoRequestDto
                            {
                                DateOfBirth = signer.BirthOn.ToDateTime(TimeOnly.MaxValue),
                                Number = signer.PassportNumber,
                                Seria = signer.PassportSeria
                            });

                            if (person == null)
                            {
                                AddError($"Davlat personallashtirish markazidan ma'lumot topilmadi. Seria: {signer.PassportSeria}. Raqam: {signer.PassportNumber}. Tug'ilgan sanasi: {signer.BirthOn.ToString(Constants.DATE_FORMAT)}");
                                return null;
                            }

                            var mc = new AutoMapper.MapperConfiguration(cfg =>
                            {
                                cfg.CreateMap<PersonDto, CreatePersonDlDto>();
                            });
                            var createPersonDlDto = mc.CreateMapper().Map<CreatePersonDlDto>(person);
                            personId = _personService.Create(createPersonDlDto).Id;
                            CombineStatuses(_personService);
                            if (HasErrors)
                                return null;
                        }
                        else
                            personId = personEntity.Id;

                        if (personEntity == null)
                            personEntity = _unitOfWork.PersonRepository.AllAsQueryable
                                .FirstOrDefault(a => !string.IsNullOrEmpty(signer.Pinfl) && a.Pinfl == signer.Pinfl);

                        if (!_unitOfWork.UserRepository.AllAsQueryable.Any(a => a.Person.PassportSeria == signer.PassportSeria && a.Person.PassportNumber == signer.PassportNumber))
                        {
                            var userName = $"{signer.PassportSeria}-{signer.PassportNumber}";
                            bool hasUserName = true;

                            int userNameIndex = 1;
                            while (hasUserName)
                            {
                                if (_unitOfWork.Context.Set<User>().Any(a => a.UserName == userName))
                                {
                                    userName = userName + userNameIndex;
                                    userNameIndex++;
                                }
                                else
                                    hasUserName = false;
                            }

                            var user = _userService.Create(new CreateUserDto
                            {
                                LanguageId = LanguageIdConst.RU,
                                OrganizationId = entity.Id,
                                PersonId = personId,
                                PhoneNumber = Regex.Replace(signer.PhoneNumber, @"\D", ""),
                                Person = new CreatePersonDlDto
                                {
                                    Pinfl = personEntity.Pinfl
                                },
                                UserName = userName,
                                Password = userName,
                                Roles = defaultRoles
                            });
                            signer.UserId = user.Id;
                        }
                        CombineStatuses(_userService);
                        if (HasErrors)
                            return null;
                    }
                }

                ///Edoc schema uchun Organizationni sync qilish.
                if (!HasErrors)
                    CreateOrganizationForEdocSchema(entity);

                if (HasErrors)
                    return null;

                if (entity.OrganizationGroupId == OrganizationGroupIdConst.SSP
                    || entity.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP)
                {
                    var wbacOrgCreateResult = await _wbacClientService.UpSertOrganizationAsync(new WbacOrganizationDto
                    {
                        OrganizationId = entity.Id,
                        ShortName = entity.ShortName,
                        FullName = entity.FullName,
                        Inn = entity.Inn,
                        Address = entity.Address,
                        PhoneNumber = entity.PhoneNumber
                    });

                    if (!wbacOrgCreateResult.IsSuccess || !wbacOrgCreateResult.Response)
                        CombineStatuses(wbacOrgCreateResult.GetStatusGeneric());
                }

                if (canCommit)
                    transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        new public async Task Update(UpdateOrganizationDlDto dto)
        {
            if (!_authService.HasPermission(ModuleCode.AllOrganizationEdit))
            {
                AddError("У вас нет разрешения на редактирование других организации");
                return;
            }

            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                // If signer change
                var changedSigners = dto.Signs.Where(signer => signer.ExpireOn.HasValue && signer.Id != 0).ToArray();

                var defaultRoles = _unitOfWork.RoleRepository.AllAsQueryable
                    .IsActive()
                    .Where(a => a.IsDefault)
                    .Select(a => a.Id)
                    .ToList();

                foreach (var signer in dto.Signs)
                {
                    bool existUser = _unitOfWork.UserRepository.AllAsQueryable.Include(a => a.Person)
                        .Any(a => a.Person.Pinfl == signer.Pinfl);

                    if (!existUser)
                    {
                        int personId = 0;

                        var personEntity = _unitOfWork.PersonRepository.AllAsQueryable
                            .FirstOrDefault(a => a.Pinfl == signer.Pinfl);

                        if (personEntity == null)
                        {
                            var person = await _personService.GetByPassportDataFromDigital(new GSPNewApiRequestDto
                            {
                                document = signer.PassportSeria + signer.PassportNumber,
                                transaction_id = 4,
                                birth_date = signer.BirthOn.ToString("dd.MM.yyyy"),
                                is_consent = "Y",
                                is_photo = "Y",
                                langId = 3
                            });

                            if (person == null)
                            {
                                AddError($"Davlat personallashtirish markazidan ma'lumot topilmadi. Seria: {signer.PassportSeria}. Raqam: {signer.PassportNumber}. Tug'ilgan sanasi: {signer.BirthOn.ToString(Constants.DATE_FORMAT)}");
                                return;
                            }

                            var mc = new AutoMapper.MapperConfiguration(cfg =>
                            {
                                cfg.CreateMap<PersonDto, CreatePersonDlDto>();
                            });
                            var createPersonDlDto = mc.CreateMapper().Map<CreatePersonDlDto>(person);
                            personId = _personService.Create(createPersonDlDto).Id;
                            CombineStatuses(_personService);
                            if (HasErrors)
                            {
                                AddError($"Davlat personallashtirish markazidan ma'lumot topilmadi.");
                                return;
                            }
                        }
                        else
                            personId = personEntity.Id;

                        if (personEntity == null)
                            personEntity = _unitOfWork.PersonRepository.AllAsQueryable
                                .FirstOrDefault(a => a.Pinfl == signer.Pinfl);


                        if (!_unitOfWork.UserRepository.AllAsQueryable.Any(a => a.Person.PassportSeria == signer.PassportSeria && a.Person.PassportNumber == signer.PassportNumber))
                        {
                            var userName = $"{signer.PassportSeria}-{signer.PassportNumber}";
                            bool hasUserName = true;

                            int userNameIndex = 1;
                            while (hasUserName)
                            {
                                if (_unitOfWork.Context.Set<User>().Any(a => a.UserName == userName))
                                {
                                    userName = userName + userNameIndex;
                                    userNameIndex++;
                                }
                                else
                                    hasUserName = false;
                            }

                            var user = _userService.Create(new CreateUserDto
                            {
                                LanguageId = LanguageIdConst.RU,
                                OrganizationId = dto.Id,
                                PersonId = personId,
                                PhoneNumber = Regex.Replace(signer.PhoneNumber, @"\D", ""),
                                Person = new CreatePersonDlDto
                                {
                                    Pinfl = personEntity.Pinfl
                                },
                                UserName = userName,
                                Password = userName,
                                Roles = defaultRoles
                            });
                            CombineStatuses(_userService);
                            if (HasErrors)
                                return;
                            _unitOfWork.Save();
                            signer.UserId = user.Id;

                            if (HasErrors)
                                return;
                        }
                    }
                }

                var entity = Repository.Update(dto, ent => Validation(dto, ent));

                CombineStatuses(Repository);
                _unitOfWork.Save();
                if (HasErrors)
                    return;

                foreach (var signer in changedSigners)
                {
                    var newSigner = entity.Signs.Where(a => !a.ExpireOn.HasValue).FirstOrDefault(a => a.PrtnContractTypeTableId == signer.PrtnContractTypeTableId);

                    if (newSigner == null)
                    {
                        AddError("New signer not found");
                        return;
                    }
                    var prtnContractSigners = _unitOfWork.PrtnContractRepository.AllAsQueryable
                        .Where(a => new int[]
                        {
                                StatusIdConst.CREATED,
                                StatusIdConst.SENT_FOR_EXPERTISE,
                                StatusIdConst.NOT_PASS_EXPERTISE,
                                StatusIdConst.SIGNING,
                                StatusIdConst.PASS_EXPERTISE
                        }.Contains(a.StatusId) && a.Signs.Any(a => a.OrganizationSignId == signer.Id))
                        .SelectMany(a => a.Signs.Where(b => b.OrganizationSignId == signer.Id).Select(a => new { a.OwnerId, a.Owner.DocOn, a.Id }))
                        .ToList();

                    foreach (var prtnContractSigner in prtnContractSigners)
                    {
                        _unitOfWork.PrtnContractRepository.AllAsQueryable.Lock(prtnContractSigner.OwnerId);

                        _unitOfWork.PrtnContractRepository.ChangeSigner(prtnContractSigner.Id, newSigner.Id);
                        CombineStatuses(_unitOfWork.PrtnContractRepository);
                        if (HasErrors)
                            return;

                        if (prtnContractSigner.DocOn < signer.ExpireOn.Value)
                        {
                            _unitOfWork.PrtnContractRepository.ChangeDate(prtnContractSigner.OwnerId, DateTime.Now.AsDateOnly());
                            CombineStatuses(_unitOfWork.PrtnContractRepository);
                            if (HasErrors)
                                return;
                        }

                    }

                    UnitOfWork.Save();
                    if (HasErrors)
                        return;
                }

                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                UnitOfWork.Save();

                ///Edoc schema uchun Organizationni sync qilish.
                if (!HasErrors)
                    UpdateOrganizationForEdocSchema(entity);

                if (HasErrors)
                    return;

                _storageService.ResolveMarkedFiles(
                    DocumentStorageConst.INFO_ORGANIZATION_FILES,
                    entity.Id.ToString());
                CombineStatuses(_storageService);

                if (HasErrors)
                    return;

                if (entity.OrganizationGroupId == OrganizationGroupIdConst.SSP
                    || entity.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP)
                {
                    var wbacOrgCreateResult = await _wbacClientService.UpSertOrganizationAsync(new WbacOrganizationDto
                    {
                        OrganizationId = entity.Id,
                        ShortName = entity.ShortName,
                        FullName = entity.FullName,
                        Inn = entity.Inn,
                        Address = entity.Address,
                        PhoneNumber = entity.PhoneNumber
                    });

                    if (!wbacOrgCreateResult.IsSuccess || !wbacOrgCreateResult.Response)
                        CombineStatuses(wbacOrgCreateResult.GetStatusGeneric());
                }

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception e)
            {
                AddError($"{e.Message}, {e.InnerException}, {e.HelpLink}, {e.Source}, {e.Data}, {e.HResult}, {e.StackTrace}");
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public override void Delete(int id)
        {
            var entity = Repository.ById(id);
            if (!_authService.HasPermission(ModuleCode.AllOrganizationDelete))
            {
                AddError("У вас нет разрешения на удаление других организации");
                return;
            }
            base.Delete(id);
        }

        public void UpdateStructure(int organizationId, int? structureId)
        {
            var entity = Repository.Context.Set<Organization>().FirstOrDefault(x => x.Id == organizationId);
            entity.OrganizationalStructureId = structureId;
            Repository.Context.SaveChanges();
        }

        public SelectList<int> AsSelectList(int? parentId = null, bool authorizedOnly = false, bool inspectionOnly = false, int? signOrganizationTypeId = null, int? organizationGroupId = null)
        {
            if (authorizedOnly)
            {
                return Repository.AllAsQueryable
                                 .IsActive()
                                 .Where(a => a.ParentId == _systemConf.DefaultOrganizationId
                                          || a.Id == _systemConf.DefaultOrganizationId)
                                 .AsSelectList();
            }
            var query = Repository.AllAsQueryable.IsActive();
            if (inspectionOnly)
                query = query.Where(a => a.ParentId != _systemConf.DefaultOrganizationId
                                      && a.Id != _systemConf.DefaultOrganizationId);

            return query.Where(a => (!parentId.HasValue || a.Id == parentId || a.ParentId == parentId)
                && (!signOrganizationTypeId.HasValue || a.SignOrganizationTypeId == signOrganizationTypeId.Value) && (!organizationGroupId.HasValue || a.OrganizationGroupId == organizationGroupId)
            ).AsSelectList();
        }
        public SelectList<int> AsSelectListByGroup(int? groupId)
        {
            groupId = groupId ?? OrganizationGroupIdConst.REGIONAL_BRANCH;
            return Repository.AllAsQueryable
            .Where(x => x.OrganizationGroupId == groupId)
            .AsSelectList();
        }


        public SelectList<int> SelectListByRegionInOrganizations(int? regionId)
        {

            return Repository.AllAsQueryable
            .Where(x => x.RegionId == regionId
            && ((x.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
            || (x.OrganizationGroupId == OrganizationGroupIdConst.SSP)
            || (x.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP)))
            .AsSelectList();
        }


        public SelectList<long> AsSelectListOrgSettlementAccount()
        {
            return _unitOfWork.Context.Set<OrganizationSettlementAccount>()
                .Where(x => x.OrganizationId == _authService.User.OrganizationId)
                .AsSelectListItem();
        }
        public SelectList<int> AsSelectListOrgForBank(int? langId)
        {
            return _unitOfWork.Context.Set<Organization>().Include(a => a.Translates)
                .Where(x => x.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH || x.OrganizationGroupId == OrganizationGroupIdConst.SSP)
                .AsSelectListForBank(langId);
        }

        public async Task<OrganizationDto> GetByInn(string inn)
        {
            var org = await _soliqContractorService.GetByInn(inn);
            CombineStatuses(_soliqContractorService);
            if (HasErrors)
                return null;

            var dto = new OrganizationDto
            {
                Inn = inn,
                ShortName = org.Company.ShortName,
                FullName = org.Company.Name,
                Director = $"{org.Director.LastName} {org.Director.FirstName} {org.Director.MiddleName ?? ""}",
                Address = org.CompanyBillingAddress.StreetName,
            };

            if (org.Accountant != null)
            {
                dto.Accounter = $"{org.Accountant.LastName} {org.Accountant.FirstName} {org.Accountant.MiddleName ?? ""}";
            }

            var oked = UnitOfWork.OkedRepository.ByCode(org.Company.Oked);
            if (oked != null)
            {
                dto.OkedId = oked.Id;
                dto.Oked = oked.Translates.AsQueryable()
                           .FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? oked?.FullName;
            }

            Bank bank;
            foreach (var companyBank in org.CompanyBanks)
            {
                bank = UnitOfWork.BankRepository.ByCode(companyBank.Mfo);

                dto.SettlementAccounts.Add(new OrganizationSettlementAccountDto
                {
                    BankId = bank.Id,
                    BankCode = bank.Code,
                    Bank = bank.BankName,
                    AccountCode = companyBank.PaymentAccount,
                    AccountName = companyBank.PaymentAccount,
                    StateId = StateIdConst.ACTIVE
                });
            }

            var district = UnitOfWork.DistrictRepository.BySoato($"{org.CompanyBillingAddress.District.Code}");
            if (district != null)
            {
                dto.DistrictId = district.Id;
                dto.District = district.Translates.AsQueryable()
                                       .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? district.FullName;
                var region = UnitOfWork.RegionRepository.ById(district.RegionId);
                var country = UnitOfWork.CountryRepository.ById(region.CountryId);
                dto.CountryId = country.Id;
                dto.Country = country.Translates.AsQueryable()
                                   .FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? country.FullName;

                dto.RegionId = region.Id;
                dto.Region = region.Translates.AsQueryable()
                                   .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? region.FullName;
            }

            return dto;
        }

        private IQueryable<TDto> GetQuery<TDto>()
            where TDto : class
        {
            return Repository.ReadAsNoTracked<TDto>();
        }

        public Stream SaveAsExecel(OrganizationSortFilterPageOptions dto)
        {
            var data = GetQuery<OrganizationListDto>()
                        .SortFilter(dto)
                        .Filter(dto.Filters)
                        .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.ORGANIZATIOM_LIST));

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["Organization"];
            namerange.Value = "";

            if (IsValid && data != null)
            {
                namerange = excelPackage.Workbook.Names["ImportRow"];
                var currentrow = namerange.Start.Row;
                var ws = namerange.Worksheet;
                int i = 1;
                foreach (var item in data)
                {
                    ws.InsertRow(currentrow, 1, namerange.Start.Row);
                    ws.Cells[currentrow, 1].Value = i++;
                    ws.Cells[currentrow, 2].Value = item.FullName;
                    ws.Cells[currentrow, 3].Value = item.Inn;
                    ws.Cells[currentrow, 4].Value = item.OrderCode;
                    ws.Cells[currentrow, 5].Value = item.Region;
                    ws.Cells[currentrow, 6].Value = item.District;
                    ws.Cells[currentrow, 7].Value = item.Address;
                    ws.Cells[currentrow, 8].Value = item.Oked;
                    ws.Cells[currentrow, 9].Value = item.Director;
                    ws.Cells[currentrow, 10].Value = item.PhoneNumber;
                    ws.Cells[currentrow, 11].Value = item.Parent;
                    ws.Cells[currentrow, 12].Value = item.State;
                    currentrow++;
                }
                ws.DeleteRow(namerange.Start.Row, 1);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }
        public Organization GetOrganizationByLocation(int regionId, int districtId, int prtnContractTypeId)
        {
            var orgQuery = Repository.AllAsQueryable.Include(a => a.Translates).Include(a => a.Signs).IsActive();

            if (prtnContractTypeId == PrtnContractTypeIdConst._50_100)
            {
                orgQuery = orgQuery.Where(a => a.DistrictId == districtId
                    && a.RegionId == regionId
                    && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.DISTRICT
                );
            }
            else if (prtnContractTypeId == PrtnContractTypeIdConst._101_200)
            {
                orgQuery = orgQuery.Where(a => a.RegionId == regionId
                    && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION
                );
            }
            else if (prtnContractTypeId == PrtnContractTypeIdConst._201__)
            {
                orgQuery = orgQuery.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY);
            }

            var organization = orgQuery.Include(a => a.Translates).FirstOrDefault();

            if (organization == null)
            {
                AddError("Tashkilot topilmadi / Организация не найдено");
                return null;
            }
            return organization;
        }

        public string GetOrganizationNameByLocation(int regionId, int districtId, int prtnContractTypeId)
        {
            var orgQuery = Repository.AllAsQueryable.Include(a => a.Translates).IsActive();

            if (prtnContractTypeId == PrtnContractTypeIdConst._50_100)
            {
                orgQuery = orgQuery.Where(a => a.DistrictId == districtId
                    && a.RegionId == regionId
                    && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.DISTRICT
                );
            }
            else if (prtnContractTypeId == PrtnContractTypeIdConst._101_200)
            {
                orgQuery = orgQuery.Where(a => a.RegionId == regionId
                    && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION
                );
            }
            else if (prtnContractTypeId == PrtnContractTypeIdConst._201__)
            {
                orgQuery = orgQuery.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY);
            }

            var organization = orgQuery.Include(a => a.Translates).FirstOrDefault();

            if (organization == null)
            {
                AddError("Tashkilot topilmadi / Организация не найдено");
                return null;
            }

            var orgFullName = organization.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                ?? organization.FullName;

            return orgFullName;
        }

        private void Validation<TDto>(OrganizationDlDto<TDto> dto, Organization entity)
            where TDto : OrganizationDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null)
            {
                query = query.Where(a => a.Id != entity.Id);
            }

            if (!dto.Inn.NullOrEmpty() && query.ByInn(dto.Inn, isIncludePassive: true).Any())
                AddError($"Bunday INN li tashkilot ({dto.Inn}) ro'yxatdan o'tgan / Организация с этим ИНН ({dto.Inn}) уже существует.", nameof(dto.Inn));

            if (dto.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION)
            {
                if (query.Any(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION && a.RegionId == dto.RegionId))
                    AddError($"Bu viloyatda tashkilot ro'yxatdan o'tgan / В этом регионе зарегистрирована организация.", nameof(dto.Inn));
            }
            // Agar bitta imzo chekadian joyga 2ta odam biriktirib qoygan bo'sa
            if (dto.Signs.Where(a => !a.ExpireOn.HasValue).GroupBy(a => a.PrtnContractTypeTableId).Any(a => a.Count() > 1))
                AddError("Bir lavozimga 2ta kishi biriktirib bo'lmaydi / Вы не можете привязать двух людей к одной должности");

            // Agar Imzolovchi boshqa tashkilotda IMZOLOVCHI sifatida tanlangan bo'lsa 
            if (entity != null)
            {
                foreach (var item in dto.Signs)
                {
                    if (item.Id == 0 && item.ExpireOn == null)
                    {
                        var duplicateUserInOrganization = query
                            .Count(a => a.Signs.Any(a => a.Pinfl == item.Pinfl && a.ExpireOn == null));

                        if (duplicateUserInOrganization > 0)
                        {
                            var userOrganizations = _unitOfWork.Context.Set<Organization>()
                                .Where(a => a.Signs.Any(s => s.Pinfl == item.Pinfl))
                                .FirstOrDefault();

                            if (userOrganizations != null)
                            {
                                AddError($"{item.LastName} {item.FirstName} {item.MiddleName} {userOrganizations.FullName} da imzo chekuvchi sifatida kiritilgan");
                            }
                        }
                    }
                }
            }
            // Agar hisobraqam boshqa tashkilotda hisobraqami bo'lsa 
            if (dto is CreateOrganizationDlDto)
            {
                foreach (var item in dto.SettlementAccounts)
                {
                    if (query.Any(a => a.SettlementAccounts.Any(a => a.AccountCode == item.AccountCode)))
                    {
                        var orgName = _unitOfWork.Context.Set<Organization>().Where(a => a.SettlementAccounts.Any(b => b.AccountCode == item.AccountCode)).FirstOrDefault();
                        Repository.AddError($" {item.AccountCode} Bunday shaxsiy g'azna hisobraqami {orgName.FullName} tashkilotiga biriktirilgan");
                    }
                }
            }

            // Agar qaysidir lavozimdagi odamni ishdan bo'shatsa, usha lavozimga yangi odamni ko'rsatishi shart
            if (dto.Signs.Any(a => a.ExpireOn.HasValue))
            {
                foreach (var sign in dto.Signs.Where(a => a.ExpireOn.HasValue))
                {
                    if (dto.Signs.Where(a => !a.ExpireOn.HasValue).All(a => a.PrtnContractTypeTableId != sign.PrtnContractTypeTableId))
                    {
                        var prtnContractTypePosition = _unitOfWork.Context.Set<PrtnContractTypeTable>()
                            .Where(a => a.Id == sign.PrtnContractTypeTableId)
                            .Select(a => a.Position.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Position.FullName)
                            .FirstOrDefault();

                        AddError($"Iltimos {prtnContractTypePosition} lavozimdagi hodimni ko'rsating / Пожалуйста, выберите человека на должность {prtnContractTypePosition}");
                    }
                }
            }
        }

        /// <summary>
        /// Tizim boshida bir marta tashkilot va foydalanuvchilarni JSON dan import qilish uchun.
        /// Keyinchalik bu API yopiladi.
        /// </summary>
        /// <param name="dto"></param>
        public async Task<string> CheckPersonFromGsp(List<CheckPersonFromGspDlDto> listDto)
        {
            string errors = "";
            foreach (var dto in listDto)
            {
                var birthOn = new DateTime(dto.Year, dto.Month, dto.Day);
                var person = await _personService.GetByPassportData(new WEBASE.Integration.MSPD.GSP.GSPPersonInfoRequestDto
                {
                    DateOfBirth = birthOn,
                    Number = dto.Number,
                    Seria = dto.Seria
                });

                if (person == null)
                {
                    errors += $"Davlat personallashtirish markazidan ma'lumot topilmadi. Seria: {dto.Seria}. Raqam: {dto.Number}. Tug'ilgan sanasi: {birthOn.ToString(Constants.DATE_FORMAT)} \n";
                }

            }
            return errors;

            //string errors = "";
            //foreach (var dto in listDto) 
            //{
            //    var organization = await GetByInn(inn: dto.OrganizationInn);
            //    if (organization == null)
            //    {
            //        errors += $"{dto.OrganizationInn} ";
            //        this._errors.Clear();
            //    }
            //}

            //var regionInns = new string[]
            //{
            //    "201943244", "200838399", "200151400", "201317491", "200056383", "201980453", "200006555", "201999056",
            //    "200342788", "201512528", "200237839", "201212624", "201623064"
            //};
            //var ministryInns = new string[]
            //{
            //    "201122919", "201190645"
            //};
            //
            //var organizationIds = Repository.AllAsQueryable.Where(a => listDto.Select(a => a.OrganizationInn).Contains(a.Inn))
            //    .ToDictionary(
            //        a => a.Inn,
            //        a => a.Id
            //    );
            //
            //var personIds = _unitOfWork.PersonRepository.AllAsQueryable
            //    .AsEnumerable()
            //    .Where(a =>
            //        (
            //            listDto.Any(b => a.PassportSeria == b.DirectorPassportSeria
            //            && a.PassportNumber == b.DirectorPassportNumber
            //            && a.BirthDate == b.DirectorBirthOn)
            //        )
            //        ||
            //        (
            //            listDto.Any(b => a.PassportSeria == b.DeputyPassportSeria
            //            && a.PassportNumber == b.DeputyPassportNumber
            //            && a.BirthDate == b.DeputyBirthOn)
            //        )
            //    )
            //    .ToDictionary(
            //        a => $"{a.PassportSeria}-{a.PassportNumber}-{a.BirthDate}",
            //        a => new { a.Id, a.Pinfl }
            //    );
            //
            //var userIds = _unitOfWork.Context.Set<User>()
            //    .AsEnumerable()
            //    .Where(a =>
            //        (
            //            listDto.Any(b => a.Person.PassportSeria == b.DirectorPassportSeria
            //            && a.Person.PassportNumber == b.DirectorPassportNumber
            //            && a.Person.BirthDate == b.DirectorBirthOn)
            //        )
            //        ||
            //        (
            //            listDto.Any(b => a.Person.PassportSeria == b.DeputyPassportSeria
            //            && a.Person.PassportNumber == b.DeputyPassportNumber
            //            && a.Person.BirthDate == b.DeputyBirthOn)
            //        )
            //    )
            //    .ToDictionary(
            //        a => $"{a.Person.PassportSeria}-{a.Person.PassportNumber}-{a.Person.BirthDate}",
            //        a => a.Id
            //    );
            //
            //var defaultRoles = _unitOfWork.RoleRepository.AllAsQueryable
            //    .IsActive()
            //    .Where(a => a.IsDefault)
            //    .Select(a => a.Id)
            //    .ToList();
            //
            //try
            //{
            //    using (var transaction = _unitOfWork.BeginTransaction())
            //    {
            //        foreach (var dto in listDto)
            //        {
            //            // Organization yengi bo'sa, Signer larini qoshish uchun kerak
            //            UpdateOrganizationDlDto newOrganization = null;
            //            List<OrganizationSignDlDto> newOrganizationSigners = new List<OrganizationSignDlDto>();
            //
            //            // Initialize organization if not exists in database
            //            if (!organizationIds.ContainsKey(dto.OrganizationInn))
            //            {
            //                var organization = await GetByInn(inn: dto.OrganizationInn);
            //                if (organization == null)
            //                {
            //                    //this._errors.Clear();
            //                    //continue;
            //                    return;
            //                }
            //                organization.CountryId = CountryIdConst.UZB;
            //
            //                var mc = new AutoMapper.MapperConfiguration(cfg =>
            //                {
            //                    cfg.CreateMap<OrganizationDto, CreateOrganizationDlDto>();
            //                    cfg.CreateMap<CreateOrganizationDlDto, UpdateOrganizationDlDto>();
            //                });
            //                var createOrganizationDlDto = mc.CreateMapper().Map<CreateOrganizationDlDto>(organization);
            //                createOrganizationDlDto.SignOrganizationTypeId = SignOrganizationTypeIdConst.DISTRICT;
            //                if (regionInns.Contains(dto.OrganizationInn))
            //                    createOrganizationDlDto.SignOrganizationTypeId = SignOrganizationTypeIdConst.REGION;
            //                else if (ministryInns.Contains(dto.OrganizationInn))
            //                    createOrganizationDlDto.SignOrganizationTypeId = SignOrganizationTypeIdConst.MINISTRY;
            //
            //                var organizationId = Create(createOrganizationDlDto).Result.Id;
            //                if (HasErrors)
            //                    return;
            //
            //                newOrganization = mc.CreateMapper().Map<UpdateOrganizationDlDto>(createOrganizationDlDto);
            //                newOrganization.Id = organizationId;
            //                newOrganization.StateId = StateIdConst.ACTIVE;
            //                organizationIds.Add(createOrganizationDlDto.Inn, organizationId);
            //            }
            //
            //            if (!personIds.ContainsKey($"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"))
            //            {
            //                var person = await _personService.GetByPassportData(new WEBASE.Integration.MSPD.GSP.GSPPersonInfoRequestDto
            //                {
            //                    DateOfBirth = dto.DirectorBirthOn,
            //                    Number = dto.DirectorPassportNumber,
            //                    Seria = dto.DirectorPassportSeria
            //                });
            //
            //                if (person == null)
            //                {
            //                    AddError($"Davlat personallashtirish markazidan ma'lumot topilmadi. Seria: {dto.DirectorPassportSeria}. Raqam: {dto.DirectorPassportNumber}. Tug'ilgan sanasi: {dto.DirectorBirthOn.ToString(Constants.DATE_FORMAT)}");
            //                    //continue;
            //                    return;
            //                }
            //
            //                var mc = new AutoMapper.MapperConfiguration(cfg =>
            //                {
            //                    cfg.CreateMap<PersonDto, CreatePersonDlDto>();
            //                });
            //                var createPersonDlDto = mc.CreateMapper().Map<CreatePersonDlDto>(person);
            //                var personId = _personService.Create(createPersonDlDto).Id;
            //                CombineStatuses(_personService);
            //                if (HasErrors)
            //                    return;
            //
            //                if (newOrganization != null)
            //                {
            //                    int prtnContractTypeTableId = PrtnContractTypeTableIdConst.DISTRICT__DIRECTOR;
            //                    if (regionInns.Contains(dto.OrganizationInn))
            //                        prtnContractTypeTableId = PrtnContractTypeTableIdConst.REGION__DIRECTOR;
            //                    else if (ministryInns.Contains(dto.OrganizationInn))
            //                        throw new Exception("Vazirliklar shartnoma turida hali ko'rsatilmagan");
            //
            //                    newOrganizationSigners.Add(new OrganizationSignDlDto
            //                    {
            //                        FirstName = createPersonDlDto.NameLatin,
            //                        LastName = createPersonDlDto.SurnameLatin,
            //                        MiddleName = createPersonDlDto.PatronymLatin,
            //                        PassportSeria = createPersonDlDto.PassportSeria,
            //                        PassportNumber = createPersonDlDto.PassportNumber,
            //                        BirthOn = createPersonDlDto.BirthDate.AsDateOnly(),
            //                        Pinfl = createPersonDlDto.Pinfl,
            //                        PrtnContractTypeTableId = prtnContractTypeTableId
            //                    });
            //                }
            //
            //                personIds.Add($"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}", new { Id = personId, person.Pinfl });
            //            }
            //
            //            if (!userIds.ContainsKey($"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"))
            //            {
            //                var user = _userService.Create(new CreateUserDto
            //                {
            //                    LanguageId = LanguageIdConst.RU,
            //                    OrganizationId = organizationIds[dto.OrganizationInn],
            //                    PersonId = personIds[$"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"].Id,
            //                    Person = new CreatePersonDlDto
            //                    {
            //                        Pinfl = personIds[$"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"].Pinfl
            //                    },
            //                    PhoneNumber = Regex.Replace(dto.DirectorPhoneNumber, @"\D", ""),
            //                    UserName = $"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}",
            //                    Password = $"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}",
            //                    Roles = defaultRoles
            //                });
            //                CombineStatuses(_userService);
            //                if (HasErrors)
            //                    return;
            //
            //                userIds.Add($"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}", user.Id);
            //            }
            //
            //
            //            if (!personIds.ContainsKey($"{dto.DeputyPassportSeria}-{dto.DeputyPassportNumber}-{dto.DeputyBirthOn}"))
            //            {
            //                var person = await _personService.GetByPassportData(new WEBASE.Integration.MSPD.GSP.GSPPersonInfoRequestDto
            //                {
            //                    DateOfBirth = dto.DeputyBirthOn,
            //                    Number = dto.DeputyPassportNumber,
            //                    Seria = dto.DeputyPassportSeria
            //                });
            //
            //                if (person == null)
            //                {
            //                    AddError($"Davlat personallashtirish markazidan ma'lumot topilmadi. Seria: {dto.DeputyPassportSeria}. Raqam: {dto.DeputyPassportNumber}. Tug'ilgan sanasi: {dto.DeputyBirthOn.ToString(Constants.DATE_FORMAT)}");
            //                    //continue;
            //                    return;
            //                }
            //
            //                var mc = new AutoMapper.MapperConfiguration(cfg =>
            //                {
            //                    cfg.CreateMap<PersonDto, CreatePersonDlDto>();
            //                });
            //                var createPersonDlDto = mc.CreateMapper().Map<CreatePersonDlDto>(person);
            //                var personId = _personService.Create(createPersonDlDto).Id;
            //                CombineStatuses(_personService);
            //                if (HasErrors)
            //                    return;
            //
            //                if (newOrganization != null)
            //                {
            //                    int prtnContractTypeTableId = PrtnContractTypeTableIdConst.DISTRICT__DEPUTY;
            //                    if (regionInns.Contains(dto.OrganizationInn))
            //                        prtnContractTypeTableId = PrtnContractTypeTableIdConst.REGION__DEPUTY;
            //                    else if (ministryInns.Contains(dto.OrganizationInn))
            //                    {
            //                        if (dto.OrganizationInn == "201190645")
            //                            prtnContractTypeTableId = PrtnContractTypeTableIdConst.MINISTRY__DIRECTOR;
            //                        else prtnContractTypeTableId = PrtnContractTypeTableIdConst.MINISTRY__DEPUTY;
            //                    }
            //
            //                    newOrganizationSigners.Add(new OrganizationSignDlDto
            //                    {
            //                        FirstName = createPersonDlDto.NameLatin,
            //                        LastName = createPersonDlDto.SurnameLatin,
            //                        MiddleName = createPersonDlDto.PatronymLatin,
            //                        PassportSeria = createPersonDlDto.PassportSeria,
            //                        PassportNumber = createPersonDlDto.PassportNumber,
            //                        BirthOn = createPersonDlDto.BirthDate.AsDateOnly(),
            //                        Pinfl = createPersonDlDto.Pinfl,
            //                        PrtnContractTypeTableId = prtnContractTypeTableId
            //                    });
            //                }
            //
            //                personIds.Add($"{dto.DeputyPassportSeria}-{dto.DeputyPassportNumber}-{dto.DeputyBirthOn}", new { Id = personId, person.Pinfl });
            //            }
            //
            //            if (!userIds.ContainsKey($"{dto.DeputyPassportSeria}-{dto.DeputyPassportNumber}-{dto.DeputyBirthOn}"))
            //            {
            //                var user = _userService.Create(new CreateUserDto
            //                {
            //                    LanguageId = LanguageIdConst.RU,
            //                    OrganizationId = organizationIds[dto.OrganizationInn],
            //                    PersonId = personIds[$"{dto.DeputyPassportSeria}-{dto.DeputyPassportNumber}-{dto.DeputyBirthOn}"].Id,
            //                    Person = new CreatePersonDlDto
            //                    {
            //                        Pinfl = personIds[$"{dto.DirectorPassportSeria}-{dto.DirectorPassportNumber}-{dto.DirectorBirthOn}"].Pinfl
            //                    },
            //                    PhoneNumber = Regex.Replace(dto.DeputyPhoneNumber, @"\D", ""),
            //                    UserName = $"{dto.DeputyPassportSeria}-{dto.DeputyPassportNumber}",
            //                    Password = $"{dto.DeputyPassportSeria}-{dto.DeputyPassportNumber}",
            //                    Roles = defaultRoles
            //                });
            //                CombineStatuses(_userService);
            //                if (HasErrors)
            //                    return;
            //
            //                userIds.Add($"{dto.DeputyPassportSeria}-{dto.DeputyPassportNumber}-{dto.DeputyBirthOn}", user.Id);
            //            }
            //
            //            if (newOrganization != null)
            //            {
            //                newOrganization.Signs.AddRange(newOrganizationSigners);
            //                await Update(newOrganization);
            //            }
            //        }
            //
            //        if (IsValid)
            //            transaction.Commit();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    AddError($"{ex.Message}: {ex.InnerException.Message}");
            //}
            //
        }

        #region Files
        public IEnumerable<OrganizationFileDto> UploadFiles(params StorageFile[] files)
        {
            if (files.Count() == 0)
            {
                AddError("Файл не прикреплен");
                return null;
            }

            var result = _storageService
                .SaveTemp(DocumentStorageConst.INFO_ORGANIZATION_FILES, files)
                .Select(a => new OrganizationFileDto
                {
                    Id = a.FileId,
                    FileName = a.FileName
                });
            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }

        public StorageFile DownloadFile(Guid fileId)
        {
            var entity = _unitOfWork.Context.Set<OrganizationFile>().FirstOrDefault(a => a.Id == fileId);
            return Download(fileId, entity, DocumentStorageConst.INFO_ORGANIZATION_FILES);
        }

        public void DeleteFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<OrganizationFile>()
                .FirstOrDefault(a => a.Id == fileId);

            Delete(fileId, entity, DocumentStorageConst.INFO_ORGANIZATION_FILES);
        }

        private void Delete(Guid fileId, FileEntity<int> entity, string storageDocument)
        {
            if (entity == null)
            {
                _storageService.DeleteTemp(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
        }

        private StorageFile Download(Guid fileId, FileEntity<int> entity, string storageDocument)
        {
            StorageFile file;
            try
            {
                if (entity == null)
                {
                    file = _storageService.GetTempFile(storageDocument, fileId);
                    CombineStatuses(_storageService);
                }
                else
                {
                    file = _storageService.GetFile(storageDocument, entity.OwnerId.ToString(), fileId);
                    CombineStatuses(_storageService);

                    if (IsValid)
                        file.FileName = entity.FileName;
                }

                return file;
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }
            return null;
        }
        #endregion

        #region Edoc Organization

        private void SyncEdocOrganization()
        {
            var organizations = _unitOfWork.Context.Set<Organization>()
                                    .Include(x => x.Oked).ToList();

            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                foreach (var item in organizations)
                {
                    CreateOrganizationForEdocSchema(item);
                }
                if (HasErrors)
                    return;

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

            try
            {
                _userService.SyncEdocUser();
                CombineStatuses(_userService);
                if (HasErrors)
                    return;

                _departmentService.SyncEdocDepartment();
                CombineStatuses(_departmentService);
                if (HasErrors)
                    return;

                _positionService.SyncEdocPosition();
                CombineStatuses(_positionService);
                if (HasErrors)
                    return;
            }
            catch (Exception ex)
            {
                AddError(ex.InnerException.Message);
            }
        }

        private void CreateOrganizationForEdocSchema(Organization dto)
        {
            try
            {
                var exsist = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Organization>()
                                        .Any(x => x.Id == dto.Id);
                var oked = _unitOfWork.Context.Set<Oked>()
                    .FirstOrDefault(a => a.Id == dto.OkedId);

                if (!exsist)
                {
                    var organization = new Ssp.DataLayer.EFClasses.Edoc.Organization()
                    {
                        Id = dto.Id,
                        OrderCode = dto.OrderCode,
                        RegionId = dto.RegionId,
                        Accounter = !string.IsNullOrEmpty(dto.Accounter) ? dto.Accounter : "ACCOUNTER",
                        Address = dto.Address,
                        AppId = null,
                        Cashier = !string.IsNullOrEmpty(dto.Accounter) ? dto.Accounter : "ACCOUNTER",
                        Director = dto.Director,
                        ContactInfo = "1234567",
                        DateOfCreated = DateTime.Now,
                        DistrictId = dto.DistrictId.Value,
                        FullName = dto.FullName,
                        ShortName = dto.ShortName,
                        Inn = dto.Inn,
                        LogoImage = Encoding.UTF8.GetBytes("empty"),
                        Oked = oked != null ? oked.Code : "",
                        UniqueId = Guid.NewGuid().ToString(),
                        VatCode = !string.IsNullOrEmpty(dto.VatCode) ? dto.VatCode : "000",
                        ZipCode = !string.IsNullOrEmpty(dto.VatCode) ? dto.VatCode : "000",
                        Website = "www.edoc.uz",
                        StateId = dto.StateId,
                        OrganizationGroupId = dto.OrganizationGroupId,
                        IncomingDocReceiverEmployeeId = dto.IncomingDocReceiverEmployeeId
                    };
                    _unitOfWork.Context.Add(organization);
                    _unitOfWork.Save();
                }
                else
                {
                    UpdateOrganizationForEdocSchema(dto);
                }

            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }
        }

        private void UpdateOrganizationForEdocSchema(Organization dto)
        {
            try
            {
                var organization = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Organization>()
                                        .FirstOrDefault(x => x.Id == dto.Id);
                var oked = _unitOfWork.Context.Set<Oked>().AsNoTracking()
                    .FirstOrDefault(a => a.Id == dto.OkedId);
                if (organization is not null)
                {
                    organization.OrderCode = !string.IsNullOrEmpty(dto.OrderCode) ? dto.OrderCode : dto.Id.ToString("0000");
                    organization.RegionId = dto.RegionId;
                    organization.Accounter = !string.IsNullOrEmpty(dto.Accounter) ? dto.Accounter : "ACCOUNTER";
                    organization.Address = dto.Address;
                    organization.Cashier = !string.IsNullOrEmpty(dto.Accounter) ? dto.Accounter : "ACCOUNTER";
                    organization.Director = dto.Director;
                    organization.DistrictId = dto.DistrictId.HasValue ? dto.DistrictId.Value : 1;
                    organization.FullName = dto.FullName;
                    organization.ShortName = dto.ShortName;
                    organization.Inn = dto.Inn;
                    organization.Oked = oked != null ? oked.Code : "";
                    organization.VatCode = !string.IsNullOrEmpty(dto.VatCode) ? dto.VatCode : "000";
                    organization.ZipCode = !string.IsNullOrEmpty(dto.VatCode) ? dto.VatCode : "000";
                    organization.StateId = dto.StateId;
                    organization.OrganizationGroupId = dto.OrganizationGroupId;
                    organization.IncomingDocReceiverEmployeeId = dto.IncomingDocReceiverEmployeeId;
                    _unitOfWork.Context.Update(organization);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }
        }
        #endregion

        #region Organization For My page
        public List<OrganizationMyPageDto> GetListForMyPage()
        {
            var dto = _unitOfWork.Context.Set<Organization>()
                .Include(a => a.Files)
                .Include(a => a.Region)
                .Include(a => a.District)
                .Where(a => a.OrganizationGroupId == 3)
                .Select(a => new OrganizationMyPageDto()
                {
                    Address = a.Address,
                    Region = a.Region.FullName,
                    RegionId = a.RegionId,
                    Email = a.Email,
                    District = a.District.FullName,
                    PhoneNumber = a.PhoneNumber,
                    ZipCode = a.ZipCode,
                    Files = a.Files.Select(b => new OrganizationFileDto()
                    {
                        Id = b.Id,
                    }).ToList(),
                }).ToList();

            //if (dto == null)
            //    AddError("По вашему запросу запись не найдено");

            //var org = new List<OrganizationMyPageDto>()
            //{
            //    Address = dto.Address,
            //    Region = dto.Region.FullName,
            //    District = dto.District.FullName,
            //    PhoneNumber = dto.PhoneNumber,
            //    ZipCode = dto.ZipCode,
            //    Id = dto.Files.FirstOrDefault(a => a.OwnerId == dto.Id)?.Id,
            //};
            return dto;
        }
        #endregion
    }
}
