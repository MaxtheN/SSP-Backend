using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using Spire.Doc;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Extensions;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.UserServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.Integration.Soliq;
using SspUis.Integration.Soliq.Models;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;
using Document = Spire.Doc.Document;

namespace SspUis.BizLogicLayer.MemshipApplicationServices;

public class MemshipApplicationService
    : BaseApplicationService
        <MemshipApplication,
        MemshipApplicationListDto,
        MemshipApplicationDto,
        CreateMemshipApplicationDlDto,
        UpdateMemshipApplicationDlDto,
        IMemshipApplicationRepository,
        MemshipApplicationSortFilterOptions>, IMemshipApplicationService
{
    private readonly Lazy<IMemshipContractService> _memshipContractService;
    private readonly IServiceProvider _serviceProvider;

    #region ctor
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IEImzoService _eImzoService;
    private readonly INumberService _numberService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly SystemConf _systemConf;
    private readonly IContractorService _contractorService;
    private readonly IConvertService _pdfConverter;
    private readonly IStorageService _storageService;
    private readonly ICultureHelper _cultureHelper;
    private readonly ISoliqContractorService _soliq;

    public MemshipApplicationService(IUnitOfWork unitOfWork,
        IAuthService authService,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IEImzoService eImzoService,
        IContractorService contractorService,
        SystemConf systemConf,
        IConvertService pdfConverter,
        IStorageService storageService,
        ICultureHelper cultureHelper,
        IServiceProvider serviceProvider,
        ISoliqContractorService soliq)
        : base(unitOfWork, documentChangeLogService)
    {
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _numberService = numberService;
        _documentChangeLogService = documentChangeLogService;
        _contractorService = contractorService;
        _storageService = storageService;
        _eImzoService = eImzoService;
        _systemConf = systemConf;
        _pdfConverter = pdfConverter;
        _storageService = storageService;
        _cultureHelper = cultureHelper;
        this._soliq = soliq;
    }
    #endregion

    protected override IQueryable<MemshipApplicationListDto> SortFilter(IQueryable<MemshipApplicationListDto> query, MemshipApplicationSortFilterOptions options)
    {
        return base.SortFilter(query, options)
                   .SortFilter(options)
                   .Where(a => a.Application.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP);
    }

    public override PagedResult<MemshipApplicationListDto> GetList(MemshipApplicationSortFilterOptions options)
    {
        return base.GetList(options);
    }

	public int GetCount()
	{
        MemshipApplicationSortFilterOptions memshipApplicationSortFilterOptions =
            new MemshipApplicationSortFilterOptions();
        return base.GetList(memshipApplicationSortFilterOptions).Rows.Count();
	}
	public async ValueTask<MemshipApplicationDto> Get()
    {
        var region = _unitOfWork.RegionRepository.ById<RegionListDto>(_authService.Contractor.RegionId);
        var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(_authService.Contractor.DistrictId);
        var contractor = _unitOfWork.ContractorRepository.ById<ContractorListDto>(_authService.Contractor.Id);

        //if ((contractor.Inn != null || contractor.Inn.Length != 0) && (contractor.Pinfl == null || contractor.Pinfl.Length != 0))
        //{

        //}

        try
        {
            decimal NetIncome = 0;

            string inn = contractor.Inn;
            bool isAnnualReportSubmitted = true;
            SoliqAosAylanmaData benefit = null;
            SoliqQqsAylanmaData benefitQqs = null;
            SoliqQqsAylanmaData benefitNowQqs = null;

            var dateNow = DateTime.Today.AsDateOnly();
            if (contractor == null)
                AddError("Contractor null");
            if (contractor.InnOrPinfl.Length < 10 && contractor.Pinfl == null)
            {
                try
                {
                    benefit = await _soliq.GetAosAylanmaData(inn: int.Parse(inn), year: DateTime.Now.Year - 1);
                }
                catch (DataNotFoundInSoliqException ex)
                {
                    AddError(ex.Message);
                    return null;
                }
                catch (ResponseConvertToModelException ex)
                {
                    AddError(ex.Message);
                    return null;
                }

                if (benefit == null)
                {
                    var today = DateTime.Today;
                    if (today.DayOfYear < 46)
                    {
                        benefit = await _soliq.GetAosAylanmaData(inn: int.Parse(inn), year: DateTime.Now.Year - 2);

                        for (int i = 1; i <= 12; i++)
                        {
                            benefitQqs = await _soliq.GetQqsAylanmaData(
                                month: i,
                                inn: int.Parse(inn),
                                year: today.Year - 1
                            );

                            if (benefitQqs == null || benefitQqs.NetIncomeWithoutVat == null)
                            {
                                benefitQqs = await _soliq.GetQqsAylanmaData(
                                month: i,
                                inn: int.Parse(inn),
                                year: today.Year - 2);

                                if (!(contractor.RegistrationDate.Year > today.Year - 2
                                          && contractor.RegistrationDate.Month > 1
                                          && benefit == null))
                                    isAnnualReportSubmitted = false;
                                continue;
                            }
                            NetIncome += benefitQqs.NetIncomeWithoutVat;
                        }
                    }
                    else 
                    { 
                        for (int i = 1; i <= 12; i++)
                        {
                            benefitQqs = await _soliq.GetQqsAylanmaData(month: i,
                            inn: int.Parse(inn), DateTime.Now.Year - 1);
                            if (benefitQqs == null || benefitQqs.NetIncomeWithoutVat == null)
                            {
                                if (!(contractor.RegistrationDate.Year > dateNow.Year - 2
                                            && contractor.RegistrationDate.Month > 1
                                                && benefit == null))
                                    isAnnualReportSubmitted = false;
                                continue;
                            }
                            NetIncome += benefitQqs.NetIncomeWithoutVat;
                        }
                    }
                }
                else
                {
                    NetIncome = benefit.NetIncome;
                }

                if (benefit == null && contractor.RegistrationDate.AddYears(1) < dateNow)
                {
                    if (!isAnnualReportSubmitted)
                    {
                        if (NetIncome > 0)
                            CombineStatuses(_soliq);
                    }
                    else
                    {
                        if (NetIncome == 0 && NetIncome == null)
                            CombineStatuses(_soliq);
                    }
                }
            }
            if (HasErrors)
            {
                return new()
                {
                    Application = new()
                };
            }

            var category = UnitOfWork.Context.Set<ContractorCategoryCriterion>()
                 .Include(c => c.ContractorCategory).ThenInclude(c => c.Translates)
                 .FirstOrDefault(c =>
                     (c.MinAmount == null || c.MinAmount < NetIncome) &&
                     (c.MaxAmount == null || c.MaxAmount > NetIncome)
                 );
            
            decimal benefitNow = 0;
            
            if (contractor.Inn != null)
            {
                for (int i = 1; i < 13; i++)
                {
                    benefitNowQqs = await _soliq.GetQqsAylanmaData(month: i,
                    inn: int.Parse(inn), DateTime.Now.Year);
                    if (benefitNowQqs == null || benefitNowQqs.NetIncomeWithoutVat == 0)
                    {
                        continue;
                    }
                    benefitNow += benefitNowQqs.NetIncomeWithoutVat;
                }
                // benefitNow = _contractorService.GetAylanmaByInn(contractor.Inn);
            }
            if (benefitNow != 0)
            {
                var categoryNow = UnitOfWork.Context.Set<ContractorCategoryCriterion>()
                     .Include(c => c.ContractorCategory).ThenInclude(c => c.Translates)
                     .FirstOrDefault(c =>
                         (c.MinAmount == null || c.MinAmount < benefitNow) &&
                         (c.MaxAmount == null || c.MaxAmount > benefitNow)
                     );
                if (categoryNow != null && IsPayed(categoryNow.ContractorCategoryId, contractor.OpfId ?? 0))
                {
                    category = categoryNow;
                }
            }
            if (category == null)
            {
                AddError($"Category not found benefit.NetIncome: {NetIncome}");
                //return null;
            }

            SoliqContractorEmployeeCountByTinDataDto employeeCount = null;

            if (contractor.Inn != null)
                employeeCount = await _contractorService.GetEmployeeCountFromSoliq(
                    contractor.Inn,
                    DateTime.Now.Year,
                    DateTime.Now.Month - 1);

            //CombineStatuses(_contractorService);

            if (employeeCount == null)
                employeeCount = new() { MonthlyNumberEmployees = 0 };

            return new MemshipApplicationDto
            {
                Application = new()
                {
                    Contractor = _authService.Contractor.FullName,
                    ContractorInn = _authService.Contractor.Inn ?? _authService.Contractor.Pinfl,
                    DocOn = DateTime.Now.AsDateOnly(),
                    ContractorId = _authService.Contractor.Id,
                    ContractorPositionName = "Директор",
                    ContractorAddress = contractor.Address,
                    ContractorDirector = contractor.Director,
                    RegionId = _authService.Contractor.RegionId,
                    Region = region.FullName,
                    DistrictId = _authService.Contractor.DistrictId,
                    District = district.FullName,
                    ApplicationTypeId = ApplicationTypeIdConst.MEMSHIP,
                    DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                },
                ContractorEmail = contractor.Email ?? "",
                RegistrationDate = contractor.RegistrationDate,
                ContractorMobilePhoneNumber = contractor.MobilePhoneNumber ?? "",
                ContractorWorkPhoneNumber = contractor.WorkPhoneNumber ?? "",
                ContractorAdditionalPhoneNumber = contractor.AdditionalPhoneNumber ?? "",
                ContractorFaks = contractor.Faks ?? "",
                ContractorSkype = contractor.Skype ?? "",
                ContractorFacebook = contractor.Facebook ?? "",
                ContractorTelegram = contractor.Telegram ?? "",
                ContractorWebSite = contractor.WebSite ?? "",
                OwnerName = contractor.OwnerName,

                Files = new(),
                CanEdit = true,
                ContractorCategoryId = category.ContractorCategoryId,
                ContractorCategory = category.ContractorCategory.Translates
                    .AsQueryable()
                    .FirstOrDefault(
                    ContractorCategoryTranslate.GetExpr(
                        TranslateColumn.full_name,
                        _cultureHelper.CurrentCulture.Id)
                    )?.TranslateText ?? category.ContractorCategory.FullName,
                YearlyEarnings = NetIncome,
                MemshipContractTypeId = (IsPayed(category.ContractorCategoryId, contractor.OpfId ?? 0)) ? MemshipContractTypeIdConst.PAID : MemshipContractTypeIdConst.FREE,
                OkedId = (contractor.OkedId.HasValue) ? contractor.OkedId : null,
                NowYearlyEarnings = benefitNow,
                EmployeesCount = employeeCount.MonthlyNumberEmployees,
                OrganizationId = (IsPayed(category.ContractorCategoryId, contractor.OpfId ?? 0)) ? OrganizationIdConst.SSP : null,
                CanSelectOrganization = (IsPayed(category.ContractorCategoryId, contractor.OpfId ?? 0)) ? true : false
            };
        }
        catch (Exception ex)
        {
            AddError(ex.Message + "  Inner: " + ex.InnerException.Message);
            return null;
        }
    }
    public async ValueTask<MemshipApplicationDto> GetForErp(string inn)
    {

        #region ContractorCreate memshipcontractni erp tomondan yaratish uchun
        Contractor contractorCreate = _unitOfWork.ContractorRepository.ByInn(inn);
        HaveId<long> contractorEntity = null;

        if (contractorCreate == null && inn != null)
        {
            var contractorDto = _contractorService.GetByInnFromSoliq(inn);
            var mc = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ContractorDto, CreateContractorDlDto>();
            });
            var createContractorDlDto = mc.CreateMapper().Map<CreateContractorDlDto>(contractorDto.Result);

            contractorEntity = _contractorService.Create(createContractorDlDto);
            CombineStatuses(_contractorService);
            if (HasErrors)
                return null;
            _unitOfWork.Save();
        }
        #endregion
        long? contractorIds = contractorEntity?.Id ?? contractorCreate?.Id;
        var contractorForPlace = _unitOfWork.ContractorRepository.ById(contractorIds.Value);

        var regionId = contractorForPlace.RegionId != null ? contractorForPlace.RegionId : contractorCreate.RegionId;
        var region = _unitOfWork.RegionRepository.ById<RegionListDto>(regionId);

        var districtId = contractorForPlace.DistrictId != null ? contractorForPlace.DistrictId : contractorCreate.DistrictId;
        var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(districtId);

        var contractorId = contractorForPlace.Id != null ? contractorForPlace.Id : contractorCreate.Id;
        var contractor = _unitOfWork.ContractorRepository.ById<ContractorListDto>(contractorId);

        try
        {
            decimal NetIncome = 0;
            decimal NetIncomeNow = 0;
            inn = contractor.Inn;
            bool isAnnualReportSubmitted = true;
            SoliqAosAylanmaData benefit = null;
            SoliqQqsAylanmaData benefitQqs = null;
            SoliqQqsAylanmaData benefitNowQqs = null;

            var dateNow = DateTime.Today.AsDateOnly();
            if (contractor.InnOrPinfl.Length < 10 && contractor.Pinfl == null)
            {



                benefit = await _soliq.GetAosAylanmaData(
                       inn: int.Parse(inn), year: DateTime.Now.Year - 1);

                if (benefit == null)
                {
                    for (int i = 1; i < 13; i++)
                    {
                        benefitQqs = await _soliq.GetQqsAylanmaData(month: i,
                        inn: int.Parse(inn), DateTime.Now.Year - 1);
                        if (benefitQqs == null || benefitQqs.NetIncomeWithoutVat == null)
                        {
                            isAnnualReportSubmitted = false;
                            continue;
                        }
                        NetIncome += benefitQqs.NetIncomeWithoutVat;
                    }
                }
                else
                {
                    NetIncome = benefit.NetIncome;
                }

                //if (benefit == null && contractor.RegistrationDate.AddYears(1) < dateNow)
                //{
                //    if (!isAnnualReportSubmitted)
                //    {
                //        if (NetIncome > 0)
                //            CombineStatuses(_soliq);
                //    }
                //    else
                //    {
                //        if (NetIncome == 0 && NetIncome == null)
                //            CombineStatuses(_soliq);
                //    }
                //}

            }
            if (HasErrors)
            {
                return new()
                {
                    Application = new()
                };
            }

            var category = UnitOfWork.Context.Set<ContractorCategoryCriterion>()
                 .Include(c => c.ContractorCategory).ThenInclude(c => c.Translates)
                 .FirstOrDefault(c =>
                     (c.MinAmount == null || c.MinAmount < NetIncome) &&
                     (c.MaxAmount == null || c.MaxAmount > NetIncome)
                 );
            decimal benefitNow = 0;
            if (contractor.Inn != null)
            {
                for (int i = 1; i < DateTime.Now.Month; i++)
                {
                    benefitNowQqs = await _soliq.GetQqsAylanmaData(month: i,
                    inn: int.Parse(inn), DateTime.Now.Year);
                    if (benefitNowQqs == null || benefitNowQqs.NetIncomeWithoutVat == 0)
                    {
                        continue;
                    }
                    benefitNow += benefitNowQqs.NetIncomeWithoutVat;
                }
                // benefitNow = _contractorService.GetAylanmaByInn(contractor.Inn);
            }
            if (benefitNow != 0)
            {
                var categoryNow = UnitOfWork.Context.Set<ContractorCategoryCriterion>()
                     .Include(c => c.ContractorCategory).ThenInclude(c => c.Translates)
                     .FirstOrDefault(c =>
                         (c.MinAmount == null || c.MinAmount < benefitNow) &&
                         (c.MaxAmount == null || c.MaxAmount > benefitNow)
                     );
                if (categoryNow != null && IsPayed(categoryNow.ContractorCategoryId, contractor.OpfId ?? 0))
                {
                    category = categoryNow;
                }
            }
            if (category == null)
            {
                AddError($"Category not found benefit.NetIncome: {NetIncome}");
                //return null;
            }

            SoliqContractorEmployeeCountByTinDataDto employeeCount = null;

            if (contractor.Inn != null)
                employeeCount = await _contractorService.GetEmployeeCountFromSoliq(
                    contractor.Inn,
                    DateTime.Now.Year,
                    DateTime.Now.Month - 1);

            //CombineStatuses(_contractorService);

            if (employeeCount == null)
                employeeCount = new() { MonthlyNumberEmployees = 0 };

            return new MemshipApplicationDto
            {
                Application = new()
                {
                    Contractor = contractor.FullName,
                    ContractorInn = contractor.Inn ?? contractor.Pinfl,
                    DocOn = DateTime.Now.AsDateOnly(),
                    ContractorId = contractor.Id,
                    ContractorPositionName = "Директор",
                    ContractorAddress = contractor.Address,
                    ContractorDirector = contractor.Director,
                    RegionId = contractor.RegionId,
                    Region = region.FullName,
                    DistrictId = contractor.DistrictId,
                    District = district.FullName,
                    ApplicationTypeId = ApplicationTypeIdConst.MEMSHIP,
                    DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                },
                ContractorEmail = contractor.Email ?? "",
                RegistrationDate = contractor.RegistrationDate,
                ContractorMobilePhoneNumber = contractor.MobilePhoneNumber ?? "",
                ContractorWorkPhoneNumber = contractor.WorkPhoneNumber ?? "",
                ContractorAdditionalPhoneNumber = contractor.AdditionalPhoneNumber ?? "",
                ContractorFaks = contractor.Faks ?? "",
                ContractorSkype = contractor.Skype ?? "",
                ContractorFacebook = contractor.Facebook ?? "",
                ContractorTelegram = contractor.Telegram ?? "",
                ContractorWebSite = contractor.WebSite ?? "",
                OwnerName = contractor.OwnerName,

                Files = new(),
                CanEdit = true,
                ContractorCategoryId = category.ContractorCategoryId,
                ContractorCategory = category.ContractorCategory.Translates
                    .AsQueryable()
                    .FirstOrDefault(
                    ContractorCategoryTranslate.GetExpr(
                        TranslateColumn.full_name,
                        _cultureHelper.CurrentCulture.Id)
                    )?.TranslateText ?? category.ContractorCategory.FullName,
                YearlyEarnings = NetIncome,
                MemshipContractTypeId = (IsPayed(category.ContractorCategoryId, contractor.OpfId ?? 0)) ? MemshipContractTypeIdConst.PAID : MemshipContractTypeIdConst.FREE,
                OkedId = (contractor.OkedId.HasValue) ? contractor.OkedId : null,
                NowYearlyEarnings = benefitNow,
                EmployeesCount = employeeCount.MonthlyNumberEmployees,
                OrganizationId = (IsPayed(category.ContractorCategoryId, contractor.OpfId ?? 0)) ? OrganizationIdConst.SSP : null,
                CanSelectOrganization = (IsPayed(category.ContractorCategoryId, contractor.OpfId ?? 0)) ? true : false
            };
        }
        catch (Exception ex)
        {
            AddError(ex.Message + "  Inner: " + ex.InnerException);
            return null;
        }
    }
    public override MemshipApplicationDto Get(long id)
    {
        SoliqQqsAylanmaData benefitNowQqs = null;
        var dto = Repository.ById<MemshipApplicationDto>(id);

        if (dto is null)
        {
            AddError("Малумот топилмади");
            return null;
        }
        decimal benefitNow = 0;

        if(dto.Application == null)
        {
			AddError("Малумот топилмади");
			return null;
		}

        if (dto?.Application?.ContractorInn != null && long.TryParse(dto.Application.ContractorInn, out long inn))
        {
            inn = long.Parse(dto.Application.ContractorInn);
            for (int i = 1; i < DateTime.Now.Month; i++)
            {
                try
                {
					benefitNowQqs = _soliq.GetQqsAylanmaData(month: i, inn: inn, DateTime.Now.Year).Result;
					if (benefitNowQqs == null || benefitNowQqs.NetIncomeWithoutVat == 0)
					{
						continue;
					}
					benefitNow += benefitNowQqs.NetIncomeWithoutVat;
				}
                catch (Exception ex)
                {
					AddError($"Ошибка при получении данных: {ex.Message}");
					
                }
             
            }
        }
        //benefitNow = _contractorService.GetAylanmaByInn(dto.Application.ContractorInn);

        dto.MemshipContractTypeId = IsPayed(dto.ContractorCategoryId, dto.Application.ContractorOpfId ?? 0)
            ? MemshipContractTypeIdConst.PAID
            : MemshipContractTypeIdConst.FREE;

        dto.NowYearlyEarnings = benefitNow;
        if (dto == null)
            return null;
        if (_authService.Contractor != null)
        {
            dto.CanAccept = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED);
            //dto.CanReject = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED);
            dto.CanCancel = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.CANCELED);
            dto.CanEdit = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.MODIFIED);
            dto.CanSend = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.SENT_FOR_REVIEW);
            dto.CanRevoke = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REVOKED);
            dto.CanDelete = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.DELETED);
        }
        else
        {
            dto.CanAccept = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED)
                              && _authService.HasPermission(ModuleCode.MemshipApplicationAccept);
            //dto.CanReject = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED)
            //                  && _authService.HasPermission(ModuleCode.MemshipApplicationReject);
            dto.CanCancel = StatusIdConst.CanMemshipApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.CANCELED)
                              && _authService.HasPermission(ModuleCode.ClaimApplicationCancel);
        }

        dto.MemshipContractId = UnitOfWork.MemshipContractRepository.AllAsQueryable
            .FirstOrDefault(c =>
            c.ApplicationId == dto.Application.Id
            && c.StatusId != StatusIdConst.DELETED)?.Id;
        return dto;
    }
    private static bool IsPayed(int contractorCategoryId, int opfId)
    {
        return (contractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA
                    || opfId == OpfIdConst.UYUSHMA);
    }
    private IQueryable<TDto> GetQuery<TDto>()
        where TDto : class
    {
        return Repository.ReadAsNoTracked<TDto>();
    }
    public MemshipApplicationDto Get(Guid id2)
    {
        var dto = GetQuery<MemshipApplicationDto>()
                 .FirstOrDefault(a => a.Application.Id2 == id2
                 && new int[] { StatusIdConst.SENT, StatusIdConst.ACCEPTED }
                 .Contains(a.Application.StatusId));

        if (dto == null)
            AddError("Ariza topilmadi / Заявление не найдено!");
        return dto;
    }

    public async ValueTask<CreateMemshipApplicationDto> Create(CreateMemshipApplicationDlDto dto, CancellationToken token)
    {
        #region Collecting external data for application
        var result = new CreateMemshipApplicationDto();

        var dateForNew = DateOnly.FromDateTime(DateTime.Now);
        var memshipCertificates = UnitOfWork.MemshipCertificateRepository.AllAsQueryable
            .Where(x => x.ContractorId == _authService.Contractor.Id && x.StatusId != StatusIdConst.DELETED)
            .OrderByDescending(x => x.ExpireOn)
            .ToList();

        if(memshipCertificates.Any() && memshipCertificates.FirstOrDefault().ExpireOn > dateForNew)
        {
            AddError("Sertifikat tugash muddati tugamagan!");
            return null;
        }

        if (Repository.AllAsQueryable.Any(a =>
            a.Application.ContractorId == _authService.Contractor.Id
            && a.Application.StatusId != StatusIdConst.REJECTED
            && a.Application.StatusId != StatusIdConst.DELETED
            && a.Application.StatusId != StatusIdConst.CANCELED
            && (a.Application.Contractor.MemshipCertificates
                .OrderBy(c => c.ExpireOn) // Sertifikatlarni tugash sanasiga ko'ra tartiblash
                .LastOrDefault(c => c.ContractorId == _authService.Contractor.Id).ExpireOn) > dateForNew))
        {
            AddError("Ariza allaqachon yaratilgan / Заявка уже создана");
            return null;
        }

        var contractor = UnitOfWork.Context.Set<Contractor>()
           .Include(c => c.SettlementAccounts)
           .FirstOrDefault(c => c.Id == _authService.Contractor.Id);

        contractor.OwnerName ??= dto.OwnerName;

        if (contractor == null)
        {
            AddError("Contractor not found");
            return null;
        }
        else if (contractor.Pinfl == null)
        {
            var soliqInfo = await _soliq.GetByInn(contractor.Inn);
            if (soliqInfo == null || soliqInfo?.Company?.StatusDetail?.Group != "ACTIVE")
            {
                AddError("Faoliyatni tugatgan");
                AddError($": {soliqInfo?.Company?.StatusDetail?.NameUzLatn}");
                AddError($"(qo'shimcha ma'lumot uchun SOLIQ organlariga murojaat qiling)");
                return null;
            }
        }

        string inn = _authService.Contractor.Inn;

        SoliqContractorEmployeeCountByTinDataDto employeeCount = null;
        if (contractor.Inn != null)
            employeeCount = await _contractorService.GetEmployeeCountFromSoliq(
                contractor.Inn,
                DateTime.Now.Year,
                DateTime.Now.Month - 1);

        if (employeeCount == null)
            employeeCount = new() { MonthlyNumberEmployees = 0 };

        dto.EmployeesCount = employeeCount.MonthlyNumberEmployees >= 0 ? employeeCount.MonthlyNumberEmployees : dto.EmployeesCount;
        dto.OkedId = (contractor.OkedId.HasValue) ? contractor.OkedId : dto.OkedId;
        #endregion

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                #region CONTACTS
                var contractorContacts = _contractorService.Get(_authService.Contractor.Id).Contacts.OrderByDescending(a => a.Id);

                // Har bir kontakt turi uchun mavjudligini tekshiruv va kerak bo'lsa qo'shish
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.EMAIL && a.Contact == dto.ContractorEmail))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.EMAIL, dto.ContractorEmail);

                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.MOBILE_PHONE && a.Contact == dto.ContractorMobilePhoneNumber))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.MOBILE_PHONE, dto.ContractorMobilePhoneNumber);

                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.WORK_PHONE && a.Contact == dto.ContractorWorkPhoneNumber))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.WORK_PHONE, dto.ContractorWorkPhoneNumber);
                
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.ADDITIONAL_PHONE && a.Contact == dto.ContractorAdditionalPhoneNumber))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.ADDITIONAL_PHONE, dto.ContractorAdditionalPhoneNumber);
                
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.FAKS && a.Contact == dto.ContractorFaks))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.FAKS, dto.ContractorFaks);
                
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.SKYPE && a.Contact == dto.ContractorSkype))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.SKYPE, dto.ContractorSkype);
                
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.FACEBOOK && a.Contact == dto.ContractorFacebook))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.FACEBOOK, dto.ContractorFacebook);
                
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.TELEGRAM && a.Contact == dto.ContractorTelegram))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.TELEGRAM, dto.ContractorTelegram);
                
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.WEB_SITE && a.Contact == dto.ContractorWebSite))
                    _contractorService.AddContactInfo(_authService.Contractor.Id, ContactTypeIdConst.WEB_SITE, dto.ContractorWebSite);
                #endregion

                var entity = Repository.Create(dto, ent => { });
                CombineStatuses(Repository);

                if (HasErrors)
                {
                    transaction.Rollback();  
                    return null;
                }

                UnitOfWork.Save();

                result.Id = entity.Id;
                result.Id2 = entity.Application.Id2;

                if (IsPayed(dto.ContractorCategoryId, contractor.OpfId ?? 0))
                {
                    await Send(new()
                    {
                        Id = result.Id,
                        Message = "MemshipApplication automatic sent from backend."
                    });

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
                else
                {
                var res = await Accept(new()
                    {
                        Id = entity.Id,
                        Message = "Contract Automatic created"
                    });

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    result.ContractId = res.contractorId;
                    result.ContractId2 = res.contractorId2;
                }

                _storageService.MoveToPersistent(
                    DocumentStorageConst.DOC_MEMSHIP_APPLICATION_FILES,
                    entity.Id.ToString(),
                    dto.Files.Select(f => f.Id).ToArray());

                CombineStatuses(_storageService);

                if (IsValid)
                {
                    transaction.Commit();
                    return result;
                }
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} : {ex.InnerException}");
            }
        }
        return null;
    }
    public async ValueTask<CreateMemshipApplicationDto> CreateForErp(CreateMemshipApplicationDlDto dto)
    {
        #region ContractorCreate memshipcontractni erp tomondan yaratish uchun

        Contractor contractorCreate = _unitOfWork.ContractorRepository.ByInn(dto.ContractorInn);
        HaveId<long> contractorEntity = null;

        if (contractorCreate == null && dto.ContractorInn != null)
        {
            var contractorDto = _contractorService.GetByInnFromSoliq(dto.ContractorInn);
            var mc = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ContractorDto, CreateContractorDlDto>();
            });
            var createContractorDlDto = mc.CreateMapper().Map<CreateContractorDlDto>(contractorDto.Result);

            contractorEntity = _contractorService.Create(createContractorDlDto);
            CombineStatuses(_contractorService);
            if (HasErrors)
                return null;
            _unitOfWork.Save();
            dto.Application.ContractorId = contractorEntity.Id;
        }

        #endregion

        #region Collecting external data for application

        var result = new CreateMemshipApplicationDto(); // User ni Shartnoma oynasiga o'tkazib berish uchun.
        var dateForNew = DateOnly.FromDateTime(DateTime.Now);
        var memshipCertificates = UnitOfWork.MemshipCertificateRepository.AllAsQueryable
            .Where(x => x.ContractorId == dto.Application.ContractorId && x.StatusId != StatusIdConst.DELETED)
            .OrderByDescending(x => x.ExpireOn)
            .ToList();

        if (memshipCertificates.Any() && memshipCertificates.FirstOrDefault().ExpireOn > dateForNew)
        {
            AddError("Sertifikat tugash muddati tugamagan! / Заявка уже создана");
            return null;
        }

        // Dublikat arizani tekshirish
        if (Repository.AllAsQueryable
            .Any(a =>
                a.Application.ContractorId == contractorCreate.Id &&
                a.Application.StatusId != StatusIdConst.REJECTED &&
                a.Application.StatusId != StatusIdConst.DELETED &&
                a.Application.StatusId != StatusIdConst.CANCELED &&
                (a.Application.Contractor.MemshipCertificates.Where(x => x.StatusId != StatusIdConst.DELETED)
                                                            .OrderByDescending(x => x.ExpireOn)
                                                            .FirstOrDefault().ExpireOn) > dateForNew))
        {
            AddError("Dublikat ariza yoki muddati tugamagan sertifikat mavjud.");
            return null;
        }

        var contractor = UnitOfWork.Context.Set<Contractor>()
                         .Include(c => c.SettlementAccounts)
                         .FirstOrDefault(c => (contractorCreate != null && c.Id == contractorCreate.Id) ||
                         (contractorCreate == null && contractorEntity != null && c.Id == contractorEntity.Id));

        contractor.OwnerName ??= dto.OwnerName;

        if (contractor == null)
        {
            AddError("Contractor not found");
            return null;
        }
        else if (contractor.Pinfl == null)
        {
            var soliqInfo = await _soliq.GetByInn(contractor.Inn);
            if (soliqInfo == null)
            {
                AddError("Faoliyatni tugatgan");
                AddError($": {soliqInfo?.Company?.StatusDetail?.NameUzLatn}");
                AddError($"(qo'shimcha ma'lumot uchun SOLIQ organlariga murojaat qiling)");
                return null;
            }
        }

        string inn = dto.ContractorInn;

        SoliqContractorEmployeeCountByTinDataDto employeeCount = null;
        if (contractor.Inn != null)
            employeeCount = await _contractorService.GetEmployeeCountFromSoliq(
                contractor.Inn,
                DateTime.Now.Year,
                DateTime.Now.Month - 1);

        if (employeeCount == null)
            employeeCount = new() { MonthlyNumberEmployees = 0 };

        dto.EmployeesCount = employeeCount.MonthlyNumberEmployees >= 0 ? employeeCount.MonthlyNumberEmployees : dto.EmployeesCount;
        dto.OkedId = (contractor.OkedId.HasValue) ? contractor.OkedId : dto.OkedId;

        #endregion

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                #region CONTACTS
                var contractorContacts = _contractorService.Get(contractor.Id).Contacts.OrderByDescending(a => a.Id);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.EMAIL && a.Contact == dto.ContractorEmail))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.EMAIL, dto.ContractorEmail);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.MOBILE_PHONE && a.Contact == dto.ContractorMobilePhoneNumber))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.MOBILE_PHONE, dto.ContractorMobilePhoneNumber);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.WORK_PHONE && a.Contact == dto.ContractorWorkPhoneNumber))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.WORK_PHONE, dto.ContractorWorkPhoneNumber);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.ADDITIONAL_PHONE && a.Contact == dto.ContractorAdditionalPhoneNumber))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.ADDITIONAL_PHONE, dto.ContractorAdditionalPhoneNumber);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.FAKS && a.Contact == dto.ContractorFaks))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.FAKS, dto.ContractorFaks);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.SKYPE && a.Contact == dto.ContractorSkype))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.SKYPE, dto.ContractorSkype);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.FACEBOOK && a.Contact == dto.ContractorFacebook))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.FACEBOOK, dto.ContractorFacebook);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.TELEGRAM && a.Contact == dto.ContractorTelegram))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.TELEGRAM, dto.ContractorTelegram);
                if (!contractorContacts.Any(a => a.ContactTypeId == ContactTypeIdConst.WEB_SITE && a.Contact == dto.ContractorWebSite))
                    _contractorService.AddContactInfo(contractor.Id, ContactTypeIdConst.WEB_SITE, dto.ContractorWebSite);
                #endregion

                var entity = Repository.Create(dto, ent => { });
                CombineStatuses(Repository);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                UnitOfWork.Save();

                result.Id = entity.Id;
                result.Id2 = entity.Application.Id2;
                var res = CreateDocumentChangeLog(entity.Id, statusId: StatusIdConst.CREATED, message: "CreateMemshipApplicationFromErp");

                if (dto.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA)
                {
                    await Send(new() { Id = result.Id, Message = "MemshipApplication From Erp automatic sent from backend." });

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
                else
                {
                    AcceptForErp(new() { Id = entity.Id, Message = "Contract Automatic created" }, out long contractId, out Guid contractId2);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }
                    CreateContractDocumentChangeLog(entity.Id, "MemshipContract Automatic created");
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    result.ContractId = contractId;
                    result.ContractId2 = contractId2;
                }

                _storageService.MoveToPersistent(
                        DocumentStorageConst.DOC_MEMSHIP_APPLICATION_FILES,
                        entity.Id.ToString(),
                        dto.Files.Select(f => f.Id).ToArray());

                CombineStatuses(_storageService);

                if (IsValid)
                {
                    transaction.Commit();
                    return result;
                }
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} : {ex.InnerException}");
            }
        }
        return null;
    }
    private HaveId<long> CreateContractDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
    {
        var application = Repository.ById<MemshipApplicationDto>(id, applyFilter: false);
        if (application == null || application.MemshipContractId == null)
        {
            return null;
        }
        var entityDto = UnitOfWork.MemshipContractRepository.ById<MemshipContractDto>(application.MemshipContractId.Value, applyFilter: false);
        _documentChangeLogService.Create(
            dto: entityDto,
            tableId: TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT,
            organizationId: null,
            statusId: entityDto.StatusId,
            message: message,
            userIp: userIp,
            userAgent: userAgent);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    public override void Update(UpdateMemshipApplicationDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            var entity = Repository.Update(dto, ent =>
            {
                if (!StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.MODIFIED))
                    AddError("Имкони йўқ / Нет доступа");
            });

            CombineStatuses(Repository);
            if (IsValid)
                UnitOfWork.Save();

            _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_MEMSHIP_APPLICATION_FILES, entity.Id.ToString());
            var res = CreateDocumentChangeLog(entity.Id,
                statusId: StatusIdConst.MODIFIED,
                message: "UpdateMemshipApplication");
            if (IsValid)
            {
                transaction.Commit();
            }
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusMemshipApplicationDlDto dto, Action<MemshipApplication> validation)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? _unitOfWork.BeginTransaction()
        : null;
        validation += Validation(dto);
        dto.IsRead = false;
        //var doc = Repository.AllAsQueryable.FirstOrDefault(x => x.ApplicationId == dto.Id);
        //dto.Id = doc.Id;
        try
        {
            var entity = Repository.UpdateStatus(dto, validation);
            CombineStatuses(Repository);
            if (HasErrors)
                throw new();
            var res = CreateDocumentChangeLog(entity.Id,
                statusId: dto.StatusId,
                message: "MemshipApplication - UpdateStatus to " + dto.StatusId);
            _unitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
            return HaveId.Create(entity.Id);
        }
        finally
        {
            transaction?.Dispose();
        }

    }

    private Action<MemshipApplication> Validation(UpdateStatusMemshipApplicationDlDto dto)
    {
        return ent =>
        {
            if (!StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                AddError("Имкони йўқ / Нет доступа");
        };
    }

    public bool CanCreate(string inn = null)
    {
        inn ??= _authService.Contractor.Inn;
        var dto = Repository.CrudServices.ReadManyNoTracked<MemshipApplicationDto>()
                    .Where(a => a.Application.ContractorId == _authService.Contractor.Id
                            && a.Application.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP
                            && !new int[] { StatusIdConst.DELETED, StatusIdConst.REJECTED, StatusIdConst.CANCELED }.Contains(a.Application.StatusId))
                    .ToList();
        if (dto.Any())
            return false;
        
        return true;
    }

    public async ValueTask<(long contractorId, Guid contractorId2)> Accept(AcceptStatusMemshipApplicationDto dto)
    {

        // accept ga erpdan request keladi shunga auth dan contractorga murojat null beradi.
        long contractId = 0;
        Guid contractId2 = default;
        var application = _unitOfWork.Context.Set<MemshipApplication>()
        .Include(a => a.Application)
        .ThenInclude(a => a.Contractor)
        .FirstOrDefault(a => a.Id == dto.Id);

        if (application == null)
        { AddError("application not found"); return (contractId, contractId2); }

        var contractor = UnitOfWork.Context.Set<Contractor>()
            .Include(c => c.SettlementAccounts)
            .FirstOrDefault(c => c.Id == application.Application.ContractorId);

        if (contractor == null)
        { AddError("contractor not found"); return (contractId, contractId2); }

        var orgByRegion = UnitOfWork.Context.Set<Organization>()
                                                      .FirstOrDefault(org =>
                                                      IsPayed(application.ContractorCategoryId, contractor.OpfId ?? 0) ?
                                                      org.Id == OrganizationIdConst.SSP
                                                      : ((application.ChooseLocation ?
                                                          org.RegionId == application.ChoosedRegionId
                                                          : org.RegionId == application.Application.RegionId)
                                                          && (org.OrganizationGroupId == OrganizationGroupIdConst.SSP || org.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)));

        if (orgByRegion == null)
        { AddError("orgByRegion not found"); return (contractId, contractId2); }
        var districtId = application.ChooseLocation ? application.ChoosedDistrictId : application.Application.DistrictId;

        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
        try
        {
            var res = UpdateStatus(dto, ent => { });
            MemshipContract contract = new();
            if (!IsPayed(application.ContractorCategoryId, contractor.OpfId ?? 0))
                contract = UnitOfWork.MemshipContractRepository.Create(new()
                {
                    ApplicationId = application.ApplicationId,
                    DocOn = DateOnly.FromDateTime(DateTime.Now),
                    DocNumber = _numberService.GetNext(
                        NumberTemplateDocumentConst.DOC_MEMSHIP_CONTRACT,
                        organizationId: orgByRegion.Id,
                        regionId: orgByRegion.RegionId,
                        districtId: districtId ?? 0).Item2,
                    MemshipContractTypeId = IsPayed(application.ContractorCategoryId, contractor.OpfId ?? 0) ? MemshipContractTypeIdConst.PAID : MemshipContractTypeIdConst.FREE,
                    ContractorSettlementAccountId = contractor.SettlementAccounts
                                .FirstOrDefault(ac => ac.IsMain)?.Id,
                    OrganizationId = IsPayed(application.ContractorCategoryId, contractor.OpfId ?? 0) ? OrganizationIdConst.SSP : orgByRegion.Id,
                    BaseFixedMinimumValue = 0,
                    ContractorCategoryId = application.ContractorCategoryId
                });

            CombineStatuses(UnitOfWork.MemshipContractRepository);
            if (HasErrors)
            {
                transaction.Rollback();
                return (contractId, contractId2); ;
            }
            UnitOfWork.Save();

            contractId = contract.Id;
            contractId2 = contract.Id2;
            if (IsValid && canCommit)
            {
                transaction.Commit();
            }
            //var contractService = _serviceProvider.GetRequiredService<IMemshipContractService>();
            //await contractService.PostToIMZOAndSentUrl(contract);
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction?.Dispose();
        }
        return (contractId, contractId2);
    }
    public void AcceptForErp(AcceptStatusMemshipApplicationDto dto, out long contractId, out Guid contractId2)
    {

        // accept ga erpdan request keladi shunga auth dan contractorga murojat null beradi.
        contractId = 0;
        contractId2 = default;
        var application = _unitOfWork.Context.Set<MemshipApplication>()
        .Include(a => a.Application)
        .ThenInclude(a => a.Contractor)
        .FirstOrDefault(a => a.Id == dto.Id);

        if (application == null)
        { AddError("application not found"); return; }

        var contractor = UnitOfWork.Context.Set<Contractor>()
            .Include(c => c.SettlementAccounts)
            .FirstOrDefault(c => c.Id == application.Application.ContractorId);

        if (contractor == null)
        { AddError("contractor not found"); return; }

        var orgByRegion = UnitOfWork.Context.Set<Organization>()
             .FirstOrDefault(org =>
             IsPayed(application.ContractorCategoryId, contractor.OpfId ?? 0) ?
             org.Id == OrganizationIdConst.SSP
             : ((application.ChooseLocation ?
                 org.RegionId == application.ChoosedRegionId
                 : org.RegionId == application.Application.RegionId)
                 && (org.OrganizationGroupId == OrganizationGroupIdConst.SSP || org.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)));

        if (orgByRegion == null)
        { AddError("orgByRegion not found"); return; }
        var districtId = application.ChooseLocation ? application.ChoosedDistrictId : application.Application.DistrictId;

        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
        try
        {
            var res = UpdateStatus(dto, ent => { });
            MemshipContract contract = new();

            contract = UnitOfWork.MemshipContractRepository.Create(new()
            {
                ApplicationId = application.ApplicationId,
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext(
                    NumberTemplateDocumentConst.DOC_MEMSHIP_CONTRACT,
                    organizationId: orgByRegion.Id,
                    regionId: orgByRegion.RegionId,
                    districtId: districtId ?? 0).Item2,
                MemshipContractTypeId = (application.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA) ? MemshipContractTypeIdConst.PAID : MemshipContractTypeIdConst.FREE,
                ContractorSettlementAccountId = contractor.SettlementAccounts
                            .FirstOrDefault(ac => ac.IsMain)?.Id,
                OrganizationId = IsPayed(application.ContractorCategoryId, contractor.OpfId ?? 0) ? OrganizationIdConst.SSP : orgByRegion.Id,
                BaseFixedMinimumValue = 0,
                ContractorCategoryId = application.ContractorCategoryId
            });

            CombineStatuses(UnitOfWork.MemshipContractRepository);
            if (HasErrors)
            {
                transaction.Rollback();
                return;
            }
            UnitOfWork.Save();

            contractId = contract.Id;
            contractId2 = contract.Id2;
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction?.Dispose();
        }
    }
    //public void Reject(RejectStatusMemshipApplicationDto dto)
    //{
    //    var canCommit = _unitOfWork.CurrentTransaction == null;
    //    var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

    //    try
    //    {
    //        _unitOfWork.Context.Set<Application>().Lock(dto.Id);

    //        UpdateStatus(dto, ent => { });
    //        _unitOfWork.Save();
    //        if (IsValid && canCommit)
    //            transaction.Commit();
    //    }
    //    catch (DbUpdateException e)
    //    {
    //        AddError(e.Message + " - " + e.InnerException);
    //        if (canCommit)
    //            transaction.Rollback();
    //    }
    //    finally
    //    {
    //        if (canCommit)
    //            transaction.Dispose();
    //    }
    //}

    public void Cancel(CancelStatusMemshipApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {
            _unitOfWork.Context.Set<Application>().Lock(dto.Id);

            Repository.UpdateStatus(dto, ent => { });

            CombineStatuses(Repository);

            if (HasErrors)
                return;
            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(id: dto.Id, statusId: dto.StatusId, message: "MemshipApplication");

            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }

    public void Revoke(RevokeStatusMemshipApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {
            _unitOfWork.Context.Set<Application>().Lock(dto.Id);

            UpdateStatus(dto, ent => { });
            _unitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }

    public async Task Send(SendStatusMemshipApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {
            _unitOfWork.Context.Set<MemshipApplication>().Lock(dto.Id);

            UpdateStatus(dto, ent => { });
            _unitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }

    }

    public byte[] DownloadPdf(Guid id2, string? lang)
    {
        //lang = lang ?? "uz-cyrl";

        //var user = _authService.User;
        //var userLanguage = _unitOfWork.Context.Set<User>().Where(a => a.Id == user.Id).Include(l => l.Language).FirstOrDefault();

        //var languageId = userLanguage.LanguageId ?? 1;

        //var wordFile = _storageService.GetStaticFile(
        //    StaticFileConst.WordTemplate.GetFileName(
        //        ServiceProvider.CultureHelper.CurrentCulture.Code,
        //        StaticFileConst.WordTemplate.MEMSHIP_APPLICATION)
        //    );


        lang = lang ?? "uz-latn";

        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
                lang,
                StaticFileConst.WordTemplate.MEMSHIP_APPLICATION)
            );
        var languageId = UnitOfWork.Context.Set<Language>()
            .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;

        var memshipApplication = UnitOfWork.Context.Set<MemshipApplication>()
            .Include(a => a.Application)
            .Include(a => a.ChamberServices)
            .Include(a => a.ContractorCategory.Translates)
            .Include(a => a.ContractorActivityType.Translates)
            .FirstOrDefault(a => a.Application.Id2 == id2);

        if (memshipApplication == null)
        {
            AddError("not found");
            return null;
        }

        var contractorId = memshipApplication.Application.ContractorId;
        var contractor = UnitOfWork.Context.Set<Contractor>()
            .Include(c => c.Oked)
            .ThenInclude(x => x.Translates)
            .Include(c => c.SettlementAccounts)
            .Include(c => c.Bank)
            .Include(c => c.Region.Translates)
            .Include(c => c.District.Translates)
            .Include(c => c.Bank.Translates)
            .FirstOrDefault(c => c.Id == contractorId);
        decimal? benefitNow = null;
        if (contractor.Inn != null)
            benefitNow = _contractorService.GetAylanmaByInn(contractor.Inn);
        //var sattlementAcc = UnitOfWork.Context.Set<ContractorSettlementAccount>()
        //    .FirstOrDefault(a => a.OwnerId == contractor.Id).AccountCode;

        //var serviceIds = memshipApplication.ChamberServices
        //    .Select(s => s.NeedChamberServiceId)
        //    .ToList();

        var services = UnitOfWork.Context.Set<NeedChamberService>()
            //.Where(t => serviceIds.Contains(t.Id))
            .AsEnumerable();
        var link = _systemConf.QrImagePrintMy + "/MemshipApplication/DownloadPdf?id2=" + memshipApplication.Application.Id2.ToString();
        var qrCode = QRCodeHelper.GeneratePng(link);
        var qrImage = new MemoryStream(qrCode);
        var plh = new Placeholders();
        plh.ImagePlaceholders.Add("QrCode", new ImageElement
        {
            Dpi = 512,
            MemStream = qrImage,
        });
        plh.TextPlaceholders.Add(nameof(memshipApplication.Application.DocNumber), memshipApplication.Application.DocNumber ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.Application.DocOn), memshipApplication.Application.DocOn.ToString("dd.MM.yyyy") ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorWorkPhoneNumber), memshipApplication?.ContractorWorkPhoneNumber ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorMobilePhoneNumber), memshipApplication?.ContractorMobilePhoneNumber ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorFaks), memshipApplication.ContractorFaks ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorAdditionalPhoneNumber), memshipApplication?.ContractorAdditionalPhoneNumber ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorEmail), memshipApplication?.ContractorEmail ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorWebSite), memshipApplication?.ContractorWebSite ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorSkype), memshipApplication?.ContractorSkype ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorFacebook), memshipApplication?.ContractorFacebook ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorTelegram), memshipApplication?.ContractorTelegram ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorActivityType), memshipApplication?.ContractorActivityType?.Translates
                .FirstOrDefault(c =>
                c.LanguageId == languageId
                && c.ColumnName == TranslateColumn.full_name.ToString()
                )?.TranslateText ?? memshipApplication?.ContractorActivityType?.FullName ?? "-");

        plh.TextPlaceholders.Add(nameof(memshipApplication.ContractorCategory), memshipApplication.ContractorCategory.Translates
                .FirstOrDefault(c =>
                    c.LanguageId == languageId
                    && c.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? memshipApplication.ContractorCategory.FullName ?? "-");

        plh.TextPlaceholders.Add(nameof(contractor.RegistrationDate), contractor.RegistrationDate.ToString("dd.MM.yyyy") ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.EmployeesCount), memshipApplication.EmployeesCount.ToString() ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.YearlyEarnings), memshipApplication.YearlyEarnings?.ToString().FormatNumber(3) ?? "-");
        plh.TextPlaceholders.Add(nameof(benefitNow), benefitNow?.ToString().FormatNumber(3) ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.YearlyTaxes), memshipApplication.YearlyTaxes?.ToString().FormatNumber(3) ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.YearlyExport), memshipApplication.YearlyExport?.ToString().FormatNumber(3) ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.YearlyImport), memshipApplication.YearlyImport?.ToString().FormatNumber(3) ?? "-");
        plh.TextPlaceholders.Add(nameof(memshipApplication.YearlyManufacture), memshipApplication?.YearlyManufacture?.ToString().FormatNumber(3) ?? "-");
        plh.TextPlaceholders.Add(nameof(contractor.FullName), contractor.FullName ?? "-");
        plh.TextPlaceholders.Add(nameof(contractor.Director), contractor.Director ?? "-");
        plh.TextPlaceholders.Add(nameof(contractor.Region), contractor.Region.Translates
                .FirstOrDefault(t =>
                    t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? contractor.Region.FullName ?? "-");

        plh.TextPlaceholders.Add(nameof(contractor.District), contractor.District.Translates
                .FirstOrDefault(t =>
                    t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? contractor.District.FullName ?? "-");

        plh.TextPlaceholders.Add(nameof(contractor.Address), contractor.Address ?? "-");
        plh.TextPlaceholders.Add(nameof(contractor.RegistrationNumber), contractor.RegistrationNumber ?? "-");
        plh.TextPlaceholders.Add(nameof(contractor.Inn), contractor.Inn ?? "-");
        plh.TextPlaceholders.Add(nameof(contractor.Oked), contractor?.Oked?.Code + " - " + contractor?.Oked?.FullName ?? "-");

        plh.TextPlaceholders.Add(nameof(contractor.Bank.BankCode), contractor?.Bank?.BankCode?.Code ?? "-");
        plh.TextPlaceholders.Add(nameof(contractor.SettlementAccounts), contractor?.SettlementAccounts?.FirstOrDefault(a => a.IsMain)?.AccountCode ?? "-");
        plh.TextPlaceholders.Add(nameof(contractor.Bank.BankName), contractor?.Bank?.Translates.FirstOrDefault(c =>
                c.LanguageId == languageId
                && c.ColumnName == TranslateColumn.full_name.ToString()
                )?.TranslateText ?? contractor.Bank?.BankName ?? "-");

        //plh.TextPlaceholders.Add(nameof(memshipApplication.ChamberServices), String.Join(",", services.Select(a => a.FullName)) ?? "-");
        var servicesPlh = new List<Placeholders>();
        foreach (var service in services)
        {
            var itemPlh = new Placeholders();
            itemPlh.TextPlaceholders.Add(nameof(service), service.FullName);
            servicesPlh.Add(itemPlh);
        }
        plh.TemplateListPlaceholders.Add(nameof(memshipApplication.ChamberServices), servicesPlh);
        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        /*< --ChamberServices-- >
       ##service##
        < --/ ChamberServices-- >*/

        var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
        if (res == null)
        {
            Document document = new Document();
            document.LoadFromStream(wordFile, FileFormat.Docx);
            var pdfStream = new MemoryStream();
            document.SaveToStream(pdfStream, FileFormat.PDF);
            return pdfStream.ToArray();
        }
        CombineStatuses(_pdfConverter);
        return res;
    }

    public byte[] DownloadPdf(MemshipApplicationForPdf model)
    {
        model.lang = model.lang ?? "uz-cyrl";

        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
                ServiceProvider.CultureHelper.CurrentCulture.Code,
                StaticFileConst.WordTemplate.MEMSHIP_APPLICATION)
            );

        var languageId = UnitOfWork.Context.Set<Language>()
            .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;
        decimal? benefitNow = null;
        if (model.Inn != null)
            benefitNow = _contractorService.GetAylanmaByInn(model.Inn);
        var link = "FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE ";
        var qrCode = QRCodeHelper.GeneratePng(link);
        var qrImage = new MemoryStream(qrCode);
        var plh = new Placeholders();
        plh.ImagePlaceholders.Add("QrCode", new ImageElement
        {
            Dpi = 512,
            MemStream = qrImage,
        });

        var modelType = model.GetType();
        var properties = modelType.GetProperties();
        plh.TextPlaceholders.Add(nameof(benefitNow), benefitNow?.ToString().FormatNumber(3) ?? "-");
        foreach (var item in properties)
        {
            var name = item.Name;
            plh.TextPlaceholders.Add(name, item.GetValue(model).ToString());

        }

        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
        if (res == null)
        {
            Document document = new Document();
            document.LoadFromStream(wordFile, FileFormat.Docx);
            var pdfStream = new MemoryStream();
            document.SaveToStream(pdfStream, FileFormat.PDF);
            return pdfStream.ToArray();
        }
        //CombineStatuses(_pdfConverter);
        return res;
    }

    private HaveId<long> CreateDocumentChangeLog(long id,
        int statusId,
        string message = null,
        string userIp = null,
        string userAgent = null)
    {
        var entityDto = Repository.ReadAsNoTracked<MemshipApplicationDto>(applyFilter: false)
            .FirstOrDefault(x => x.Id == id);

        entityDto.Application.Id = id;

        //if (_authService.Contractor != null)

        //    _documentChangeLogService.CreateApplication(
        //        dto: entityDto,
        //        organizationId: null,
        //        message: message);
        //else

        _documentChangeLogService.Create(
            dto: entityDto.Application,
            tableId: TableIdConst.MEMSHIP__DOC_MEMSHIP_APPLICATION,
            statusId: statusId,
            organizationId: _authService?.User?.OrganizationId ?? null,
            message: message
            );
        CombineStatuses(_documentChangeLogService);
        _unitOfWork.Save();
        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }

    private Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.MEMSHIP__DOC_MEMSHIP_APPLICATION)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }

    #region Files
    public IEnumerable<MemshipApplicationFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_MEMSHIP_APPLICATION_FILES, files).Select(a => new MemshipApplicationFileDto
        {
            Id = a.FileId,
            FileName = a.FileName
        });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }

    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<MemshipApplicationFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_APPLICATION_FILES);
    }

    public void DeleteFile(Guid fileId)
    {
        var entity = _unitOfWork.Context
            .Set<MemshipApplicationFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_APPLICATION_FILES);
    }

    private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        if (entity != null)
        {
            _storageService.DeleteTemp(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
    }

    private StorageFile Download(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        StorageFile file;

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

    public Stream SaveAsExcel(MemshipApplicationSortFilterOptions options)
    {
        var data = Repository.ReadAsNoTracked<MemshipApplicationListDto>()
            .SortFilter(options).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.MEMSHIP_APPLICATION));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;
            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Id;
                ws.Cells[currentRow, column++].Value = item.Application.DocNumber;
                ws.Cells[currentRow, column++].Value = item.Application.DocOn.ToString(Constants.DATE_FORMAT);
                ws.Cells[currentRow, column++].Value = item.Application.ContractorInn + " - " + item.Application.Contractor;
                ws.Cells[currentRow, column++].Value = item.Application.ContractorPhoneNumber;
                ws.Cells[currentRow, column++].Value = item.ContractorActivityType;
                ws.Cells[currentRow, column++].Value = item.OkedCode;
                ws.Cells[currentRow, column++].Value = item.EmployeesCount;
                ws.Cells[currentRow, column++].Value = item.Application.Region;
                ws.Cells[currentRow, column++].Value = item.Application.District;
                ws.Cells[currentRow, column++].Value = item.Application.Status;
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    public async Task<string> SummQqsOrAos(string innProp, int year, int n)
    {
        SoliqAosAylanmaData benefit = null;
        SoliqAosAylanmaData benefitNow = null;
        SoliqQqsAylanmaData benefitQqs = null;
        SoliqQqsAylanmaData benefitQqsNow = null;
        int inn = int.Parse(innProp);
        decimal NetIncome = 0;
        decimal NetIncomeNow = 0;

        benefit = await _soliq.GetAosAylanmaData(
                       inn: inn, year: year);
        bool isAos = true;
        if (benefit == null)
        {
            for (int i = 1; i < 13; i++)
            {
                benefitQqs = await _soliq.GetQqsAylanmaData(month: i,
                inn: inn, year);
                if (benefitQqs == null || benefitQqs.NetIncomeWithoutVat == 0)
                {
                    continue;
                }
                NetIncome += benefitQqs.NetIncomeWithoutVat;
            }
            isAos = false;
        }
        else
        {
            NetIncome = benefit.NetIncome;
            isAos = true;
        }

        benefitNow = await _soliq.GetAosAylanmaData(
                      inn: inn, year: year + 1);

        if (benefitNow == null)
        {
            for (int i = 1; i < n; i++)
            {
                benefitQqsNow = await _soliq.GetQqsAylanmaData(month: i,
                inn: inn, year + 1);
                if (benefitQqs == null || benefitQqs.NetIncomeWithoutVat == 0)
                {
                    continue;
                }
                NetIncomeNow += benefitQqs.NetIncomeWithoutVat;
            }
            isAos = false;
        }
        else
        {
            NetIncomeNow = benefit.NetIncome;
            isAos = true;
        }
        var categoryNow = UnitOfWork.Context.Set<ContractorCategoryCriterion>()
                     .Include(c => c.ContractorCategory)
                     .FirstOrDefault(c =>
                         (c.MinAmount == null || c.MinAmount < NetIncomeNow) &&
                         (c.MaxAmount == null || c.MaxAmount > NetIncomeNow)
                     );
        string res = $"{NetIncome}\n {categoryNow.ContractorCategoryId} \n IsAos:{isAos} \n  ";

        return res;
    }
    public async Task<List<string>> MistakeSetPropsInContractorList(int type, int pageSize, int pageNumber)
    {

        var contractors = _unitOfWork.Context
                 .MemshipApplications.Include(x => x.Application).ThenInclude(x => x.Contractor)
                .Where(x => x.CreatedAt.Month > 2
            && x.CreatedAt.Year == 2024
            && x.Application.StatusId != 5
            && x.Application.StatusId != 23
            && x.Application.StatusId != 24
            && x.Application.StatusId != 25
            && x.ContractorCategoryId == type)
                .OrderByDescending(x => x.Id) // Ensure a consistent order
                .Skip((pageNumber - 1) * pageSize) // Skip the records of previous pages
               .Take(pageSize) // Take the current page's records
                .ToList();
        var res = new List<string>();
        foreach (var contractor in contractors)
        {
            if (contractor.Application.Contractor.Inn.NullOrEmpty())
                continue;
            var trueData = await GetForErp(contractor.Application.Contractor.Inn);
            decimal temp = contractor.YearlyEarnings.Value;
            contractor.YearlyEarnings = trueData.YearlyEarnings;
            int temp2 = contractor.ContractorCategoryId;
            contractor.ContractorCategoryId = trueData.ContractorCategoryId;

            _unitOfWork.Save();

            res.Add("  id: " + contractor.Id.ToString() 
                + " sum :" + temp
                +" =>  "  + trueData.YearlyEarnings.ToString() 
                + " category: " + temp2
                + " =>  "+ trueData.ContractorCategoryId.ToString() 
                + " inn: " + contractor.Application.Contractor.Inn);
        }
        return res;

    }

}
