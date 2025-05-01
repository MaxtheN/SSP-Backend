using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.Claim.Doc.ApplicationForCourtServices;
using SspUis.BizLogicLayer.ClaimApplicationServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.AgroBank.Models;
using SspUis.Integration.AgroBank.Services;
using SspUis.Integration.DocxToPdf;
using SspUis.Integration.Sud.Models;
using SspUis.Integration.Sud.Models.AuthModels;
using SspUis.Integration.Sud.Models.MalumotnomaModel;
using SspUis.Integration.Sud.Services;
using SspUis.Integration.XalqBank.Services;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Claim;

public class ApplicationForCourtService
    : BaseEntityService<long
        , ApplicationForCourt
        , ApplicationForCourtListDto
        , ApplicationForCourtDto
        , CreateApplicationForCourtDlDto
        , UpdateApplicationForCourtDlDto
        , IApplicationForCourtRepository>
    , IApplicationForCourtService
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly INumberService _numberService;
    private readonly SystemConf _systemConf;
    private readonly IConvertService _pdfConverter;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IEImzoService _eImzoService;
    private readonly IAgroBankService _agroBankService;
    private readonly IXalqBankService _xalqBankService;
    private readonly IClaimApplicationRepository _claimApplicationRepository;
    private readonly ISudService _sudService;
    private readonly ICultureHelper _cultureHelper;
    private IWbImzoService _wbImzoService;
    private WbImzoConfig _wbImzoConfig;
    private readonly LinkConfig _linkConfig;

    public ApplicationForCourtService(
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IStorageService storageServic,
        IAgroBankService agroBankService,
        IXalqBankService xalqBankService,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        SystemConf systemConf,
        IConvertService pdfConverter,
        IEImzoService eImzoService,
        ICultureHelper cultureHelper,
        IClaimApplicationRepository claimApplicationRepository,
        WbImzoConfig wbImzoConfig,
        List<LinkConfig> linkConfigs,
        IWbImzoService wbImzoService,
        ISudService sudService)
        : base(unitOfWork)
    {
        _wbImzoService = wbImzoService;
        _wbImzoConfig = wbImzoConfig;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "ApplicationForCourt");
        _authService = authService;
        _storageService = storageServic;
        _agroBankService = agroBankService;
        _xalqBankService = xalqBankService;
        _numberService = numberService;
        _documentChangeLogService = documentChangeLogService;
        this._systemConf = systemConf;
        this._pdfConverter = pdfConverter;
        _eImzoService = eImzoService;
        _claimApplicationRepository = claimApplicationRepository;
        _sudService = sudService;
        _unitOfWork = unitOfWork;
        _cultureHelper = cultureHelper;
    }

    public PagedResult<ApplicationForCourtListDto> GetList(ApplicationForCourtSortFilterOptions options)
    {
        var result = Repository.ReadAsNoTracked<ApplicationForCourtListDto>()
                    .SortFilter(options)
                    .ToTableData(options);

        if (options.IsEmployee && !(_authService.HasPermission(ModuleCode.AcademicDegreeLawsuit)))
            result.Rows = result.Rows
                .Where(x => x.EmployeeManageId == _authService.User.EmployeeManageId && (new int[] { StatusIdConst.ACCEPTED, StatusIdConst.SENT, StatusIdConst.CREATED, StatusIdConst.MODIFIED }).Contains(x.StatusId));

        else if (_authService.User.Id != 1 && !(_authService.HasPermission(ModuleCode.AcademicDegreeLawsuit)))
            result.Rows = result.Rows
                .Where(x => x.EmployeeManageId == _authService.User.EmployeeManageId);

        return result;
    }

    public override ApplicationForCourtDto Get()
    {
        return new ApplicationForCourtDto()
        {
            DocNumber = _numberService.GetNext(
            nameof(TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT),
            organizationId: _authService.IsAuthenticated ? _authService.Organization.Id : OrganizationIdConst.SSP)
            .Item2,
            DocOn = DateOnly.FromDateTime(DateTime.Today),
            TableId = TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT
        };
    }
   
    public async Task<GetLoanActualDataResponse2> GetLoanActualData(string loanId)
    {
        var res = await _agroBankService.GetLoanActualData(loanId);
        CombineStatuses(_agroBankService);
        return res;
    }

    public async Task<CheckAmountFromBankDto> ChekAllBanks(long mediationId)
    {
        var mediation = UnitOfWork.Context.Set<Mediation>()
                         .FirstOrDefault(m => m.Id == mediationId);

        if(mediation == null)
        {
            AddError("Mediation id null");
            return null;
        }
        var contractor = (from m in UnitOfWork.Context.Set<Mediation>()
                          join mp in UnitOfWork.Context.Set<MediationPlan>()
                          on m.MediationPlanId equals mp.Id
                          join a in UnitOfWork.Context.Set<ClaimApplication>()
                          on mp.ApplicationId equals a.ApplicationId
                          where m.Id == mediationId
                          select a.Application.Contractor)
                          .FirstOrDefault();

        if(contractor.Id == 63770)
        {
            var result = await ChekFromXalqBank(mediationId);
            return result;
        }
        else if(contractor.Id == 5046)
        {
            var result = await ChekFromAgroBank(mediationId);
            return result;
        }

        else
        {
            AddError("Bu bankdan  summani chek qilib olish hali yulga quyilmagan");
            return null;
        }


    }
    
    public async Task<CheckAmountFromBankDto> ChekFromAgroBank(long mediationId)
    {

        var mediation = UnitOfWork.Context.Set<Mediation>()
                                 .FirstOrDefault(m => m.Id == mediationId);
        if (mediation == null)
        {
            AddError("Mediation id null");
            return null;
        }
        var application = (from m in UnitOfWork.Context.Set<Mediation>()
                           join mp in UnitOfWork.Context.Set<MediationPlan>()
                           on m.MediationPlanId equals mp.Id
                           join a in UnitOfWork.Context.Set<ClaimApplication>()
                           on mp.ApplicationId equals a.ApplicationId
                           where m.Id == mediationId
                           select a).Include(a => a.Application)
                           .ThenInclude(app => app.Contractor)
                           .FirstOrDefault();


        if (application != null)
        {
            var dataFromAgroBank =  await  _agroBankService.GetLoanActualData(application.ContractIdentificationNumber);
            if (dataFromAgroBank == null)
            {
                AddError($"Bu hujjat raqmi {application.ContractIdentificationNumber} boyicha AgroBankdan ma'lumot topilmadi");
                return null;
            }

            var result = new CheckAmountFromBankDto()
            {
                //Bank
                MainDebtFromBank =  dataFromAgroBank.OverdueMainDept,//dataFromAgroBank.BalanceRemainder,
                CalculedPenaltyFromBank =   dataFromAgroBank.OverduePercentDept,//dataFromAgroBank.OverdueRemainder,
                PenaltyFromBank = dataFromAgroBank.CurrentMainDept, //dataFromAgroBank.PercentsRemainder,
				PercentFromBank =   dataFromAgroBank.PenaltyInterest,
                CurrentPrincipalInterestFromBank =   dataFromAgroBank.OverdueDebtAccruedInterest,  //процентная доля
                CurrentInterestRateFromBank =   dataFromAgroBank.AccruedInterestCurrentDebt,//dataFromAgroBank.PenaltiesRemainder,
                OtherDebtRePaymentFromBank =   0, 
                DateFromBank = DateOnly.FromDateTime(DateTime.Now),
                Summ = dataFromAgroBank.OverduePercentDept
						 + dataFromAgroBank.AccruedInterestCurrentDebt
						 + dataFromAgroBank.CurrentMainDept
						 + dataFromAgroBank.OverdueMainDept,
                //Ssp
                CalculedPenaltyFromSsp = application.CalculedPenalty,
                CurrentInterestRateFromSsp = application.CurrentInterestRate,
                CurrentPrincipalInterestFromSsp = application.CurrentPrincipalInterest,
                MainDebtFromSsp = application.MainDebt,
                PenaltyFromSsp = application.Penalty,
                PercentFromSsp = application.Percent,
                OtherDebtRePaymentFromSsp = application.OtherDebtRepayment,
                DateFromSsp = application.Application.DocOn,
                BankName = application.Application.Contractor.FullName,
            };
            return result;
        }
        else
        {
            AddError("Bu ariza Agro Bankga Tegishli emas");
        }


        return null ;
	}


