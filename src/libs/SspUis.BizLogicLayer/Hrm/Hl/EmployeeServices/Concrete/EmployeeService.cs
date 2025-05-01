using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using StatusGeneric;
using WbAccessControl.Sdk;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.Manuals.Services;
using WEBASE.Integration.MSPD.Client;
using WEBASE.Integration.MSPD.GSP;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer;

public class EmployeeService : StatusGenericHandler, IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IPersonService _personService;
    private readonly IStorageService _storageService;
    private readonly ICultureHelper _cultureHelper;
    private readonly IMspdUnitOfWork _mspdUnitOfWork;
    private readonly IConvertService _pdfConverter;
    public  IReportService _reportService;
    private readonly IUOWIntegrationManuals _uowIntegrationManuals;
    private readonly IWbacClientService _wbacClientService;
    private readonly IPersonLogService _personLogService;

    //private readonly IMapper _mapper;

    public EmployeeService(IUnitOfWork unitOfWork,
        IAuthService authService,
        IPersonService personService,
        IStorageService storageService,
        ICultureHelper cultureHelper,
        IMspdUnitOfWork mspdUnitOfWork,
        IUOWIntegrationManuals uowIntegrationManuals,
        IConvertService pdfConverter,
        IReportService reportService,
        IPersonLogService personLogService,
        IWbacClientService wbacClientService
/*IMapper mapper*/)
    {
        _repository = unitOfWork.EmployeeRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _personService = personService;
        _storageService = storageService;
        _mspdUnitOfWork = mspdUnitOfWork;
        _cultureHelper = cultureHelper;
        _uowIntegrationManuals = uowIntegrationManuals;
        this._pdfConverter = pdfConverter;
        this._reportService = reportService;
        _wbacClientService = wbacClientService;
        _personLogService = personLogService;
        // _mapper = mapper;
    }

    public PagedResult<EmployeeListDto> GetList(EmployeeSortFilterPageOptions dto)
    => GetListMethod(dto)
            .SortFilter(dto)
            .AsPagedResult(dto);

    public IQueryable<EmployeeListDto> GetListMethod(EmployeeSortFilterPageOptions dto)
    {
        var data = GetQuery<EmployeeListDto>();
        return data;
    }
    public EmployeeDto Get()
    {
        //SyncEdocEmployee();
        //if (HasErrors)
        //    return null;
        return new EmployeeDto
        {
            Organization = _authService.Organization?.FullName,
        };
    }

    public EmployeeDto Get(int id)
    {
        var dto = GetQuery<EmployeeDto>().FirstOrDefault(a => a.Id == id);
        if (dto == null)
            AddError("По вашему запросу запись не найдено");
        if (dto.User == null)
            dto.User = new UpdateEployeeUserDto();
        return dto;
    }

    public PagedSelectList<int> AsSelectList(EmployeeSortFilterPageOptions options)
    {
        return _repository.ReadAsNoTracked<EmployeeListDto>()
                          .FilterByIds(options)
                          .FilterByCustomFields(options)
                          .SortFilter(options)
                          .AsPagedResult(options)
                          .AsSelectList();
    }

    public async Task<EmployeeDto> GetByPassportData(GSPPersonInfoRequestDto dto)
    {
        var person = await _personService.GetByPassportData(dto);
        var employee = Get();
        employee.Person = person;
        return employee;
    }

    public HaveId<int> Create(CreateEmployeeDto dto)
    {
        var transaction = _unitOfWork.BeginTransaction();
        try
        {
            Validate(null, dto);

            Person person = _unitOfWork.PersonRepository.ByPinfl(dto.Person.Pinfl);
            if (person == null)
            {
                person = _unitOfWork.PersonRepository.Create(dto.Person);
                CombineStatuses(_unitOfWork.PersonRepository);

                if (HasErrors)
                    return null;
                _unitOfWork.Save();

                if (person.PictureId != null)
                {
                    _storageService.MoveToPersistent(DocumentStorageConst.HL_PERSON_FILES, person.Id.ToString(), (Guid)person.PictureId);
                    CombineStatuses(_storageService);
                }
            }

            dto.PersonId = person.Id;
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                //edoc Employee Sync
                CreateEmployeeForEdocSchema(entity);
                if (HasErrors)
                    return null;

                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
        }
        catch (Exception e)
        {
            AddError($"{e.Message} + {e.InnerException} + {e.StackTrace} + {e.Source}" , "Message");
            _unitOfWork.Rollback();
        }
        finally
        {
            transaction.Dispose();
        }
        return null;
    }

    public HaveId<int> Create(CreateEmployeeByUserDlDto dto)
    {
        var transaction = _unitOfWork.BeginTransaction();
        try
        {
            Validate(null, dto);
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();

                //edoc Employee Sync
                CreateEmployeeForEdocSchema(entity);
                if (HasErrors)
                    return null;

                transaction.Commit();
                return HaveId.Create(entity.Id);
            }

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
        return null;
    }

    public async Task Update(UpdateEmployeeDlDto dto, bool isJob = false)
    {
        var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var person = _unitOfWork.Context.Set<Employee>().Include(x => x.Person).FirstOrDefault(x => x.Id == dto.Id);
            if (person != null)
            {
                if ((person.Person.PassportDate != dto.Person.PassportDate ||
                    person.Person.PassportNumber != dto.Person.PassportNumber ||
                    person.Person.PassportSeria != dto.Person.PassportSeria ||
                    person.Person.PassportExpiration != dto.Person.PassportExpiration ||
                    person.Person.PassportDivName != dto.Person.PassportDivName)
                                && person.Person.Pinfl == dto.Person.Pinfl
                                    && person.Person.BirthDate == dto.Person.BirthDate)

                    _personLogService.Create(new CreatePersonLogDlDto()
                    {
                        EmployeeId = person.Id,
                        PersonId = person.Person.Id,
                        PassportDate = person.Person.PassportDate,
                        PassportDivName = person.Person.PassportDivName,
                        PassportExpiration = person.Person.PassportExpiration,
                        PassportNumber = person.Person.PassportNumber,
                        PassportSeria = person.Person.PassportSeria,
                        Pinfl = person.Person.Pinfl,
                    });
            }
            if (HasErrors)
                return;
            Validate(null, dto, true);
            if (HasErrors)
                return;
            if (dto.Person.PictureId != null && isJob == false)
            {
                _personService.AddOrUpdateFiles(dto.Person.Id, (Guid)dto.Person.PictureId);
                CombineStatuses(_personService);
            }
            if (HasErrors)
                return;
            if (isJob == false)
                _unitOfWork.PersonRepository.Update(dto.Person);

            var employee = _repository.Update(dto);

            if (HasErrors)
                return;

            CombineStatuses(_repository);


            if (HasErrors)
                return;
            if (IsValid)

                _unitOfWork.Save();
            if (HasErrors)
                return;
            _repository.UpdateRelatives(dto.Relatives, employee.Id);

            _unitOfWork.Save();
            if (HasErrors)
                return;
            //edoc Employee Sync
            UpdateEmployeeForEdocSchema(employee);
            if (HasErrors)
                return;

            transaction.Commit();

            if (isJob == false && employee.Person.PictureId != null && IsValid)
            {
                var employeeManage = await _unitOfWork.Context.Set<EmployeeManage>()
                    .Include(a => a.Organization)
                    .Include(a => a.Employee)
                    .ThenInclude(a => a.Person)
                    .FirstOrDefaultAsync(a => (a.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP 
                                              || a.Organization.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP) 
                                              && a.EndOn == null && a.IsDeleted == false 
                                              && a.Employee.Person.Id == employee.PersonId);

                if (employeeManage != null)
                {
                    var turnstilePersonDataResult = await _wbacClientService.UpSertPersonAsync(new WbacTurnstilePersonDto
                    {
                        PersonId = employeeManage.Employee.Id,
                        TableId = TableIdConst.HL_EMPLOYEE,
                        OrganizationId = employeeManage.Employee.OrganizationId,
                        FullName = employeeManage.Employee.Person.FullName,
                        ImageId = employeeManage.Employee.Person.PictureId != null ? employeeManage.Employee.Person.PictureId.ToString() : null
                    });

                    if (!turnstilePersonDataResult.IsSuccess || !turnstilePersonDataResult.Response)
                        CombineStatuses(turnstilePersonDataResult.GetStatusGeneric());
                }
            }
        }
        catch (Exception e)
        {
            AddError(e.Message);
            transaction.Rollback();
        }
        finally
        {
            transaction.Dispose();
        }
    }

    public async Task<byte[]> DownloadCV(string pinfl, string? langue)
    {
        var language = langue ?? "uz-latn";

        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(language, StaticFileConst.WordTemplate.EMPLOYEE_CV));

        var languageId = _unitOfWork.Context.Set<Language>()
            .FirstOrDefault(l => l.Code == language)?.Id ?? 1;

        var person = _unitOfWork.Context.Set<Person>().Select(a => new
        {
            a.Id,
            a.Pinfl,
            a.BirthDate,
            BirthCountry = a.BirthCountry.Translates
                .Where(t => t.LanguageId == languageId)
                .Select(t => t.TranslateText)
                .FirstOrDefault(),
            BirthDistrict = a.BirthDistrict.FullName,
            Nationality = a.Nationality.FullName,
            Name = a.FullName,
            a.ShortName,
            a.PictureId
        }).FirstOrDefault(a => a.Pinfl == pinfl);

        var employee = _unitOfWork.Context.Set<Employee>()
                .Include(a => a.Person)
                .Include(a => a.Relatives)
                    .ThenInclude(a => a.Region)
                .Include(a => a.Relatives)
                    .ThenInclude(a => a.District)
                .Include(a => a.Relatives)
                    .ThenInclude(a => a.Citizenship)
                .Include(a => a.Relatives)
                    .ThenInclude(a => a.Country)
                .Include(a => a.Relatives)
                    .ThenInclude(a => a.RelativeDegree)
                    .ThenInclude(a => a.Translates)
                .Include(a => a.HigherEdu)
                    .ThenInclude(a => a.Institute)
                .Include(a => a.HigherEdu)
                    .ThenInclude(a => a.Specialty)
                .Include(a => a.HigherEdu)
                    .ThenInclude(a => a.EmployeeHigherEduDegrees)
                    .ThenInclude(degree => degree.Translates)
                .Include(a => a.Relatives)
                    .ThenInclude(a => a.Nationality)
                .Include(a => a.PlaceOfWorks)
                    .ThenInclude(a => a.EmploymentType)
                .Include(a => a.AcademicDegrees)
                    .ThenInclude(a => a.AcademicDegree)
                .Include(a => a.DegreeTitles)
                    .ThenInclude(a => a.DegreeTitle)
                .Include(a => a.ElectionMembers)
                    .ThenInclude(a => a.ElectionMember)
                .Include(a => a.LanguageProficiencys)
                    .ThenInclude(a => a.Languagperoficiency)
                .Include(a => a.Partisanships)
                    .ThenInclude(a => a.Partisanship)
                .Include(a => a.ScientificDegrees)
                    .ThenInclude(a => a.ScientificDegree)
                .Include(a => a.StateAwards)
                    .ThenInclude(a => a.StateAwards)
                .Include(a => a.MilitaryRanks)
                    .ThenInclude(a => a.MilitaryRanks)
                .Select(e => new
                {
                    e.Id,
                    e.PersonId,
                    e.StateId,
                    StateAwards = e.StateAwards.Select(a => new EmployeeStateAwardDto
                    {
                        StateAwards = a.StateAwards.FullName
                    }).ToList(),
                    ScientificDegrees = e.ScientificDegrees.Select(a => new EmployeeScientificDegreeDto
                    {
                        ScientificDegree = a.ScientificDegree.FullName,
                    }).ToList(),
                    Partisanships = e.Partisanships.Select(a => new EmployeePartisanshipDto
                    {
                        Partisanship = a.Partisanship.FullName,
                        Year = a.Year
                    }).ToList(),
                    LanguageProficiencys = e.LanguageProficiencys.Select(a => new EmployeeLanguageProficiencyDto
                    {
                        LanguageDegree = a.LanguageDegree,
                        Languagperoficiency = a.Languagperoficiency.FullName,
                        LanguageDegrees = a.LanguageDegrees.FullName
                    }).ToList(),
                    ElectionMembers = e.ElectionMembers.Select(a => new EmployeeElectionMemberDto
                    {
                        ElectionMember = a.ElectionMember.FullName,
                        Year = a.Year
                    }).ToList(),

                    AcademicDegrees = e.AcademicDegrees.Select(a => new EmployeeAcademicDegreeDto
                    {
                        AcademicDegree = a.AcademicDegree.FullName
                    }).ToList(),
                    HigherEdu = e.HigherEdu.Select(a => new EmployeeHigherEduDto
                    {
                        Institute = a.Institute.FullName,
                        InstituteName = a.InstituteName,
                        DateOfIssue = a.DateOfIssue,
                        EmployeeHigherEduDegree = a.EmployeeHigherEduDegrees.Translates.AsQueryable().FirstOrDefault(EmployeeHigherEduDegreeTranslate.GetExpr(
                            TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.EmployeeHigherEduDegrees.FullName
                    }).ToList(),
                    Speciality = e.HigherEdu.Select(a => new
                    {
                        Specialty = a.Specialty.FullName,
                    }).ToList(),
                    Relatives = e.Relatives.Select(a => new EmployeeRelativeDto
                    {
                        RelativeDegree = a.RelativeDegree.Translates.AsQueryable().FirstOrDefault(RelativeDegreeTranslate.GetExpr(
                            TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
                        FullName = a.FullName,
                        RelativeWorkPlace = a.RelativeWorkPlace,
                        RelativeWorkPlacePosition = a.RelativeWorkPlacePosition,
                        Address = a.Address,
                        DateOfBirth = a.DateOfBirth
                    }).ToList(),
                    PlaceOfWorks = e.PlaceOfWorks.Select(a => new EmployeePlaceOfWorkDto
                    {
                        PositionName = a.PositionName,
                        ContractorId = a.ContractorId,
                        Contractor = a.ContractorName,
                        StartOn = a.StartOn,
                        EndOn = a.EndOn
                    }).ToList(),
                    DegreeTitles = e.DegreeTitles.Select(a => new EmployeeDegreeTitleDto
                    {
                        DegreeTitle = a.DegreeTitle.FullName,
                        Year = a.Year
                    }).ToList()
                })
                .FirstOrDefault(a => a.PersonId == person.Id && a.StateId == StateIdConst.ACTIVE);

        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
            .Include(a => a.Employee)
            .Include(a => a.Position)
            .Include(a => a.Organization)
            .Include(a => a.Department)
            .Select(e => new
            {
                e.Employee,
                e.StartOn,
                e.EmpAppointOrderTypeId,
                Position = e.Position.FullName,
                Department = e.Department.FullName,
                Organization = e.Organization.FullName
            })
            .FirstOrDefault(a => a.Employee.Id == employee.Id);

        var plh = new Placeholders();

        plh.TextPlaceholders.Add(nameof(person.Name), person.Name.CapitalizeEachWord(" "));
        plh.TextPlaceholders.Add(nameof(person.Pinfl), person.Pinfl);
        plh.TextPlaceholders.Add(nameof(person.BirthDate), person.BirthDate.ToString("dd.MM.yyyy"));
        plh.TextPlaceholders.Add(nameof(person.Nationality), person.Nationality);
        plh.TextPlaceholders.Add(nameof(person.BirthDistrict), person.BirthCountry ?? "");
        plh.TextPlaceholders.Add(nameof(employee.Partisanships), employee.Partisanships.Count > 0 ?
            string.Join(",", employee.Partisanships.Select(p => p.Partisanship).ToList()) : getNoMessage(languageId));

        var employeeHigherEduDegree = employee.HigherEdu.FirstOrDefault() ?? new();
        plh.TextPlaceholders.Add(nameof(employeeHigherEduDegree.EmployeeHigherEduDegree), employee.HigherEdu.Count > 0 ?
            string.Join(",", employee.HigherEdu.Select(t => t.EmployeeHigherEduDegree).ToList()) : getNoMessage(languageId));

        plh.TextPlaceholders.Add(nameof(employee.LanguageProficiencys), employee.LanguageProficiencys.Count > 0 ?
            string.Join(", ", employee.LanguageProficiencys.Select(p => p.LanguageDegrees).ToList()) + " tillari" : getNoMessage(languageId));
        plh.TextPlaceholders.Add(nameof(employee.AcademicDegrees), employee.AcademicDegrees.Count > 0 ?
            string.Join(",", employee.AcademicDegrees.Select(p => p.AcademicDegree).ToList()) : getNoMessage(languageId));
        plh.TextPlaceholders.Add(nameof(employee.ScientificDegrees), employee.ScientificDegrees.Count > 0 ?
            string.Join(",", employee.ScientificDegrees.Select(p => p.ScientificDegree).ToList()) : getNoMessage(languageId));
        plh.TextPlaceholders.Add(nameof(employee.StateAwards), employee.StateAwards.Count > 0 ?
            string.Join(",", employee.StateAwards.Select(p => p.StateAwards).ToList()) : getNoMessage(languageId));
        plh.TextPlaceholders.Add(nameof(employee.ElectionMembers), employee.ElectionMembers.Count > 0 ?
            string.Join(",", employee.ElectionMembers.Select(p => p.ElectionMember).ToList()) : getNoMessage(languageId));
        plh.TextPlaceholders.Add(nameof(employee.HigherEdu), employee.HigherEdu.Count > 0 ?
            string.Join(", ", employee.HigherEdu.Select(p => $"{p.DateOfIssue.Year}-yil {p.Institute}").ToList()) : "");
        plh.TextPlaceholders.Add(nameof(employee.Speciality), employee.Speciality.Count > 0 ?
            string.Join(",", employee.Speciality.Select(p => p.Specialty).ToList()) : getNoMessage(languageId));
        if (employeeManage != null)
        {
            if (employeeManage.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER || employeeManage.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE)
            {
                plh.TextPlaceholders.Add(nameof(employeeManage.StartOn), employeeManage.StartOn.ToString("dd.MM.yyyy"));
            }
            plh.TextPlaceholders.Add(nameof(employeeManage.Department), employeeManage.Department);
            plh.TextPlaceholders.Add(nameof(employeeManage.Position), employeeManage.Position);
            plh.TextPlaceholders.Add(nameof(employeeManage.Organization), employeeManage.Organization);
        }
        else
        {
            plh.TextPlaceholders.Add(nameof(employeeManage.StartOn), "");
            plh.TextPlaceholders.Add(nameof(employeeManage.Department), "");
            plh.TextPlaceholders.Add(nameof(employeeManage.Position), "");
            plh.TextPlaceholders.Add(nameof(employeeManage.Organization), "");
        }
        if (person.PictureId != null)
        {
            var image = _storageService.GetFile(
                DocumentStorageConst.HL_PERSON_FILES
                , person.Id.ToString(),
                person.PictureId.Value).GetStream();

            plh.ImagePlaceholders.Add("Avatar", new()
            {
                Dpi = 512,
                MemStream = (System.IO.MemoryStream)image,
                Width = 400,
                Height = 600
            });
        }
        plh.TextPlaceholders.Add(nameof(employee.PlaceOfWorks), employee.PlaceOfWorks.Count > 0 ?
            string.Join("<br/>", employee.PlaceOfWorks.OrderBy(a => a.StartOn).Select(p => $"{p.StartOn.ToString("dd.MM.yyyy")}-{p.EndOn?.ToString("dd.MM.yyyy")}" + " " +
            $"{p.Contractor}({p.PositionName})").ToList()) : getNoMessage(languageId));

        var item = employee.Relatives.FirstOrDefault() ?? new();
        plh.TablePlaceholders.Add(new Dictionary<string, List<string>>
        {
            { nameof(item.RelativeDegree), employee.Relatives.Select(t => t.RelativeDegree).ToList() },
            { nameof(item.FullName), employee.Relatives.Select(t => t.FullName).ToList() },
            { nameof(item.DateOfBirth), employee.Relatives.Select(t => t.DateOfBirth.ToString("dd.MM.yyyy")).ToList() },
            { nameof(item.RelativeWorkPlace), employee.Relatives.Select(t => $"{t.RelativeWorkPlace} - {t.RelativeWorkPlacePosition}").ToList() },
            { nameof(item.Address), employee.Relatives.Select(t => t.Address).ToList() }
        });
        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();


        var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
        CombineStatuses(_pdfConverter);
        return res;
    }

    public async Task<EmployeeRelativeDto> GetRelativesByPassportData(GSPPersonInfoRequestDto dto)
    {
        var person = _repository.CrudServices.ProjectFromEntityToDto<EmployeeRelative, EmployeeRelativeDto>(
                    query => query.Where(a => a.Owner.Person.PassportSeria == dto.Seria
                    && a.Owner.Person.PassportNumber == dto.Number
                    && a.Owner.Person.BirthDate == DateOnly.FromDateTime(dto.DateOfBirth))
            ).FirstOrDefault();

        if (person == null)
        {
            var status = await GetFromGsp(dto);
            CombineStatuses(status);
            person = status.Result;
            return new EmployeeRelativeDto()
            {
                OnDate = DateOnly.FromDateTime(DateTime.Now),
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                Pinfl = person.Pinfl,
                CountryId = person.CountryId,
                Country = person.Country,
                RegionId = person.RegionId,
                Region = person.Region,
                DistrictId = person.DistrictId,
                District = person.District,
                DocumentSeries = person.DocumentSeries,
                DocumentNumber = person.DocumentNumber,
                DateOfIssue = person.DateOfIssue.Value,
                DateOfExpire = person.DateOfExpire.Value,
                NationalityId = person.NationalityId,
                Nationality = person.Nationality,
                CitizenshipId = person.CitizenshipId,
                Citizenship = person.Citizenship,
            };
        }
        return person;
    }

    private async Task<IStatusGeneric<EmployeeRelativeDto>> GetFromGsp(GSPPersonInfoRequestDto dto)
    {
        var res = new StatusGenericHandler<EmployeeRelativeDto>();
        var status = await _mspdUnitOfWork.GSP.GetPersonInfoAsync(dto);
        if (status.IsValid)
        {
            var wbCodes = status.Result.LoadCodes(_uowIntegrationManuals);
            var birthCountry = _unitOfWork.CountryRepository.ByWbCode(wbCodes.BirthCountryWbCode);
            var nationality = _unitOfWork.NationalityRepository.ByWbCode(wbCodes.NationalityWbCode);
            var citizenship = _unitOfWork.CitizenshipRepository.ByWbCode(wbCodes.CitizenshipWbCode);
            var livingRegion = _unitOfWork.RegionRepository.ByWbCode(wbCodes.LivingRegionWbCode);
            var livingDistrict = _unitOfWork.DistrictRepository.ByWbCode(wbCodes.LivingDistrictWbCode);
            var gender = _unitOfWork.Context.Set<Gender>()
                                            .AsQueryable()
                                            .Include(a => a.Translates)
                                            .FirstOrDefault(a => a.Id == status.Result.person_sex);
            var birthDate = status.Result.person_birth_date.AsDateOnly();
            res.SetResult(new EmployeeRelativeDto
            {
                Pinfl = status.Result.person_pin,
                DocumentSeries = status.Result.person_passport_seria,
                DocumentNumber = status.Result.person_passport_number,
                DateOfIssue = DateOnly.FromDateTime(status.Result.person_passport_date),
                DateOfExpire = DateOnly.FromDateTime(status.Result.person_passport_expiration),
                FamilyName = status.Result.person_surname_latin,
                FirstName = status.Result.person_name_latin,
                LastName = status.Result.person_patronym_latin,
                //SurnameEng = status.Result.person_surname_eng,
                //NameEng = status.Result.person_name_eng,
                DateOfBirth = birthDate,
                //PassportDivName = status.Result.person_passport_div_name,
                CountryId = birthCountry?.Id,
                Country = birthCountry?.Translates.AsQueryable().FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? birthCountry?.FullName,
                //BirthRegionId = _unitOfWork.RegionRepository.ByWbCode(birthRegionWbCode)?.Id,
                //BirthDistrictId = _un,
                //GenderId = gender?.Id,
                //Gender = gender?.Translates.AsQueryable().FirstOrDefault(GenderTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? gender?.FullName,
                NationalityId = nationality?.Id,
                Nationality = nationality?.Translates.AsQueryable().FirstOrDefault(NationalityTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? nationality?.FullName,
                CitizenshipId = citizenship?.Id,
                Citizenship = citizenship?.Translates.AsQueryable().FirstOrDefault(CitizenshipTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? citizenship?.FullName,
                RegionId = livingRegion?.Id,
                Region = livingRegion?.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? livingRegion?.FullName,
                DistrictId = livingDistrict?.Id,
                District = livingDistrict?.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? livingDistrict?.FullName,
            });
        }
        res.CombineStatuses(status);
        return res;
    }

    public void Delete(int id)
    {
        try
        {
            _repository.Delete(id);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
        catch
        {
            AddError("Пользователь не может быть удален");
        }
    }

    private IQueryable<TDto> GetQuery<TDto>()
        where TDto : class
    => _repository.ReadAsNoTracked<TDto>(q => q.StateId == StateIdConst.ACTIVE &&
                                                      (_authService.HasPermission(ModuleCode.AllEmployeeView) ||
                                                       q.OrganizationId == _authService.User.OrganizationId));


    private void Validate<TDto>(Employee entity, EmployeeDlDto<TDto> dto, bool isUpdated = false)
           where TDto : EmployeeDlDto<TDto>
    {
        var query = _repository.DbSet.Include(a => a.Person).AsQueryable();

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

        var emp = query.IsActive().FirstOrDefault(a => a.PersonId == dto.PersonId);

        if (emp != null && isUpdated == false)
            AddError($"Это сотрудник уже существует.", $"{emp.Person.PassportSeria}{emp.Person.PassportNumber}");

        if (isUpdated == false)
        {
            var hasNumberPrevios = _repository.AllAsQueryable.Where(a => a.PersonId != dto.PersonId &&
                    a.PhoneNumber == dto.PhoneNumber);
            if (hasNumberPrevios.Any())
                AddError("С этим номером телефона создан другой сотрудник");
        }

    }

    #region Edoc Organization

    private void SyncEdocEmployee()
    {
        var employees = _unitOfWork.Context.Set<Employee>()//.Where(x => x.Id > 700)
                                .Include(x => x.Person).ToList();

        var transaction = _unitOfWork.BeginTransaction();
        try
        {
            foreach (var item in employees)
            {
                CreateEmployeeForEdocSchema(item);
                if (HasErrors)
                    return;

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
    }

    private void CreateEmployeeForEdocSchema(Employee dto)
    {
        try
        {
            var exsist = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Employee>()
                                    .Include(x => x.Passports)
                                    .Any(x => x.Id == dto.Id);
            if (!exsist)
            {
                var employee = new Ssp.DataLayer.EFClasses.Edoc.Employee()
                {
                    Id = dto.Id,
                    Pinfl = dto.Person.Pinfl,
                    Inn = dto.Person.Inn,
                    FirstName = dto.Person.NameLatin,
                    LastName = dto.Person.PatronymLatin,
                    MiddleName = dto.Person.SurnameLatin,
                    ShortName = dto.Person.ShortName,
                    FullName = dto.Person.FullName,
                    Category = null,
                    DateOfBirth = dto.Person.BirthDate.ToString("yyyy-MM-dd").ToDate(),
                    OrganizationId = dto.OrganizationId,
                    UserId = null,
                    CreatedUserId = dto.CreatedUserId.HasValue ? dto.CreatedUserId.Value : 1,
                    DateOfCreated = DateTime.UtcNow,
                    DateOfModified = null,
                    StateId = dto.StateId
                };

                employee.Passports.Add(new Ssp.DataLayer.EFClasses.Edoc.EmployeePassport()
                {
                    Series = dto.Person.PassportSeria,
                    Number = dto.Person.PassportNumber,
                    StateId = dto.Person.StateId,
                    DateOfIssue = dto.Person.PassportDate,
                    Validity = dto.Person.PassportExpiration,
                    CreatedUserId = dto.CreatedUserId.HasValue ? dto.CreatedUserId.Value : 1,
                    DateOfCreated = DateTime.UtcNow,
                    IssuedBy = dto.Person.PassportDivName,
                });
                employee.EntityJsonData = JsonSerializer.Serialize<Ssp.DataLayer.EFClasses.Edoc.Employee>(employee);

                _unitOfWork.Context.Add(employee);
                _unitOfWork.Save();
            }
            else
            {
                UpdateEmployeeForEdocSchema(dto);
            }

        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
    }

    private void UpdateEmployeeForEdocSchema(Employee dto)
    {
        try
        {

            var employee = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Employee>()
                                    .Include(x => x.Passports)
                                    .FirstOrDefault(x => x.Id == dto.Id);
            if (employee is not null)
            {
                employee.FirstName = dto.Person.NameLatin;
                employee.LastName = dto.Person.PatronymLatin;
                employee.MiddleName = dto.Person.SurnameLatin;
                employee.FullName = dto.Person.FullName;
                employee.ShortName = dto.Person.ShortName;
                employee.DateOfBirth = dto.Person.BirthDate.ToString("yyyy-MM-dd").ToDate();
                employee.StateId = dto.StateId;
                employee.ModifiedUserId = _authService.User.Id;
                employee.DateOfModified = DateTime.Now;

                employee.Passports.First().Series = dto.Person.PassportSeria;
                employee.Passports.First().Number = dto.Person.PassportNumber;
                employee.Passports.First().StateId = dto.Person.StateId;
                employee.Passports.First().DateOfIssue = dto.Person.PassportDate;
                employee.Passports.First().Validity = dto.Person.PassportExpiration;
                employee.Passports.First().IssuedBy = dto.Person.PassportDivName;
                employee.Passports.First().ModifiedUserId = _authService.User.Id;
                employee.Passports.First().DateOfModified = DateTime.Now;


                _unitOfWork.Context.Update(employee);
                _unitOfWork.Save();
            }
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
    }
    #endregion

    private string getNoMessage(int langId)
    {
        return langId switch
        {
            1 => "Нет",
            2 => "Йўқ",
            3 => "Yo'q",
            4 => "No",
        };
    }

}