//2. overdueMainDebt
//3. overduePercentDebt
//4. currentMainDebt
//5. penaltyInterest
//6. overdueDebtAccruedInterest
//7. accruedInterestCurrentDebt

//8. (Не обновляется. Данные не будут возвращаться)

	public async Task<CheckAmountFromBankDto> ChekFromXalqBank(long mediationId)
    {
        var mediation = UnitOfWork.Context.Set<Mediation>()
                         .FirstOrDefault(m => m.Id == mediationId);

        if (mediation == null)
        {
            AddError("Mediation id null");
            return null;
        }
        var application = (from m in UnitOfWork.Context.Set<Mediation>()
                           join mp in UnitOfWork.Context.Set<MediationPlan>()
                           on m.MediationPlanId equals mp.Id
                           join a in UnitOfWork.Context.Set<ClaimApplication>()
                           on mp.ApplicationId equals a.ApplicationId
                           where m.Id == mediationId
                           select a).Include(a => a.Application)
                           .ThenInclude(app=>app.Contractor)
                           .FirstOrDefault();

        if (application != null)
        {
            var dataFromXalqBank = await _xalqBankService.ChekAmountXalqBank(application.ContractIdentificationNumber);
            if (dataFromXalqBank == null)
            {
                AddError($"Bu hujjat raqmi {application.ContractIdentificationNumber} boyicha XalqBankdan ma'lumot topilmadi");
                return null;
            }
            var result = new CheckAmountFromBankDto()
            {
                //Bank
                CalculedPenaltyFromBank = dataFromXalqBank.CalculedPenalty  ,
                CurrentInterestRateFromBank = dataFromXalqBank.CurrentInterestRate,
                CurrentPrincipalInterestFromBank = dataFromXalqBank.CurrentPrincipalInterest,
                OtherDebtRePaymentFromBank = dataFromXalqBank.OtherDebtRePayment,
                PenaltyFromBank = dataFromXalqBank.Penalty,
                PercentFromBank = dataFromXalqBank.Percent,
                MainDebtFromBank = dataFromXalqBank.MainDebt,
                DateFromBank = DateOnly.FromDateTime(DateTime.Now),
                Summ = dataFromXalqBank.CalculedPenalty
                          + dataFromXalqBank.CurrentPrincipalInterest
                          + dataFromXalqBank.OtherDebtRePayment
                          + dataFromXalqBank.Penalty
                          + dataFromXalqBank.Percent
                        + dataFromXalqBank.MainDebt,
                //Ssp
                CalculedPenaltyFromSsp = application.CalculedPenalty,
                CurrentInterestRateFromSsp = application.CurrentInterestRate,
                CurrentPrincipalInterestFromSsp = application.CurrentPrincipalInterest,
                MainDebtFromSsp = application.MainDebt,
                PenaltyFromSsp = application.Penalty,
                PercentFromSsp = application.Percent,
                OtherDebtRePaymentFromSsp = application.OtherDebtRepayment,
                DateFromSsp = application.Application.DocOn,
                BankName = application.Application.Contractor.FullName,
            };
            return result;
        }
        else
        {
            AddError("Bu ariza Xalq Bankga Tegishli emas");
        }
        return null;
    }

    public ApplicationForCourtDto GetByMediationId(long mediationId)
    {
        var mediation = UnitOfWork.Context.Set<Mediation>()
            .Include(x => x.Status).ThenInclude(x => x.Translates)
            .Include(x => x.MediationPlan).ThenInclude(x => x.Application).ThenInclude(x => x.CurrentStep).ThenInclude(x => x.Translates)
            .Include(x => x.MediationPlan).ThenInclude(x => x.Contractor)
            .AsSplitQuery()
            .FirstOrDefault(x => x.Id == mediationId);

        if (mediation == null)
        { AddError("Mediation not found"); return null; }

        var forCourt = UnitOfWork.Context.Set<ApplicationForCourt>()
            .FirstOrDefault(x => x.MediationId == mediation.Id && x.StatusId != StatusIdConst.DELETED);

        return new ApplicationForCourtDto()
        {
            Id = forCourt == null ? 0 : forCourt.Id,
            Contractor = mediation.Contractor.FullName,
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            StatusId = mediation.StatusId,
            Status = mediation.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                ?? mediation.Status.FullName,
            StepId = mediation.MediationPlan.Application.CurrentStepId,
            Step = mediation.MediationPlan.Application.CurrentStep != null
            ? mediation.MediationPlan.Application.CurrentStep.Translates.AsQueryable()
                .FirstOrDefault(ApplicationTypeStepTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                ?? mediation.MediationPlan.Application.CurrentStep.FullName
            : string.Empty,
            DocNumber = mediation.DocNumber,
            MediationId = mediation.Id,
            ContractorId = mediation.ContractorId,
        };
    }

    public async Task<byte[]> DownloadXalqBankDocument(long mediationId, string langu)
    {
        var lang = langu ?? "uz-cyrl";
        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
                lang,
                StaticFileConst.WordTemplate.XALQ_BANK_MEDIATION_DOCUMENT)
            );
        
        var mediation = UnitOfWork.Context.Set<Mediation>()
                         .FirstOrDefault(m => m.Id == mediationId);
        var data = new CheckAmountFromBankDto();

        if (mediation == null)
        {
            AddError("Mediation id null");
            return null;
        }
        var contractor = (from m in UnitOfWork.Context.Set<Mediation>()
                          join mp in UnitOfWork.Context.Set<MediationPlan>()
                          on m.MediationPlanId equals mp.Id
                          join a in UnitOfWork.Context.Set<ClaimApplication>()
                          on mp.ApplicationId equals a.ApplicationId
                          where m.Id == mediationId
                          select a.Application.Contractor)
                          .FirstOrDefault();

        if (contractor.Id == 63770)
        {
            var result = await ChekFromXalqBank(mediationId);
            data = result;
        }
        else if (contractor.Id == 5046)
        {
            var result = await ChekFromAgroBank(mediationId);
            data = result;
        }
        var plh = new Placeholders();
       
        plh.TextPlaceholders.Add(nameof(data.BankName), data.BankName?.ToString() ?? "-");
        plh.TextPlaceholders.Add(nameof(data.DateFromBank), data.DateFromBank?.ToString() ?? "-");
        plh.TextPlaceholders.Add(nameof(data.CalculedPenaltyFromBank), data.CalculedPenaltyFromBank?.ToString() ?? "0");
        plh.TextPlaceholders.Add(nameof(data.CurrentInterestRateFromBank), data.CurrentInterestRateFromBank?.ToString() ?? "0");
        plh.TextPlaceholders.Add(nameof(data.CurrentPrincipalInterestFromBank), data.CurrentPrincipalInterestFromBank?.ToString() ?? "0");
        plh.TextPlaceholders.Add(nameof(data.MainDebtFromBank), data.MainDebtFromBank?.ToString() ?? "0");
        plh.TextPlaceholders.Add(nameof(data.PercentFromBank), data.PercentFromBank?.ToString() ?? "0");
        plh.TextPlaceholders.Add(nameof(data.PenaltyFromBank), data.PenaltyFromBank?.ToString() ?? "0");
        plh.TextPlaceholders.Add(nameof(data.OtherDebtRePaymentFromBank), data.OtherDebtRePaymentFromBank?.ToString() ?? "0");
        plh.TextPlaceholders.Add(nameof(data.Summ), data.Summ?.ToString() ?? "0");

        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();

        return await _pdfConverter.DocxToPdfAsync(wordFile, new());
    }

    public async Task<byte[]> DownloadBankDocument(long mediationId)
    {
        var lang = "uz-cyrl";
        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
                lang,
                StaticFileConst.WordTemplate.BANK_MEDIATION_DOCUMENT)
            );

        var data = ChekFromAgroBank(mediationId);

        var plh = WordFactory.MakePlaceholders(
            new
            {
                Data = data
            }, "N1");
        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        return await _pdfConverter.DocxToPdfAsync(wordFile, new());
    }
    
    public override ApplicationForCourtDto Get(long id)
    {
        var dto = Repository.ById<ApplicationForCourtDto>(id);
        if (dto == null)
            return null;
        return dto;
    }

    public override HaveId<long> Create(CreateApplicationForCourtDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Create(dto);
                CombineStatuses(Repository);
                if (HasErrors)
                    return null;

                #region Validation
                if (dto.MediationId.HasValue)
                {
                    var mediation = UnitOfWork.Context.Set<Mediation>().AsNoTracking()
                        .Include(x => x.MediationPlan).ThenInclude(x => x.Application)
                        .FirstOrDefault(x => x.Id == dto.MediationId && x.StatusId != StatusIdConst.DELETED);

                    if (mediation is null)
                    { AddError("Bunday Mediatsiya bayonnomasi mavjud emas !"); transaction.Rollback(); return null; }

                    var claimApplication = UnitOfWork.Context.Set<ClaimApplication>().AsNoTracking()
                        .FirstOrDefault(a => a.ApplicationId == mediation.MediationPlan.ApplicationId);

                    if (claimApplication.ClaimApplicationTypeId != ClaimApplicationTypeIdConst.COUNTER_CLAIM || claimApplication.ClaimApplicationTypeId != ClaimApplicationTypeIdConst.APILATION_CASSATION || claimApplication.ClaimApplicationTypeId != ClaimApplicationTypeIdConst.CLAIM_REVISION)
                    {
                        var applicationForCourt = UnitOfWork.Context.Set<ApplicationForCourt>().Any(a => a.MediationId == dto.MediationId);
                        if (applicationForCourt)
                        {
                            AddError("Bunday Mediatsiya bayonnomasi bilan Davo arizasi(sudga) yaratilgan");
                            transaction.Rollback();
                            return null;
                        }
                    }

                    _claimApplicationRepository.UpdateStep(
                        new UpdateStepDlDto
                        {
                            Id = mediation.MediationPlan.ApplicationId,
                            StepId = StepIdConst.APPLICATION_FOR_COURT_CREATE
                        });
                }
                else if (dto.ApplicationId.HasValue)
                {
                    var claimApplication = UnitOfWork.Context.Set<ClaimApplication>().AsNoTracking()
                        .FirstOrDefault(a => a.ApplicationId == dto.ApplicationId);

                    if (claimApplication is null)
                    { AddError("Bunday Davo arizasi mavjud emas !"); transaction.Rollback(); return null; }



                    if (claimApplication.ClaimApplicationTypeId != ClaimApplicationTypeIdConst.COUNTER_CLAIM || claimApplication.ClaimApplicationTypeId != ClaimApplicationTypeIdConst.APILATION_CASSATION || claimApplication.ClaimApplicationTypeId != ClaimApplicationTypeIdConst.CLAIM_REVISION)
                    {
                        var applicationForCourt = UnitOfWork.Context.Set<ApplicationForCourt>().Any(a => a.ApplicationId == dto.ApplicationId);
                        if (applicationForCourt)
                        {
                            AddError("Bunday Davo arizasi(sudga) yaratilgan");
                            transaction.Rollback();
                            return null;
                        }
                    }

                    _claimApplicationRepository.UpdateStep(
                        new UpdateStepDlDto
                        {
                            Id = dto.ApplicationId.Value,
                            StepId = StepIdConst.APPLICATION_FOR_COURT_CREATE
                        });
                }
                else if (dto.MediationId == null && dto.ApplicationId == null)
                {
                    AddError("MediationId and ApplicationId is null");
                    transaction.Rollback();
                    return null;
                }
                #endregion

                CombineStatuses(_claimApplicationRepository);
                if (HasErrors)
                { transaction.Rollback(); return null; }

                if (IsValid)
                    UnitOfWork.Save();

                SaveFiles(entity, dto.Files);
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CreateApplicationForCourt");

                if (HasErrors)
                { transaction.Rollback(); return null; }

                if (IsValid)
                    transaction.Commit();

                return HaveId.Create(entity.Id);
            }
            catch (DbException ex)
            {
                AddError($"{ex.Message} // {ex.InnerException}");
                transaction.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} // {ex.InnerException}");
                transaction.Rollback();
                throw;
            }
        }
    }

    public override void Update(UpdateApplicationForCourtDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            var entity = Repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(Repository);

            _storageService.ResolveMarkedFiles(
                DocumentStorageConst.DOC_APPLICATION_FOR_COURT_FILE,
                $"{dto.Id}");

            if (IsValid)
                UnitOfWork.Save();

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateApplicationForCourt");

            if (IsValid)
            {
                transaction.Commit();
            }
        }
    }

    public override void Delete(long id)
    {

        var statusDto = new UpdateStatusApplicationForCourtDlDto()
        {
            Id = id,
            StatusId = StatusIdConst.DELETED
        };
        var res = UpdateStatus(statusDto, ent =>
        {
            if (!StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, statusDto.StatusId))
                AddError("Нет доступа");
        });

    }
    #region STATUS

    #region WebImzo
    public async ValueTask<string> PostToIMZOAndSentUrl(ApplicationForCourt contract)
    {
        if (contract.WebImzoSecretKey != null)
            return await SendUrl(contract.Id);

        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(contract);

        {
            signRequestCreateDto.SignRequestUsers = new List<WbImzoCreateSignRequestUserDto>
                                                    {
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = _authService.User.Inn == null ? _authService.User.Pinfl : _authService.User.Inn,
                                                            UserInfo = _authService.User.ToTextForDocumentLog(),
                                                            UserId = (int)_authService.UserId,
                                                            DocStatusId = StatusIdConst.CREATED,
                                                            SignPriority = 1,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = _authService.User.PhoneNumber,
                                                        }
                                                    };
        }

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if (HasErrors || wbImzoResult.Response is null)
        {
            AddError("Tasdiqlash jarayonda xatolik yuz berdi joyda xatolik yuz berdi");
            return null;
        }

        try
        {
            contract.WebImzoRequestId = wbImzoResult.Response.RequestId;
            contract.WebImzoSecretKey = wbImzoResult.Response.SecretKey;

            _unitOfWork.Save();
            CombineStatuses(this);
            if (canDispose)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError("Document did not update" + ex.Message);
        }

        return await SendUrl(contract.Id);
    }

    private async ValueTask<string?> SendUrl(long contractId)
    {

        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        var contract = _unitOfWork.Context.Set<ApplicationForCourt>().FirstOrDefault(a => a.Id == contractId);

        if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
        {
            AddError("Contract imzolash uchun yuborilayotgan jarayonda qaytgan keylani saqlashda xatolik yuz berdi");
            return null;
        }

        if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
        {
            await PostToIMZOAndSentUrl(contract);
        }

        var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

        return url;

    }

    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(ApplicationForCourt contract)
    {
        var organization = _unitOfWork.OrganizationRepository.AllAsQueryable.FirstOrDefault(a => a.Id == contract.OrganizationId);


        string documentDataAsString = JsonConvert.SerializeObject(ConvertToDto(contract));
        var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}&lang=uz-cyrl&__lang=uz-cyrl&langId=2";

        return new WbImzoCreateSignRequestDto
        {
            DocumentId = contract.Id,
            ApiKey = _wbImzoConfig.ApiKey,
            Title = contract.DocNumber,
            IsForceCreate = true,
            TableId = TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT,
            SignData = documentDataAsString,
            OrganizationInn = organization != null ? organization.Inn : null,
            OrganizationName = organization != null ? organization.FullName : null,
            PrintableLink = filePrintableLink,
            SignRequestActionTypes = new()
            {
                new WbImzoCreateSignRequestActionTypeDto
                {
                    ActionTypeId =  StatusIdConst.SIGNED,
                    ActionTypeName = "SIGNED",
                        Translates = new()
                        {
                            new ActionTranslateTypeDto
                            {
                                LanguageCode = LanguageCodeConst.RU,
                                Name =  "Я согласен"
                            },
                           new ActionTranslateTypeDto
                           {
                                LanguageCode = LanguageCodeConst.UZ_LATN,
                                Name = "Roziman"
                           },
                           new ActionTranslateTypeDto
                           {
                                LanguageCode = LanguageCodeConst.UZ_CYRL,
                                Name = "Розиман"
                           },
                        }
                },
            },
            SignatureMethodIds = new List<int> { SignatureMethodIdConst.E_IMZO },
            TemplateId = 28
        };
    }
    
    public WebImzoDto ConvertToDto(ApplicationForCourt dto)
    => new()
    {
        DocOn = dto.DocOn,
        StatusId = dto.StatusId,
        DocNumber = dto.DocNumber,
        Id = dto.Id,
        OrganizationId = dto.OrganizationId,
    };

    public async ValueTask<(string? Url, bool Result)> WebImzoSign(WebImzoSignedFilter filter)
    {
        var doc = _unitOfWork.Context.Set<ApplicationForCourt>()
                                                  .Include(a => a.EmployeeManage).ThenInclude(a => a.Employee).ThenInclude(a => a.Person)
                                                  .Include(x => x.Signs)
                                                  .FirstOrDefault(x => x.Id == filter.Id && x.StatusId != StatusIdConst.DELETED);

        var signerPinfl = doc.EmployeeManage.Employee.Person.Pinfl; 

        if (doc == null)
        { AddError("Bunday hujjat mavjud emas!"); return (null, false); }

        if (_authService.User.Pinfl != doc.EmployeeManage.Employee.Person.Pinfl)
        {
            AddError($"Imzolayotgan odam - {_authService.User.Pinfl} Imzolashi kerak bolgan odam - {signerPinfl} -  Imzolovchilar mos emas");
            return (Url:null, Result:false);
        }


        var url = await PostToIMZOAndSentUrl(doc);

        if (url == null)
            return (Url: null, Result: false);

        return (Url: url, Result: true);

    }

    #endregion WebImzo

    public async Task Accept(AcceptUpdateStatusApplicationForCourtDlDto dto)
    {
        var doc = Repository.ById<ApplicationForCourtDto>(dto.Id, true);

        var updateDto = new UpdateStatusApplicationForCourtDlDto
        {
            Id = dto.Id,
            StatusId = StatusIdConst.ACCEPTED,
        };

        if (doc == null)
        { 
            AddError("Bunday hujjat mavjud emas!"); 
            return; 
        }

        var currentPrtnContractSign = UnitOfWork.Context.Set<ApplicationForCourt>()
            .Select(a => new
            {
                Id = a.Id,
                EmployeeManageId = a.EmployeeManageId,
                Pinfl = a.EmployeeManage.Employee.Person.Pinfl
            })
            .FirstOrDefault(a => a.Id == doc.Id);

        var eImzoTimeStampDto = new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
        };

        if (_authService.User.Pinfl != currentPrtnContractSign.Pinfl)
        {
            AddError($"Imzolayotgan odam - {_authService.User.Pinfl} Imzolashi kerak bolgan odam - {currentPrtnContractSign.Pinfl} -  Imzolovchilar mos emas");
            return;
        }

        if (HasErrors)
            return;

        EImzoTimeStampResultDto eImzoTimeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
            Inn = _authService.User.Inn,
            Pinfl = _authService.User.Pinfl
        });

        CombineStatuses(_eImzoService);
        if (HasErrors) 
            return;

        var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
        {
            SignData = eImzoTimeStamp.Pkcs7b64,
            Inn = null,
            Pinfl = _authService.User.Pinfl
        });

        CombineStatuses(_eImzoService);
        if (HasErrors) 
            return;

        dto.SignFile = SaveFile(dto.Id, eImzoTimeStamp.Pkcs7b64, "prtnSign.txt");
        dto.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
        dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

        //_claimApplicationRepository.UpdateStep(
        //    new UpdateStepDlDto
        //    {
        //        Id = application.ApplicationId,
        //        StepId = StepIdConst.ACCEPT
        //    });

        //var res = UpdateStatus(
        //      new() { Id = dto.Id, StatusId = StatusIdConst.CREATED }
        //      , ent =>
        //      {
        //          if (!StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.CREATED))
        //              AddError("Нет доступа");
        //      });

        Repository.UpdateStatus(updateDto, ent =>
        {
            if(!StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.ACCEPTED))
                AddError("Имкони йўқ / Нет доступа");
            else
            {
                ent.StatusId = StatusIdConst.ACCEPTED;
                ent.StepId = StepIdConst.ACCEPT;
                CreateDocumentChangeLog(dto.Id, StatusIdConst.ACCEPTED);
            }
        });

        CombineStatuses(Repository);
        if (HasErrors)
            return;
        
        _unitOfWork.Save();
    }
    
    public async Task AcceptForEmployee(AcceptUpdateStatusApplicationForCourtDlDto dto)
    {
        if (!_authService.HasPermission(ModuleCode.ApplicationForCourtAccept))
        {
            AddError("У вас нет разрешения на создание других организации.");
            return;
        }



        var eImzoTimeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
            Inn = _authService.User.Inn,
            Pinfl = _authService.User.Pinfl
        });

        CombineStatuses(_eImzoService);
        if (HasErrors)
            return;

        if (_authService.Contractor != null)
        {
            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = eImzoTimeStamp.Pkcs7b64,
                Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                Pinfl = _authService.Contractor.Pinfl
            });
        }
        else
        {
            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = eImzoTimeStamp.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.User.Pinfl
            });
        }


        CombineStatuses(_eImzoService);
        if (HasErrors)
            return;

        var entity = Repository.Context.Set<ApplicationForCourt>()
            .Include(x => x.Mediation).ThenInclude(x => x.MediationPlan).ThenInclude(x => x.Application)
            .FirstOrDefault(x => x.Id == dto.Id && x.StatusId != StatusIdConst.DELETED);

        if (entity is null)
        { AddError("Bunday ariza hali yaratilmagan !"); return; };

        Repository.UpdateStep(entity, new UpdateStepApplicationForCourtDlDto { StepId = StepIdConst.ACCEPT_THAT_EMPLOYEE });
        CombineStatuses(Repository);
        if (HasErrors)
            return;

        _claimApplicationRepository.UpdateStep(
            new UpdateStepDlDto
            {
                Id = entity.Mediation.MediationPlan.ApplicationId,
                StepId = StepIdConst.ACCEPT_THAT_EMPLOYEE
            });

        CombineStatuses(_claimApplicationRepository);
        if (HasErrors)
            return;

        if (IsValid)
            UnitOfWork.Save();
    }
    
    public async Task<EImzoVerifyResultDto> AcceptCourt(AcceptUpdateStatusApplicationForCourtDlDto dto)
    {
        var doc = Repository.ById<ApplicationForCourtDto>(dto.Id, true);
      
        if (doc == null)
        { AddError("Bunday hujjat mavjud emas!"); return null; }

        var currentPrtnContractSign = UnitOfWork.Context.Set<ApplicationForCourt>()
            .Select(a => new
            {
                Id = a.Id,
                EmployeeManageId = a.EmployeeManageId,
                Pinfl = a.EmployeeManage.Employee.Person.Pinfl
            })
            .FirstOrDefault(a => a.Id == doc.Id);

        var eImzoTimeStampDto = new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
        };

        var res = UpdateStatusCourt(new() { Id = dto.Id, StatusId = StatusIdConst.SENT }, ent =>
        {
            if (!StatusIdConst.CanApplicationForCourtSendStatus(ent.StatusId, StatusIdConst.SENT)) AddError("Нет доступа");
        });

        if (HasErrors)
            return null;

        EImzoTimeStampResultDto eImzoTimeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
            Inn = _authService.User.Inn,
            Pinfl = _authService.User.Pinfl
        });

        CombineStatuses(_eImzoService);
        if (HasErrors) return null;


        EImzoVerifyResultDto eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
        {
            SignData = eImzoTimeStamp.Pkcs7b64,
            Inn = null,
            Pinfl = _authService.User.Pinfl
        });

        CombineStatuses(_eImzoService);
        if (HasErrors) return null;

        dto.SignFile = SaveFile(dto.Id, eImzoTimeStamp.Pkcs7b64, "prtnSign.txt");
        dto.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
        dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

        //_claimApplicationRepository.UpdateStep(
        //    new UpdateStepDlDto
        //    {
        //        Id = application.ApplicationId,
        //        StepId = StepIdConst.ACCEPT
        //    });

        //var res = UpdateStatus(new() { Id = dto.Id, StatusId = StatusIdConst.SIGNED }, ent => { if (!StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.CREATED)) AddError("Нет доступа");
        //      });

        return eImzoVerifyAttached;
    }
   
    public void Send(long id)
    {
        var res = UpdateStatus(
              new() { Id = id, StatusId = StatusIdConst.SENT }
              , ent =>
              {
                  if (!StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.SENT))
                      AddError("Нет доступа");
              });
    }
    
    public void Reject(long id, string message)
    {
        var res = UpdateStatus(
              new() { Id = id, StatusId = StatusIdConst.REJECTED, Message = message }
              , ent =>
              {
                  if (!StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.REJECTED))
                      AddError("Нет доступа");
              });

        CreateDocumentChangeLog(id, StatusIdConst.REJECTED, message);
    }
    #endregion

    #region FILES
    public async ValueTask<byte[]> DownloadFileWithQrCode(Guid id)
    {
        var data = DownloadFile(id, true);
        if (HasErrors)
            return null;
        var entity = UnitOfWork.Context.Set<ApplicationForCourtFile>().Include(a => a.Owner).FirstOrDefault(a => a.Id == id);
        MemoryStream wordFile = new MemoryStream();
        data.Item1.GetStream().CopyTo(wordFile);

        var plh = new Placeholders();
        if (data.Item2 == StepIdConst.ACCEPT && data.Item3 == StatusIdConst.ACCEPTED)
        {
            var link = _systemConf.QrImagePrintERP + $"/ApplicationForCourt/DownloadFileWithQrCode?id={id}";

            var qrData = $"{entity.Owner.Id2} Hujjat raqami {entity.Owner.DocNumber} Hujjat sanasi {entity.Owner.DocOn:dd.MM.yyyy}";
            var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(qrData));
            
            plh.ImagePlaceholders.Add("QrCode", new() { Dpi = 512, MemStream = qrDownload });
            
            plh.TextPlaceholders.Add("DocNumber", entity.Owner.DocNumber ?? "");
            plh.TextPlaceholders.Add("DocOn", entity.Owner.DocOn.ToString("dd.MM.yyyy") ?? "");
            wordFile = (new DocXHandler(wordFile, plh)).ReplaceImages();
            wordFile = (new DocXHandler(wordFile, plh)).ReplaceTexts();
        }
        else
        {
            plh.TextPlaceholders.Add("++QrCode++", "");
            plh.TextPlaceholders.Add("DocOn", entity.Owner.DocOn.ToString() ?? "");
            wordFile = (new DocXHandler(wordFile, plh)).ReplaceTexts();
        }

        var dat = await _pdfConverter.DocxToPdfAsync(wordFile, new());
        return dat;
    }
    
    public (StorageFile, int?, int) DownloadFile(Guid fileId, bool isGenerationQrCode)
    {
        StorageFile file;

        var entity = UnitOfWork.Context.Set<ApplicationForCourtFile>()
            .Include(a => a.Owner)
                .ThenInclude(x => x.Mediation)
                    .ThenInclude(x => x.MediationPlan)
                        .ThenInclude(x => x.Application)
            .AsSplitQuery()
            .FirstOrDefault(a => a.Id == fileId);

        if (isGenerationQrCode && entity.FileExtension != ".docx")
        {
            AddError("Берилган филе (word.docx) филе емас  !");
            return (null, null, 0);
        }

        if (entity == null)
        {
            file = _storageService.GetTempFile(DocumentStorageConst.DOC_APPLICATION_FOR_COURT_FILE, fileId);
            CombineStatuses(_storageService);
        }
        else
        {
            file = _storageService.GetFile(DocumentStorageConst.DOC_APPLICATION_FOR_COURT_FILE, entity.OwnerId.ToString(), fileId);
            CombineStatuses(_storageService);

            if (IsValid)
                file.FileName = entity.FileName;
        }

        return (file, entity.Owner.StepId, entity.Owner.StatusId);
    }
   
    public async ValueTask<byte[]> DownloadPdf(Guid id2)
    {
        var lang = "uz-cyrl";
        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
                _cultureHelper.CurrentCulture.Code,
                StaticFileConst.WordTemplate.APPLICATION_FOR_COURT)
            );

        var languageId = UnitOfWork.Context.Set<Language>()
            .FirstOrDefault(l => l.Code == _cultureHelper.CurrentCulture.Code)?.Id ?? 1;

        var application = await UnitOfWork.Context.Set<ApplicationForCourt>()
            .Include(x => x.ClaimOrganization)
                .ThenInclude(x => x.Translates)
            .Include(x => x.Contractor)
            .Include(x => x.Mediation)
                .ThenInclude(x => x.MediationPlan)
            .Include(x => x.Organization)
                .ThenInclude(x => x.Translates)
            .Where(a => a.Id2 == id2)
            .FirstOrDefaultAsync();

        var responsibles = UnitOfWork.Context.Set<ClaimApplicationTable>()
            .Include(x => x.Owner)
            .Where(x => x.Owner.ApplicationId == application.Mediation.MediationPlan.ApplicationId);

        var plh = new Placeholders();
        var link = _systemConf.QrImagePrintMy + "/ApplicationForCourt/DownloadPdf?id2=" + application.Id2.ToString();
        var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
        plh.ImagePlaceholders.Add("QrCode", new() { Dpi = 512, MemStream = qrDownload });

        plh.TextPlaceholders.Add(nameof(application.DocOn), application.DocOn.ToString("dd.MM.yyyy"));
        plh.TextPlaceholders.Add(nameof(application.DocNumber), application.DocNumber);
        plh.TextPlaceholders.Add(nameof(application.ClaimOrganization), application.ClaimOrganization.Translates
            .AsQueryable().FirstOrDefault(ClaimOrganizationTranslate.GetExpr(TranslateColumn.full_name, languageId))
                ?.TranslateText ?? application.ClaimOrganization.FullName);
        plh.TextPlaceholders.Add(nameof(application.Organization), application.Organization.Translates
            .AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, languageId))
                ?.TranslateText ?? application.Organization.FullName);
        plh.TextPlaceholders.Add(nameof(application.Contractor.FullName), application.Contractor.FullName);
        plh.TextPlaceholders.Add(nameof(application.Contractor.Director), application.Contractor.Director);
        plh.TextPlaceholders.Add(nameof(application.Contractor.Address), application.Contractor.Address);
        plh.TextPlaceholders.Add("ContractorDetails", string.Empty);
        plh.TextPlaceholders.Add("Details", string.Empty);
        var account = application.Contractor.SettlementAccounts.FirstOrDefault(a => a.IsMain);
        plh.TextPlaceholders.Add(nameof(account.AccountCode), account?.AccountCode ?? "-");
        plh.TextPlaceholders.Add(nameof(application.Contractor.Bank.BankCode), application.Contractor.Bank?.Code ?? "-");
        plh.TextPlaceholders.Add(nameof(application.Contractor.PhoneNumber), application.Contractor.PhoneNumber ?? "-");
        plh.TextPlaceholders.Add(nameof(application.Contractor.Inn), application.Contractor.Inn ?? "-");
        List<Placeholders> tables = new List<Placeholders>();
        foreach (var item in responsibles)
        {
            var tplh = new Placeholders();
            tplh.TextPlaceholders.Add("ResponsibleName", item.FullName ?? "");
            tplh.TextPlaceholders.Add("ResponsibleAddress", item.Address ?? "");
            tplh.TextPlaceholders.Add("ResponsiblePhone", item.PhoneNumber ?? "");
            tplh.TextPlaceholders.Add("ResponsibleInnOrPinfl", item.InnOrPinfl ?? "");
            tables.Add(tplh);
        }
        plh.TemplateListPlaceholders.Add("Tables", tables);
        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        return await _pdfConverter.DocxToPdfAsync(wordFile, new());
    }
    
    public IEnumerable<IStorageFileInfo> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_APPLICATION_FOR_COURT_FILE, files);

        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    #endregion

    #region HELPER
    private void SaveFiles(ApplicationForCourt entity, List<ApplicationForCourtFileDlDto> files)
    {
        if (files != null)
        {
            _storageService.MoveToPersistent(
                DocumentStorageConst.DOC_APPLICATION_FOR_COURT_FILE,
                $"{entity.Id}",
                files.Select(a => a.Id).ToArray());

            CombineStatuses(_storageService);
        }
    }
    
    private HaveId<long> UpdateStatus(UpdateStatusApplicationForCourtDlDto dto, Action<ApplicationForCourt> validation)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        using var transaction = canCommit ? UnitOfWork.BeginTransaction() : null;

        try
        {
            var entity = Repository.UpdateStatus(dto, validation);

            entity.Message = dto.Message;

            Repository.UpdateStep(entity, new UpdateStepApplicationForCourtDlDto { StepId = StepIdConst.ACCEPT });
            CombineStatuses(Repository);
            if (HasErrors)
                return null;

            _claimApplicationRepository.UpdateStep(
                new UpdateStepDlDto
                {
                    Id = entity.Mediation.MediationPlan.ApplicationId,
                    StepId = StepIdConst.ACCEPT
                });

            CombineStatuses(_claimApplicationRepository);
            if (HasErrors)
                return null;

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
            if (IsValid)
                UnitOfWork.Save();

            if (IsValid && canCommit)
                transaction.Commit();

            return res;
        }
        finally
        {
            transaction?.Dispose();
        }
    }
    
    private HaveId<long> UpdateStatusCourt(UpdateStatusApplicationForCourtDlDto dto, Action<ApplicationForCourt> validation)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        using var transaction = canCommit ? UnitOfWork.BeginTransaction() : null;

        try
        {
            var entity = Repository.UpdateStatus(dto, validation);

            entity.Message = dto.Message;

            Repository.UpdateStep(entity, new UpdateStepApplicationForCourtDlDto { StepId = StepIdConst.EXECUTING });
            CombineStatuses(Repository);
            if (HasErrors)
                return null;

            _claimApplicationRepository.UpdateStep(
                new UpdateStepDlDto
                {
                    Id = entity.Mediation.MediationPlan.ApplicationId,
                    StepId = StepIdConst.EXECUTING
                });

            CombineStatuses(_claimApplicationRepository);
            if (HasErrors)
                return null;

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
            if (IsValid)
                UnitOfWork.Save();

            if (IsValid && canCommit)
                transaction.Commit();

            return res;
        }
        finally
        {
            transaction?.Dispose();
        }
    }
    
    public HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<ApplicationForCourtDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    
    private void Validation<TDto>(ApplicationForCourtDlDto<TDto> dto, ApplicationForCourt entity)
           where TDto : ApplicationForCourtDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);
        }
        if (!StatusIdConst.CanApplicationForCourtApplyStatus(entity.StatusId, StatusIdConst.MODIFIED))
            AddError("Нет доступа");
    }
    
    public Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
    #endregion

    public ApplicationForCourtForGetDto GetForFiles(long mediationId)
    {
        var mediation = UnitOfWork.Context.Set<Mediation>()
            .Where(a => a.Id == mediationId)
            .Select(a => new
            {
                a.Id,
                a.MediationPlanId,
                a.Id2,
                FilesIds = a.Files.Select(f => f.Id),
                FilesNames = a.Files.Select(f => f.FileExtension)
            })
            .FirstOrDefault();

        if (mediation == null)
            return null;

        var mediationPlan = UnitOfWork.Context.Set<MediationPlan>()
            .Where(a => a.Id == mediation.MediationPlanId)
            .Select(a => new
            {
                a.Application,
                a.Id,
                a.Id2
            })
            .FirstOrDefault();

        if (mediationPlan == null || mediationPlan.Application == null)
            return null;

        var claimApplication = UnitOfWork.Context.Set<ClaimApplication>()
            .Include(a => a.Files)
            .Where(a => a.ApplicationId == mediationPlan.Application.Id)
            .Select(a => new
            {
                a.Id,
                a.ApplicationId,
                Id2 = a.Application.Id2,
                FilesIds = a.Files.Select(f => f.Id),
                FilesNames = a.Files.Select(f => f.FileExtension)
            })
            .FirstOrDefault();

        if (claimApplication == null)
            return null;

        ApplicationForCourtForGetDto result = new()
        {
            ClaimApplicationId = claimApplication.Id,
            MediationId = mediation.Id,
            MediationPlanId = mediationPlan.Id,
            claimApplicationIntegrationFileUrl = new ClaimApplicationIntegrationFileUrlDto
            {
                ClaimApplicationLink = _systemConf.QrImagePrintPath + "/ClaimApplication/DownloadPdf?Id2=" + claimApplication.Id2 + "&lang=uz-cyrl",
                MediationLink = _systemConf.QrImagePrintPath + "/Mediation/DownloadPdf?Id2=" + mediation.Id2 + "&lang=uz-cyrl",
                MediationPLanLink = _systemConf.QrImagePrintPath + "/MediationPlan/DownloadPdf?Id2=" + mediationPlan.Id2 + "&lang=uz-cyrl"
            },
            ClaimApplicationFileIds = new ClaimApplicationFileIdDto
            {
                Ids = claimApplication.FilesIds.ToList(),
                FileNames = claimApplication.FilesNames.ToList(),
            },
            MediationFileIds = new MediationFileIdDto
            {
                Ids = mediation.FilesIds.ToList(),
                FileNames = mediation.FilesNames.ToList(),
            }
        };

        return result;
    }

    #region Sud
    //Upload File sudga
    public async Task<UploadFileResponseDto> SudUploadFileIntegration(IFormFile file)
    {
        try
        {
            if (file == null)
            {
                AddError("Empty or null file");
                return null;
            }
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            string base64Data = Convert.ToBase64String(fileBytes);
            var result = new UploadFileDto
            {
                name = file.FileName,
                size = file.Length,
                data = base64Data,
                type = "application/pdf"
            };
            if (result != null)
            {
                var resultTwo = await _sudService.SudUploadFile(result);
                return resultTwo;
            }
        }
        catch (Exception ex)
        {
            AddError($"Error message uploadFile {ex.Message}", "Message");
            return null;
        }
        return null;
    }

    //Davo arizasini yuborish sudga
    public async Task<object> SendSudIntegration(CreateCourtntegrationDlDto dto)
    {
        var application = await UnitOfWork.Context.Set<ApplicationForCourt>()
                                              .Where(a => a.Id == dto.ApplicationForCourtId)
                                              .FirstOrDefaultAsync();


       

        var claimApplications = await _unitOfWork.Context
               .Set<ClaimApplication>()
               .Include(c => c.Application)
               .ThenInclude(s => s.Contractor)
               .ThenInclude(x=>x.BusinessmanUserInContractors)
               .Include(c => c.Application)
               .ThenInclude(s => s.ContractorSettlementAccount)
               .Include(x => x.Organization)
               .ThenInclude(a => a.Translates)
               .Include(c => c.ClaimApplicationType)
               .Include(c => c.Currency)
               .ToListAsync();

        var claimApplication = claimApplications
                         .FirstOrDefault(c => c.Application != null 
                         && c.Application.DocNumber == application.DocNumber);

        if(claimApplication == null )
        {
            AddError("Davo Arzasi topilmadi");
            return null;
        }

        try
        {
            Task<EImzoVerifyResultDto> dataImzo = AcceptCourt(new AcceptUpdateStatusApplicationForCourtDlDto()
            {
                SignedData  = dto.SignedData,
                Id = dto.ApplicationForCourtId
            });

            var docOn = claimApplication.Application.DocOn;
            var amount = claimApplication.TotalAmount.ToString();
            amount = Regex.Replace(amount, @"\,\d+", "");

            var result = new SendingNewClaimDto
            {
                Case = new CaseDto
                {
                    DocDate = docOn.ToString("yyyy-MM-dd"),
                    CourtId = dto.CourtId,
                    DocNumber = claimApplication.Application.DocNumber,
                    //DutyReasonId = dto.DutyReasonId,
                    //PostReasonId = dto.PostReasonId,
                    IsElectronFormed = true,
                },
                ClaimCategories = new List<ClaimCategory>
                {
                    new ClaimCategory
                    {
                        CategoryId = dto.CategoryId
                    }
                },
                CaseDocuments = new List<CaseDocument>
                {
                    new CaseDocument
                    {
                        FileId = dto.FileId,
                        TypeId = dto.TypeId,
                        FileHash = "Lh6nKC0mSMyBt75uvRsCMQ=="
                    }
                },
                CaseParticipants = new List<CaseParticipant>
                {
                    new CaseParticipant
                    {
                        Entity = new Entity
                        {
                            Tin = long.Parse(claimApplication.Application.Contractor.Inn),       //check
                            Pinfl = null, //check
                            NotCitizen = false
                        },
                        Participant = new Participant
                        {
                            Type = dto.ParticipantType,
                            IsMain = true
                        },
                        EntityDetails = new EntityDetails
                        {
                            EntityType = dto.EntityType,
                            IsCurrent = true,
                            Phone = claimApplication.Application.Contractor.PhoneNumber,
                            Address = claimApplication.Application.Contractor.Address,
                            RegionId = dto.RegionId,
                            DistrictId = dto.DistrictId,
                            CountryId = Guid.Parse("e17a4b5d-d1ec-44ae-bfd2-16c143389513"),  //Uzbekistan i
                            Name = claimApplication.Organization.FullName,
                            //email = claimApplication.Organization.Email ,
                            //postcode = "200100",
                            
                            Director = claimApplication.Application.Contractor.Director,
                            OrgType = "LOCAL_ORG",
                            Founders = new List<Founder>
                            {
                                new Founder
                                {
                                    FounderName = claimApplication.Application.Contractor.FullName
                                }
                            }
                        }
                    }
                },
                ClaimAmountsWithParts = new List<ClaimAmountsWithPart>
                {
                    new ClaimAmountsWithPart
                    {
                        ClaimAmount = new ClaimAmount
                        {
                            Amount =  claimApplication.TotalAmount ?? 0,
                            Forfeit = claimApplication.CalculedPenalty ?? 0,
                            CurrencyId = dto.CurrencyId,
                        },
                        ClaimAmountParts = new List<ClaimAmountPart>
                        {
                            new ClaimAmountPart
                            {
                                Amount = amount,
                                AmountType = "DEPT",
                                AmountCategoryId = dto.AmountCategoryId,
                                Details = new  ClaimAmountPartsDetails
                                {
                                    PaymentAccount = dto.PaymentAccount 
                                }
                            }
                        }
                    }
                },
                    Claim = new ClaimDto
                    {
                        ClaimKind = dto.ClaimKind,
                    },

                    RequestId = Guid.NewGuid(),
                    Signature = new Signature
                    {
                        PublicCertificate = dataImzo.Result.Pkcs7Info.Signers.FirstOrDefault().Certificate.FirstOrDefault().PublicKey.PublicKey, //@"MIIIVDCCB/ygAwIBAgIEd/tC8jAPBgsqhlwDDwEBAgICAgUAMIIBQzEpMCcGA1UEAwwgUUFZVU1PViBTSEFSSUZKT04gU09UVk9MRElZRVZJQ0gxGTAXBgNVBAwMENCU0LjRgNC10LrRgtC+0YAxNzA1BgNVBAoMLkRVSyBZQU5HSSBURVhOT0xPR0lZQUxBUiBJTE1JWS1BWEJPUk9UIE1BUktBWkkxQTA/BgNVBAsMONCt0KDQmCDRj9GA0LDRgtC40Ygg0LLQsCDRgNC10LXRgdGC0YDQuNC90Lgg0Y7RgNC40YLQuNGIMVcwVQYDVQQHDE4xMDAwOTYg0KLQvtGI0LrQtdC90YIg0YguINCn0LjQu9C+0L3Qt9C+0YAg0YIuINCc0YPSm9C40LzQuNC5INC60Z7Rhy4gMTY2LdGD0LkxGTAXBgkqhkiG9w0BCQEWCmluZm9AeXQudXoxCzAJBgNVBAYTAlVaMB4XDTIzMDQxNzA3NDQzNVoXDTI1MDQxNzE4NTk1OVowgcoxLDAqBgNVBAMMI0VSR0FTSEVWIERPU1RPTiBCQVhUSVlPUiBP4oCYR+KAmExJMQ8wDQYDVQQpDAZET1NUT04xETAPBgNVBAQMCEVSR0FTSEVWMRUwEwYDVQQHDAxCbydrYSB0dW1hbmkxGjAYBgNVBAgMEVRvc2hrZW50IHZpbG95YXRpMQswCQYDVQQGEwJVWjEZMBcGCgmSJomT8ixkAQEMCTU3MzYwMDAwMDEbMBkGByqGXAMQAQIMDjMyNzA3OTg2NzMwMDEzMGAwGQYJKoZcAw8BAQIBMAwGCiqGXAMPAQECAQEDQwAEQM0kob0JBAMdpr2WwOMzVQ0Pc72/eL/4G/El4QZlVcljX3VNhXj1MfGTCWke5ak7TLp3PjKz5FTVyvGf92X8d6KjggVGMIIFQjCB3AYDVR0jBIHUMIHRgBSQxH+XIpY3FCJjtfzHmUKVeeUSAaGBrqSBqzCBqDELMAkGA1UEBhMCVVoxSTBHBgNVBAsMQEVSSSBrYWxpdGxhcmluaSByb+KAmHl4YXRnYSBvbGlzaCBvcmdhbmkgKE/igJhaRFNUIDEwOTItMjAwOS1JSSkxPDA6BgNVBAoMM0FUIHZhIGtvbW11bmlrYXRzaXlhbGFyaW5pIHJpdm9qbGFudGlyaXNoIHZhemlybGlnaTEQMA4GA1UEAwwHbWl0Yy51eoIIFyy1DqqXoQUwHQYDVR0OBBYEFG0cwn88Curf3NLloOcLN45QYCGJMA4GA1UdDwEB/wQEAwID+DAgBgNVHSUBAf8EFjAUBggrBgEFBQcDAgYIKwYBBQUHAwQwgYsGCCsGAQUFBwEBBH8wfTBTBggrBgEFBQcwAoZHaHR0cDovL2UtaW16by51ei9kaXJlY3RvcnkvY2VydGlmaWNhdGVzLzIwMjEvMDMvMjkvMTcyY2I1MGVhYTk3YTEwNS5jZXIwJgYIKwYBBQUHMAGGGmh0dHA6Ly9lLWltem8udXovY2Ftcy9vY3NwMIIBpQYDVR0fBIIBnDCCAZgwggGUoEOgQYY/aHR0cDovL2UtaW16by51ei9kaXJlY3RvcnkvY3Jscy8yMDIxLzAzLzI5LzE3MmNiNTBlYWE5N2ExMDUuY3JsooIBS6SCAUcwggFDMSkwJwYDVQQDDCBRQVlVTU9WIFNIQVJJRkpPTiBTT1RWT0xESVlFVklDSDEZMBcGA1UEDAwQ0JTQuNGA0LXQutGC0L7RgDE3MDUGA1UECgwuRFVLIFlBTkdJIFRFWE5PTE9HSVlBTEFSIElMTUlZLUFYQk9ST1QgTUFSS0FaSTFBMD8GA1UECww40K3QoNCYINGP0YDQsNGC0LjRiCDQstCwINGA0LXQtdGB0YLRgNC40L3QuCDRjtGA0LjRgtC40YgxVzBVBgNVBAcMTjEwMDA5NiDQotC+0YjQutC10L3RgiDRiC4g0KfQuNC70L7QvdC30L7RgCDRgi4g0JzRg9Kb0LjQvNC40Lkg0LrRntGHLiAxNjYt0YPQuTEZMBcGCSqGSIb3DQEJARYKaW5mb0B5dC51ejELMAkGA1UEBhMCVVowggHXBgNVHSABAf8EggHLMIIBxzCBgQYJKoZcAwICAQIBMHQwJwYIKwYBBQUHAgEWG2h0dHA6Ly9lLWltem8udXovY2EvY3BzLnBkZjBJBggrBgEFBQcCAjA9DDvQktGB0LUg0LLQuNC00Ysg0Y3Qu9C10LrRgtGA0L7QvdC90L7QuSDQvtGC0YfQtdGC0L3QvtGB0YLQuDBpBgkqhlwDAgIBAgMwXDAnBggrBgEFBQcCARYbaHR0cDovL2UtaW16by51ei9jYS9jcHMucGRmMDEGCCsGAQUFBwICMCUMI9Cf0LvQsNGC0LXQttC90YvQtSDQvtC/0LXRgNCw0YbQuNC4MHEGCSqGXAMCAgECAjBkMCcGCCsGAQUFBwIBFhtodHRwOi8vZS1pbXpvLnV6L2NhL2Nwcy5wZGYwOQYIKwYBBQUHAgIwLQwr0K3Qu9C10LrRgtGA0L7QvdC90YvQtSDQtNC10LrQu9Cw0YDQsNGG0LjQuDBjBgkqhlwDAgIBAgQwVjAnBggrBgEFBQcCARYbaHR0cDovL2UtaW16by51ei9jYS9jcHMucGRmMCsGCCsGAQUFBwICMB8MHdCR0LjRgNC20LXQstGL0LUg0YHQtNC10LvQutC4MA8GCyqGXAMPAQECAgICBQADQQAdWnI/FawRATWCCBpHAe+Zs0f8ZY9kddP0puolR3jqwgSbsqQZaHVFBGvx/tsFXcWme0Ceop110HsACA3In78P",
                        Sign = dto.SignedData,
                        SignedHash = "jSqi0ZtdYSdiQzNtWyw64Q=="
                    }
                };

            var resultTwo = await _sudService.SudSendingNewClaim(result, dto);
            return resultTwo;
        }
        catch (Exception ex)
        {
            AddError($"applicationForCourt service {ex.Message} : {ex.InnerException}");
            return null;
        }
    }
    //Квитанция генерация қилиш 
    public async Task<InvoiceResponseModel> SudInvoice(SudInvoiceDto dto)
    {
        try
        {
            var invResult = new InvoiceModel
            {
                court_id = dto.CourtId,
                entity = new InvoiceEntity()
                {
                    tin = dto.tin,
                },
                entity_details = new InvoiceEntityDetails()
                {
                    address = dto.Address,
                    name = dto.Name,
                    org_type = "LOCAL_ORG",
                },
                invoices = new List<Invoice>()
                {
                     new Invoice
                     {
                        amount = dto.Amount,
                        amount_type = "POST",
                     }
                }
            };
            var result = await _sudService.SudInvoice(invResult);
            if (result == null)
            {
                AddError("Квитанция генерация қилиш da error response null");
                return null;
            }

            return result;
        }
        catch (Exception ex)
        {
            AddError($"Квитанция генерация қилиш da error {ex.Message}","Message");
            return null;
        }
    }
    
    #region Get zaproslar
    public async Task<List<CommonEntity>> GetSudAmountCategoryList()
    {
        var result = await _sudService.GetSudAmountCategoryList();
        if(result == null)
        {
            AddError("SudAmountCategories da hatolik yuz berdi ", "Message");
            return null;
        }
        return result;   
    }

    public async Task<List<CommonEntity>> GetSudCategoriesSecondList()
    {
        var result = await _sudService.GetSudCategoriesSecondList();
        if (result == null)
        {
            AddError("CategoriesSecond da hatolik yuz berdi ", "Message");
            return null;
        }
        return result;
    }

    public async Task<List<CommonEntity>> GetSudCategoriesSubList()
    {
        var result = await _sudService.GetSudCategoriesSubList();
        if (result == null)
        {
            AddError("CategoriesSub da hatolik yuz berdi ", "Message");
            return null;
        }
        return result;
    }

    public async Task<List<string>> GetSudClaimKindList()
    {
        var result = await  _sudService.GetSudClaimKindList();
        if (result == null)
        {
            AddError("ClaimKinds da hatolik yuz berdi ", "Message");
            return null;
        }
        return result;
    }

    public async Task<List<CommonEntity>> GetSudCountryList()
    {
        var result = await _sudService.GetSudCountryList();
        if(result == null)
        {
            AddError("Error Countries da hatolik  yuz berdi ", "Message");
            return null;
        }
        return result;
    }

    public async Task<List<CommonEntity>> GetSudCategoryList()
    {
        var result = await  _sudService.GetSudCategoryList();
        if( result == null)
        {
            AddError("Category error", "Message");
            return null;
        }
        return result;
    }

    public async Task<List<CommonEntity>> GetSudCourtList()
    {
        var result = await _sudService.GetSudCourtList();
        if( result == null)
        {
            AddError("Error Courts da hatolik  yuz berdi ", "Message");
            return null ;
        }
        return result;
    }

    public async Task<List<CurrencyModel>> GetSudCurrencyList()
    {
        var result = await _sudService.GetSudCurrencyList();
        if(result == null)
        {
            AddError("Error Currencies da hatolik yuz beri " , "Message");
            return null;
        }
        return result;
    }

    public async Task<List<CommonEntity>> GetSudDocumentTypesList()
    {
        var result = await _sudService.GetSudDocumentTypesList();
        if(result ==null)
        {
            AddError("Error DocumentTypesList da hatolik  yuz berdi ", "Message");
            return null;
        }
        return result;
    }

    public async Task<List<CommonEntity>> GetSudDutyReasonList()
    {
        var result = await _sudService.GetSudDutyReasonList();
        if(result is  null)
        {
            AddError("Error Duty Reasons da hatolik  yuz berdi ", "Message");
            return null;
        }
        return result;
    }
    
    public async Task<List<string>> GetSudEntityTypeList()
    {
        var result = await _sudService.GetSudEntityTypeList();
        if(result is null)
        {
            AddError("EntityTypes da hatolik  yuz berdi ", "Message");
            return null;
        }
        return result;
    }

    public async Task<List<string>> GetSudParticipantTypeList()
    {
        var result = await _sudService.GetSudParticipantTypeList();
        if (result is null)
        {
            AddError("ParticipantTypes da hatolik yuz berdi", "Message");
            return null;
        }
        return result;
    }
    
    public async Task<List<CommonEntity>> GetSudPostReasonList()
    {
        var result = await  _sudService.GetSudPostReasonList();
        if( result is null)
        {
            AddError("Post Reasonsda hatolik  ", "Message");
            return null ;
        }
        return result;
    }

    public async Task<List<CommonEntityRegion>> GetSudRegionList()
    {
        var result = await  _sudService.GetSudRegionList();
        if(result is null)
        {
            AddError("Regions da hatolik  yuz berdi ", "Message");
            return null;
        }
        return result;
    }

    public async Task<List<CommonEntityRegion>> GetSudDistrictList(Guid regionId)
    {

        var result = await _sudService.GetSudDistrictList(regionId);
        if (result is null)
        {
            AddError("Districts da hatolik  yuz berdi ", "Message");
            return null;
        }
        return result;
    }
    
    public async Task<List<BankModel>> GetSudBankList()
    {
        var result = await _sudService.GetSudBankList();
        if(result is null)
        {
            AddError("Banks da hatolik  yuz berdi ", "Message");
            return null;
        }
        return result;
    }

    #endregion

    #endregion

}