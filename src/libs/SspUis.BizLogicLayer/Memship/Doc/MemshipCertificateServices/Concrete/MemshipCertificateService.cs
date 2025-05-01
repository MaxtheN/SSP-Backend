using DocumentFormat.OpenXml;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Spire.Doc;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.Notify;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using SspUis.Integration.DocxToPdf;
using SspUis.Integration.Soliq;
using SspUis.Integration.Stat.Services;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class MemshipCertificateService
    : BaseEntityService<long, MemshipCertificate, MemshipCertificateListDto, MemshipCertificateDto, CreateMemshipCertificateDlDto, UpdateMemshipCertificateDlDto, IMemshipCertificateRepository, MemshipCertificateSortFilterOptions>
    , IMemshipCertificateService
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractorService _contractorService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly INumberService _numberService;
    private readonly SystemConf _systemConf;
    private readonly IConvertService _pdfConverter;
    private readonly IStorageService _storageService;
    private readonly ICultureHelper _cultureHelper;
    private readonly ISendSmsService _sendSmsService;
    private readonly ISoliqContractorService _soliq;
    private readonly IMemshipContractService _memshipContractService;
    private readonly IStatService _statService;
    
    private const int TotalDaysOfYear = 365;

	public MemshipCertificateService(
		IUnitOfWork unitOfWork,
		INumberService numberService,
		IDocumentChangeLogService documentChangeLogService,
		IAuthService authService,
		IStorageService storageService,
		IConvertService pdfConverter,
		ICultureHelper cultureHelper,
		IContractorService contractorService,
		SystemConf systemConf,
		ISendSmsService sendSmsService,
		ISoliqContractorService soliq,
		IMemshipContractService memshipContractService,
		IStatService statService)
		: base(unitOfWork)
	{
		_authService = authService;
		_unitOfWork = unitOfWork;
		_documentChangeLogService = documentChangeLogService;
		_numberService = numberService;
		_storageService = storageService;
		_pdfConverter = pdfConverter;
		_cultureHelper = cultureHelper;
		_contractorService = contractorService;
		this._systemConf = systemConf;
		this._sendSmsService = sendSmsService;
		this._soliq = soliq;
		_memshipContractService = memshipContractService;
		_statService = statService;
	}
	public PagedResult<MemshipCertificateListDto> GetList(MemshipCertificateSortFilterOptions options)
    {
        
        return Repository.ReadAsNoTracked<MemshipCertificateListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }
    public long GetCount()
    {
        return _unitOfWork.Context.Set<MemshipCertificate>()
            .Where(x => x.StatusId == StatusIdConst.FORMED)
            .Count();
    }
	public int GetCountMy()
	{
        MemshipCertificateSortFilterOptions memshipCertificateSortFilterOptions =
            new MemshipCertificateSortFilterOptions();

		return GetList(memshipCertificateSortFilterOptions).Rows.Count();
	}
	public SelectList<long> AsSelectList()
    {
        return Repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public MemshipCertificateDto Get()
    {
        return new MemshipCertificateDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_CERTIFICATE, 1).Item2
        };
    }
    public async ValueTask<MemshipCertificateFromSoliqDto> GetFromSoliq(string inn)
    {
        //try
        //{
        //    var contractor = _unitOfWork.Context.Set<Contractor>().FirstOrDefault(a => a.Inn == inn);

        //    SoliqContractorFinanceBenefitByTinDataDto benefit = null;
        //    decimal netIncome = 0;

        //    if (inn == null)
        //        benefit = null;  // 0 set qilingan pasda
        //    else
        //    {
        //        benefit = await this._soliq.GetFinanceBenefitByInn(inn, DateTime.Now.Year - 1, 4);

        //        var firstKvDate = new DateOnly(year: DateTime.Now.Year, month: 3, day: 15);    // 15-martgacha hisobot topshiradi soliqqa.

        //        if (benefit == null && DateTime.Now.AsDateOnly() < firstKvDate)
        //        {
        //            benefit = await _soliq.GetFinanceBenefitByInn(
        //            inn, DateTime.Now.Year - 2, 4);
        //        }
        //    }
        //    CombineStatuses(_soliq);

        //    if (benefit == null || benefit.NetIncome == null)
        //    {
        //        benefit = new() { NetIncome = 0 };
        //    }
        //    //if (contractor.Inn != "308805613")       //   Bitta odam uchun.
        //    benefit.NetIncome *= 1000;
        //    var category = UnitOfWork.Context.Set<ContractorCategoryCriterion>()
        //         .Include(c => c.ContractorCategory).ThenInclude(c => c.Translates)
        //         .FirstOrDefault(c =>
        //             (c.MinAmount == null || c.MinAmount < benefit.NetIncome) &&
        //             (c.MaxAmount == null || c.MaxAmount > benefit.NetIncome)
        //         );
        //    decimal? benefitNow = null;
        //    if (inn != null)
        //        benefitNow = _contractorService.GetAylanmaByInn(inn);
        //    if (benefitNow != null)
        //    {
        //        var categoryNow = UnitOfWork.Context.Set<ContractorCategoryCriterion>()
        //             .Include(c => c.ContractorCategory).ThenInclude(c => c.Translates)
        //             .FirstOrDefault(c =>
        //                 (c.MinAmount == null || c.MinAmount < benefitNow) &&
        //                 (c.MaxAmount == null || c.MaxAmount > benefitNow)
        //             );
        //        if (categoryNow != null && IsPayed(categoryNow.ContractorCategoryId, contractor.OpfId ?? 0))
        //        {
        //            category = categoryNow;
        //        }
        //    }
        //    if (category == null)
        //    {
        //        AddError($"Category not found benefit.NetIncome: {benefit.NetIncome}");
        //        //return null;
        //    }

        //    var applicationDto = new MemshipCertificateFromSoliqDto
        //    {
        //        Amount = benefit.NetIncome,
        //        Contractor = benefit?.Name ?? "Unknown",
        //        ContractorType = category.ContractorCategory.FullName
        //    };

        //    return applicationDto;
        //}
        //catch (Exception ex)
        //{
        //    AddError(ex.Message + "  Inner: " + ex.InnerException);
        //    return null;
        //}
        try
        {
            var contractor = _unitOfWork.Context.Set<Contractor>().FirstOrDefault(a => a.Inn == inn);

            var staticAmount = 0; // Static amount value
            var staticContractor = contractor.FullName;
            var staticContractorType = "";

            // Creating the static application DTO
            var applicationDto = new MemshipCertificateFromSoliqDto
            {
                Amount = staticAmount,
                Contractor = staticContractor,
                ContractorType = staticContractorType
            };

            return applicationDto;
        }
        catch (Exception ex)
        {
            AddError(ex.Message + "  Inner: " + ex.InnerException);
            return null;
        }
    }
    public MemshipCertificateDto GetByMemshipContractId(long memshipContractId)
    {
        var memshipContract = Repository.Context.Set<MemshipContract>()
            .Include(c => c.Contractor)
            .Include(c => c.MemshipContractType)
            .Include(c => c.Application)
            .ThenInclude(c => c.MemshipApplication)
            .Include(c => c.ContractorSettlementAccount)
            .FirstOrDefault(a => a.Id == memshipContractId);
        if (memshipContract == null)
            AddError("Shartnoma topilmadi.");

        if (memshipContract.Application == null)
            AddError("Ariza topilmadi.");

        if (HasErrors)
            return null;

        var orgByRegion = UnitOfWork.Context.Set<Organization>()
            .FirstOrDefault(org =>
            ContractorCategoryIdConst.IsPayed(memshipContract.ContractorCategoryId ?? 0, memshipContract.Contractor.OpfId ?? 0) ?
            org.Id == OrganizationIdConst.SSP
            : org.RegionId == memshipContract.RegionId
            && (org.OrganizationGroupId == OrganizationGroupIdConst.SSP || org.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

        var districtId = memshipContract.DistrictId;

        var payments = UnitOfWork.MemshipPaymentOrderRepository
            .AllAsQueryable
            .Where(x => x.MemshipContractId == memshipContractId)
            .Sum(x => x.Amount);

        var bhm = UnitOfWork.FixedMinimumValueRepository.AllAsQueryable
            .FirstOrDefault(x => x.MinimumValueTypeId == MinimumValueTypeIdConst.BRV)?.FixedValue ?? 0;

        if (memshipContract == null)
        {
            AddError("Shartnoma topilmadi. :( ");
            return null;
        }
        var contractorSettlementAccounts = _unitOfWork.Context.Set<ContractorSettlementAccount>()
            .Where(a => a.OwnerId == memshipContract.ContractorId);

        var contractorSettlementAccount = contractorSettlementAccounts.FirstOrDefault();

        if (contractorSettlementAccounts.Any(csa => csa.IsMain))
            contractorSettlementAccount = contractorSettlementAccounts.FirstOrDefault(csa => csa.IsMain);
        if (!_systemConf.IsTest)
        {
            if (contractorSettlementAccount == null && memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID)
            {
                AddError("Tadbirkor hisob raqami topilmadi.");
                return null;
            }
        }
        var memshipCertificateDto = new MemshipCertificateDto
        {
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_CERTIFICATE,
            orgByRegion.Id,
            districtId: memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ? 0 : districtId,
            regionId: memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ? 0 : orgByRegion.RegionId).Item2,
            DocOn = DateOnly.FromDateTime(DateTime.Today),
            ExpireOn = memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID
            ? DateOnly.FromDateTime(DateTime.Today)
                .AddDays((int)(payments / (bhm * memshipContract.BaseFixedMinimumValue / 365)))
                : DateOnly.FromDateTime(DateTime.Today).AddYears(1),
            Contractor = memshipContract.Contractor.FullName,
            ContractorId = memshipContract.ContractorId,
            Details = memshipContract.Details,
            MemshipContractId = memshipContractId,
            ContractorSettlementAccount = contractorSettlementAccount?.AccountCode,
            ContractorSettlementAccountId = contractorSettlementAccount?.Id,
        };
        return memshipCertificateDto;
    }
    public override MemshipCertificateDto Get(long id)
    {
        var dto = Repository.ById<MemshipCertificateDto>(id);
        CombineStatuses(Repository);
        if (IsValid)
        {
            var oneMonthFromNow = DateTime.Today.AddMonths(1);

            dto.CanModify = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.MemshipCertificateEdit);
            dto.CanAccept = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.MemshipCertificateAccept);
            dto.CanCancel = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.CANCELED) && _authService.HasPermission(ModuleCode.MemshipCertificateCancel);
            dto.CanDelete = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.MemshipCertificateDelete);
            if (dto.StatusId == StatusIdConst.CANCELED)
                dto.CanProlong = false;
            else
                dto.CanProlong = true;
            //dto.CanProlong = _authService.HasPermission(ModuleCode.MemshipCertificateProlong) &&
            //     (dto.ExpireOn < DateOnly.FromDateTime(DateTime.Today) ||
            //     dto.ExpireOn > DateOnly.FromDateTime(oneMonthFromNow.AddMonths(-1)));
        }

        return dto;
    }
    public dynamic GetByInnPinflForChamber(string innPinfl)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.Now);
        var doc = Repository.Context.Set<MemshipCertificate>()
                                    .Include(x => x.Contractor)
                                    .Where(x => x.StatusId == StatusIdConst.FORMED && x.ExpireOn > dateNow)
                                    .OrderByDescending(x => x.DocOn)
                                    .ThenBy(x=>x.ExpireOn)
                                    .FirstOrDefault(x => x.Contractor.Inn == innPinfl || x.Contractor.Pinfl == innPinfl);

        if (doc == null)
        {
            AddError("Ma'lumot topilmadi!");
            return null;
        }

        if(doc.ExpireOn < dateNow)
        {
            AddError($"Muddati tugagan {doc.ExpireOn}!" , "Message");
            return null;
        }

        return new
        {
            Inn = doc.Contractor.Inn,
            Pinfl = doc.Contractor.Pinfl,
            DocNumber = doc.DocNumber,
            ExpireOn = doc.ExpireOn,
            Contractor = doc.Contractor.FullName
        };
    }
    public async Task<HaveId<long>> Create(CreateMemshipCertificateDlDto dto)
    {
        var contract = UnitOfWork.Context.Set<MemshipContract>()
            .Include(x => x.Contractor)
            .Include(x => x.Signs)
            .Where(x => x.StatusId == StatusIdConst.SIGNED)
            .FirstOrDefault(c => c.Id == dto.MemshipContractId);

        if (contract == null)
        {
            AddError(" Imzolangan shartnoma topilmadi. :( ");
            return null;
        }
        var phones = UnitOfWork.Context.Set<BusinessmanUserInContractor>()
            .Include(x => x.BusinessmanUser)
            .Where(x => x.ContractorId == contract.ContractorId)
            .Select(x => x.BusinessmanUser.UserName).ToList();


        if (contract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID)
        {
            var payments = UnitOfWork.MemshipPaymentOrderRepository
               .AllAsQueryable
               .Where(x => x.MemshipContractId == contract.Id)
               .Where(x => x.StatusId == StatusIdConst.ACCEPTED).ToList()
               /*.Sum(x => x.Amount)*/
               ;

            var additionalAggrements = UnitOfWork.AdditionalAgreementRepository.AllAsQueryable
                .Where(x => x.MemshipContractId == contract.Id && x.StatusId == StatusIdConst.SIGNED)
                .FirstOrDefault()
                ;

            decimal days = 0;
            //var bhm = 0m;
            var realBHM = additionalAggrements?.BaseFixedMinimumValue ?? contract.BaseFixedMinimumValue;
            foreach (var paymentOrder in payments)
            {
                var bhmByPayment = UnitOfWork.FixedMinimumValueRepository
                    .GetBhmByDate(paymentOrder.DocOn, MinimumValueTypeIdConst.BRV)?.FixedValue ?? 0;

                decimal day = CalculateDays(paymentOrder.Amount, bhmByPayment, realBHM);
                days += day;
            }
            //var bhm = UnitOfWork.FixedMinimumValueRepository.AllAsQueryable
            //    .FirstOrDefault(x => x.MinimumValueTypeId == MinimumValueTypeIdConst.BRV)?.FixedValue ?? 0;
            if (additionalAggrements != null)
            {
                if (additionalAggrements.CanPayDivided)
                {
                    //if (contract.Id != 179254)   //   buni obtashlash kerak, vallomatti aytganiga qo'shildi


                    //if (payments < (additionalAggrements.BaseFixedMinimumValue * bhm * (decimal)0.51))
                    //{
                    //    AddError("Joriy to'lov miqdori bir yillik jami to'lovning 51% dan kam.");
                    //    return null;
                    //}
                }
                //else
                //if (days < TotalDaysOfYear)
                //{
                //    AddError("Joriy tolov miqdori bir yillik jami to'lov miqdoridan kam.");
                //    return null;
                //}
            }
            //else
            //if (days < TotalDaysOfYear)
            //{
            //    AddError("Joriy tolov miqdori bir yillik jami to'lov miqdoridan kam.");
            //    return null;
            //}

            dto.ExpireOn = contract.DocOn
                .AddDays((int)days);
        }
        else
        {
            dto.ExpireOn = contract.Signs.LastOrDefault().SignedAt.Value.AddYears(1).AsDateOnly();
        }

        //var cer = UnitOfWork.Context.Set<MemshipCertificate>()
        //                            .Any(x => x.MemshipContractId == contract.Id
        //                             && x.StatusId == StatusIdConst.FORMED && x.ExpireOn >= DateOnly.FromDateTime(DateTime.Today));

        //if (cer)
        //{
        //    AddError("Sertifikat taqdim etilgan.");
        //}

        // Fetch MemshipCertificates into memory
        var certificates = UnitOfWork.Context.Set<MemshipCertificate>()
            .Where(x => x.MemshipContractId == contract.Id && x.StatusId == StatusIdConst.FORMED)
            .ToList();

        // Check if there are any certificates that expire before today
        var anyExpired = certificates.Any(x => x.ExpireOn < DateOnly.FromDateTime(DateTime.Today));

        if (anyExpired)
        {
            AddError("Sertifikat taqdim etilgan.");
        }

        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                MemshipCertificate entity = Repository.Create(dto, ent =>
                {

                });
                UnitOfWork.Save();
                CombineStatuses(Repository);
                if (IsValid)
                    transaction.Commit();

                var data = await _statService.ImportAcquisition(new Integration.Stat.AuthModels.StatRequest()
                {
                    contractorname = entity.Contractor.FullName,
                    docNumber = entity.DocNumber,
                    expireOn = entity.ExpireOn,
                    inn = entity.Contractor.Inn,
                    telNumber = GetContractorPhoneNumber(entity.Contractor)

                });
				if (data.Code == 0)
                {
                    var result = _unitOfWork.Context.Set<MemshipCertificate>().FirstOrDefault(a => a.Id == entity.Id);
                    result.StatId = data.Import_id;
                    _unitOfWork.Save();
                }

                foreach (var phone in phones)
                    await _sendSmsService.SendSms(TableIdConst.MEMSHIP__DOC_MEMSHIP_CERTIFICATE, null, StatusIdConst.FORMED, phone);
                //}
                return HaveId.Create(entity.Id);
            }
            catch (DbUpdateException e)
            {
                AddError(e.InnerException?.Message ?? e.Message);
                transaction.Rollback();
            }
            return null;
        }
    }
	private string GetContractorPhoneNumber(Contractor contractor)
	{
        var data = contractor.Applications.FirstOrDefault().MemshipApplication.ContractorWorkPhoneNumber
             ?? contractor.Applications.FirstOrDefault().MemshipApplication.ContractorMobilePhoneNumber
             ?? contractor.Applications.FirstOrDefault().MemshipApplication.ContractorAdditionalPhoneNumber;
        return data;
	}
	private static decimal CalculateDays(decimal payments, decimal bhm, decimal realBHM)
    {
        return (payments / (bhm * realBHM / TotalDaysOfYear));
    }
    public override void Update(UpdateMemshipCertificateDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                CombineStatuses(Repository);
                if (IsValid)
                    transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }
    public void Accept(UpdateStatusMemshipCertificateDto dTo)
    {
        var dto = new UpdateStatusMemshipCertificateDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                Repository.AddError("Нет доступа");

            ent.IsRead = false;
        });
    }
    public byte[] DownloadPdf(string innPinfl, string? lang)
    {
        var doc = Repository.Context.Set<MemshipCertificate>().Include(x => x.Contractor)
            .FirstOrDefault(x => (x.Contractor.Inn == innPinfl || x.Contractor.Pinfl == innPinfl)
                && x.ExpireOn >= DateTime.Now.AsDateOnly());
        var language = lang ?? "uz-latn";
        if (doc == null)
        {
            //AddError("Guvohnoma topilmadi.");
            //return null;
           
            var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
              language,
                StaticFileConst.WordTemplate.MEMSHIP_CERTIFICATE_ERROR)
            );

            var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
            return res;
        }
        return DownloadPdf(doc.Id2, language);
    }
    public byte[] DownloadPdf(Guid id2, string? lang)
    {
        var language = lang ?? "uz-latn";
        var certificate = UnitOfWork.Context.Set<MemshipCertificate>()
            .Include(s => s.Contractor)
                .Include(s => s.Region)
                .ThenInclude(a => a.Translates)
            .FirstOrDefault(c => c.Id2 == id2);

        if (certificate is null)
        {
            AddError("Бундай аъзолик сертификат мавжуд емас !");
            return null;
        }

        if (certificate.StatusId == StatusIdConst.CANCELED)
        {
            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                  language,
                    StaticFileConst.WordTemplate.MEMSHIP_CERTIFICATE_Cancel)
                );

            var lan = UnitOfWork.Context.Set<Language>()
                .FirstOrDefault(l => l.Code == language)?.Id ?? 1;

            string link = GenerateLink(certificate.Id2);
            var qrCode = QRCodeHelper.GeneratePng(link, 300, 300);
            var qrImage = new MemoryStream(qrCode);
            var plh = new Placeholders();

            //plh.TextPlaceholders.Add(nameof(certificate.CancelDay), certificate.CancelDay?.ToString("dd.MM.yyyy"));
            certificate.CancelReasen = string.IsNullOrEmpty(certificate.CancelReasen) ? certificate.Message : certificate.CancelReasen;
            plh.TextPlaceholders.Add(nameof(certificate.CancelReasen), certificate.CancelReasen);

            plh.TextPlaceholders.Add(
                nameof(certificate.Region),
                certificate?.Region?.Translates?.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(
                                    TranslateColumn.full_name,
                                    ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? "");
            plh.TextPlaceholders.Add(nameof(certificate.DocNumber), certificate.DocNumber);
            plh.TextPlaceholders.Add(nameof(certificate.DocOn), certificate.ExpireOn.ToString("dd.MM.yyyy"));
            plh.TextPlaceholders.Add(nameof(certificate.Contractor.FullName), certificate.Contractor.FullName ?? "");
            plh.TextPlaceholders.Add(nameof(certificate.Details), certificate.Details);
            plh.TextPlaceholders.Add("ActivityType", certificate.Contractor.Pinfl != null ? certificate.Contractor.Director + " - " + certificate.Contractor.Pinfl : "");
            plh.TextPlaceholders.Add(nameof(certificate.Contractor.Inn), certificate.Contractor.Inn);

            wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
            wordFile.Position = 0;

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
        else
        {
            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                   language,
                    StaticFileConst.WordTemplate.MEMSHIP_CERTIFICATE)
                );

            var lan = UnitOfWork.Context.Set<Language>()
                .FirstOrDefault(l => l.Code == language)?.Id ?? 1;

            string link = GenerateLink(certificate.Id2);
            var qrCode = QRCodeHelper.GeneratePng(link, 300, 300);
            var qrImage = new MemoryStream(qrCode);
            var plh = new Placeholders();

            plh.ImagePlaceholders.Add("QrCode", new ImageElement
            {
                Dpi = 300,
                MemStream = qrImage,
            });

            plh.TextPlaceholders.Add(
                nameof(certificate.Region),
                certificate?.Region?.Translates?.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(
                                    TranslateColumn.full_name,
                                    ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? "");
            plh.TextPlaceholders.Add(nameof(certificate.DocNumber), certificate.DocNumber);
            plh.TextPlaceholders.Add(nameof(certificate.DocOn), certificate.ExpireOn.ToString("dd.MM.yyyy"));
            plh.TextPlaceholders.Add(nameof(certificate.Contractor.FullName), certificate.Contractor.FullName ?? "");
            plh.TextPlaceholders.Add(nameof(certificate.Details), certificate.Details);
            plh.TextPlaceholders.Add("ActivityType", certificate.Contractor.Pinfl != null ? certificate.Contractor.Director + " - " + certificate.Contractor.Pinfl : "");
            plh.TextPlaceholders.Add(nameof(certificate.Contractor.Inn), certificate.Contractor.Inn);

            wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
            wordFile.Position = 0;

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
    }
    //public byte[] DownloadPdf(Guid id2, string? lang)
    //{
    //    var language = lang ?? "uz-latn";
    //    var certificate = UnitOfWork.Context.Set<MemshipCertificate>()
    //        .Include(s => s.Contractor)
    //        .Include(s => s.Region)
    //        .ThenInclude(a => a.Translates)
    //        .FirstOrDefault(c => c.Id2 == id2);

    //    if (certificate is null)
    //    {
    //        AddError("Bunday a’zolik sertifikati mavjud emas!");
    //        return null;
    //    }

    //    string statusUrl = $"https://my-api.chamber.uz/MemshipCertificate/Status?id2={id2}";
    //    var qrCode = QRCodeHelper.GeneratePng(statusUrl, 300, 300);
    //    var qrImage = new MemoryStream(qrCode);

    //    var plh = new Placeholders();
    //    plh.ImagePlaceholders.Add("QrCode", new ImageElement
    //    {
    //        Dpi = 300,
    //        MemStream = qrImage,
    //    });

    //    plh.TextPlaceholders.Add(nameof(certificate.Region),
    //        certificate?.Region?.Translates?.AsQueryable()
    //            .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,
    //                ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? "");
    //    plh.TextPlaceholders.Add(nameof(certificate.DocNumber), certificate.DocNumber);
    //    plh.TextPlaceholders.Add(nameof(certificate.DocOn), certificate.ExpireOn.ToString("dd.MM.yyyy"));
    //    plh.TextPlaceholders.Add(nameof(certificate.Contractor.FullName), certificate.Contractor.FullName ?? "");
    //    plh.TextPlaceholders.Add(nameof(certificate.Details), certificate.Details);
    //    plh.TextPlaceholders.Add("ActivityType", certificate.Contractor.Pinfl != null ? certificate.Contractor.Director + " - " + certificate.Contractor.Pinfl : "");
    //    plh.TextPlaceholders.Add(nameof(certificate.Contractor.Inn), certificate.Contractor.Inn);

    //    var wordFile = _storageService.GetStaticFile(
    //        StaticFileConst.WordTemplate.GetFileName(language, StaticFileConst.WordTemplate.MEMSHIP_CERTIFICATE));

    //    wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
    //    wordFile.Position = 0;

    //    var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
    //    if (res == null)
    //    {
    //        Document document = new Document();
    //        document.LoadFromStream(wordFile, FileFormat.Docx);
    //        var pdfStream = new MemoryStream();
    //        document.SaveToStream(pdfStream, FileFormat.PDF);
    //        return pdfStream.ToArray();
    //    }

    //    CombineStatuses(_pdfConverter);
    //    return res;
    //}
    private string GenerateLink(Guid id2)
    {
        var link = _systemConf.QrImagePrintPath;
        //if (_authService.Contractor == null)
        //link = link + "/Memship";
        link = link + "/MemshipCertificate/DownloadPdf?id2=" + id2 + "&lang=uz-cyrl";

        return link;
    }
    public byte[] DownloadPdf(MemshipCertificateForPdf model)
    {
        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
                _cultureHelper.CurrentCulture.Code,
                StaticFileConst.WordTemplate.MEMSHIP_CERTIFICATE)
            );

        var contractor = UnitOfWork.Context
            .Set<Contractor>()
            .Include(c => c.Region)
                .ThenInclude(r => r.Translates)
            .FirstOrDefault(c => c.Id == model.ContractorId);

        if (contractor is null)
        {
            AddError("Бундай тадбиркорлик субекти мавжуд емас !");
            return null;
        }

        model.Region = contractor.Region.Translates.AsQueryable()
            .FirstOrDefault(RegionTranslate.GetExpr(
                        DataLayer.TranslateColumn.full_name,
                                ServiceProvider.CultureHelper.CurrentCulture.Id)
                            ).TranslateText;

        model.Inn = contractor.Inn;
        model.OrgName = contractor.FullName;

        var memshipContractor = UnitOfWork.Context
            .Set<MemshipContract>()
            .Include(c => c.Application)
                .ThenInclude(a => a.MemshipApplication)
                    .ThenInclude(m => m.ContractorActivityType)
            .FirstOrDefault(mc => mc.Id == model.MemshipContractId);

        if (memshipContractor is null)
        {
            AddError("Бундай аъзолик шартномаси мавжуд емас !");
            return null;
        }

        var link = "";
        var qrCode = QRCodeHelper.GeneratePng(link, 300, 300);
        var qrImage = new MemoryStream(qrCode);
        var plh = new Placeholders();
        if (memshipContractor.StatusId == StatusIdConst.CANCELED)
        {
            string rejectTextAndDate = @$"{memshipContractor.RejectMessage}
                                              {memshipContractor.RejectDate?.ToString("MM-dd-yyyy")}";

            plh.TextPlaceholders.Add("QrCode", rejectTextAndDate);
        }
        else
        {
            plh.ImagePlaceholders.Add("QrCode", new ImageElement
            {
                Dpi = 300,
                MemStream = qrImage,
            });
        }
        plh.TextPlaceholders.Add(
            nameof(model.Region), model.Region ?? "");
        plh.TextPlaceholders.Add(nameof(model.DocNumber), model.DocNumber);
        //var ExpireOn= DateOnly.FromDateTime(DateTime.Today).AddYears(memshipContract.MemshipContractType.CertificatePeriodInYears)
        plh.TextPlaceholders.Add("DocOn", DateTime.Now.AddYears(1).ToString("dd.MM.yyyy"));
        plh.TextPlaceholders.Add(nameof(contractor.FullName), contractor.FullName ?? "");
        plh.TextPlaceholders.Add(nameof(memshipContractor.Details), memshipContractor.Details);
        plh.TextPlaceholders.Add("ActivityType", "");
        plh.TextPlaceholders.Add(nameof(contractor.Inn), contractor.Inn);
        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        var res = _pdfConverter.DocxToPdfAsync(wordFile, model).Result;
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
    public async ValueTask Cancel(CancelStatusMemshipCertificateDto dto)
    {
        var certificate = _unitOfWork.Context.Set<MemshipCertificate>()
            .Include(a => a.MemshipContract)
                .ThenInclude(b => b.Application)
            .Include(a => a.Files)
            .FirstOrDefault(a => a.Id == dto.Id);

        if (certificate != null)
        {
            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = canCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;

            try
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                var memshipCertificate = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanMemshipCertificateApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    ent.IsRead = false;
                });

                CombineStatuses(Repository);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }

                _unitOfWork.Save();

                if (dto.IsRejectContractAndApplication)
                {
                   await _memshipContractService.Cancel(new CancelStatusMemshipContractDto { Id = memshipCertificate.MemshipContractId });
                    CombineStatuses(_memshipContractService);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return;
                    }
                }

                certificate.ExpireOn = DateOnly.FromDateTime(DateTime.Today);
                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message, _authService.User.FullName);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }

                certificate.Files.AddFromTempFiles(
                    DocumentStorageConst.DOC_MEMSHIP_CERTIFICATE_FILES,
                    dto.Files.Select(x => x.Id).ToList());

                _storageService.MoveToPersistent(DocumentStorageConst.DOC_MEMSHIP_CERTIFICATE_FILES, dto.Id.ToString(), dto.Files.Select(a => a.Id).ToArray());
                CombineStatuses(_storageService);

                _unitOfWork.Save();

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                AddError("An error occurred while processing your request.");
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        else
        {
            AddError("Entity not found");
            return;
        }
    }
    public void ProlongExpireOn(MemshipCertificateProlongDto dTo)
    {
        var entity = _unitOfWork.Context.Set<MemshipCertificate>().Include(a => a.MemshipContract).FirstOrDefault(a => a.Id == dTo.Id);

        #region MemshipContract docnumber uchun kere boldi avtomat generate qilish kere boldi
        var memshipContract = Repository.Context.Set<MemshipContract>()
            .Include(c => c.Contractor)
            .Include(c => c.MemshipContractType)
            .Include(c => c.Application)
            .ThenInclude(c => c.MemshipApplication)
            .Include(c => c.ContractorSettlementAccount)
            .FirstOrDefault(a => a.Id == entity.MemshipContractId);

        var orgByRegion = UnitOfWork.Context.Set<Organization>()
           .FirstOrDefault(org =>
           ContractorCategoryIdConst.IsPayed(memshipContract.ContractorCategoryId ?? 0, memshipContract.Contractor.OpfId ?? 0) ?
           org.Id == OrganizationIdConst.SSP
           : org.RegionId == memshipContract.RegionId
           && (org.OrganizationGroupId == OrganizationGroupIdConst.SSP || org.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

        var districtId = memshipContract.DistrictId;
        #endregion

        if (entity == null)
        {
            AddError("Ma'lumot topilmadi");
        }

        if (entity.MemshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.FREE || entity.MemshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID)
        {
            var NewDocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_CERTIFICATE,
            orgByRegion.Id,
            districtId: memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ? 0 : districtId,
            regionId: memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ? 0 : orgByRegion.RegionId).Item2;

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, $"Sertifikat amal qilish muddatini cho'zish va hujjar raqamini ozgartirish eski - {entity.ExpireOn.ToString("dd.MM.yyyy")} yangi - {dTo.NewExpireOn.ToString("dd.MM.yyyy")} - eski - {entity.DocNumber} - yangi - {NewDocNumber}", _authService.User.FullName);

            entity.DocNumber = NewDocNumber;
            entity.Details = dTo.Details;
            entity.ExpireOn = dTo.NewExpireOn;

            _unitOfWork.Save();
        }
        else
        {
            AddError("Bu guvohnoma Tekin va Pullik emas");
        }
    }
    public void ProlongAosExpireOn()
    {
        var memeshipCertificates = _unitOfWork.Context.Set<MemshipCertificate>()
            .Include(a => a.MemshipContract)
            .Where(x => x.StatusId == StatusIdConst.FORMED 
            && x.MemshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.FREE);

        if (memeshipCertificates == null)
        {
            AddError("Malumot topilmadi");
            return;
        }

        int previousYear = DateTime.Now.Year - 1;

        var taxAosAylanmalar = _unitOfWork.Context.Set<AosAylanma>().Where(x => x.Year == previousYear && x.NetIncome < 100000000000).ToList();

        if (taxAosAylanmalar != null)
        {
            foreach (var taxAosAylanma in taxAosAylanmalar)
            {
                var contractor = memeshipCertificates.FirstOrDefault(x => x.Contractor.Inn == taxAosAylanma.Inn);
                if (contractor != null &&
                    contractor.ExpireOn < DateOnly.FromDateTime(DateTime.Now).AddMonths(1)
                    && contractor.ExpireOn > DateOnly.FromDateTime(DateTime.Now)
                    || contractor != null && contractor.ExpireOn < DateOnly.FromDateTime(DateTime.Now))
                {
                    var memshipContract = Repository.Context.Set<MemshipContract>()
                                                            .Include(c => c.Contractor)
                                                            .Include(c => c.MemshipContractType)
                                                            .Include(c => c.Application)
                                                            .ThenInclude(c => c.MemshipApplication)
                                                            .Include(c => c.ContractorSettlementAccount)
                                                            .FirstOrDefault(a => a.Id == contractor.MemshipContractId);

                    var orgByRegion = UnitOfWork.Context.Set<Organization>()
                                                        .FirstOrDefault(org =>
                                                        ContractorCategoryIdConst.IsPayed(memshipContract.ContractorCategoryId ?? 0, memshipContract.Contractor.OpfId ?? 0) ?
                                                        org.Id == OrganizationIdConst.SSP : org.RegionId == memshipContract.RegionId && 
                                                        (org.OrganizationGroupId == OrganizationGroupIdConst.SSP || org.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

                    var districtId = memshipContract.DistrictId;

                    var NewDocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_CERTIFICATE,
                        orgByRegion.Id,
                        districtId: memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ? 0 : districtId,
                        regionId: memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ? 0 : orgByRegion.RegionId).Item2;

                    contractor.ExpireOn = contractor.ExpireOn.AddYears(1);
                    contractor.DocNumber = NewDocNumber;
                }
            }

            if (IsValid)
            {
                UnitOfWork.Save();
                return;
            }
        }
        else
            return;
    }
    public void ProlongQqsExpireOn()
    {
        var memeshipCertificates = _unitOfWork.Context.Set<MemshipCertificate>()
            .Include(a => a.MemshipContract)
            .Where(x => x.StatusId == StatusIdConst.FORMED 
            && x.MemshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.FREE);

        if (memeshipCertificates == null)
        {
            AddError("Malumot topilmadi");
            return;
        }

        int currentYear = DateTime.Now.Year;
        int previousYear = currentYear - 1;

        var taxQqsAylanmalar = _unitOfWork.Context.Set<QqsAylanma>()
                                                  .Where(x => x.Year == currentYear || x.Year == previousYear)
                                                  .GroupBy(x => new { x.Inn, x.Year })
                                                  .Select(g => new
                                                  {
                                                      Inn = g.Key.Inn,
                                                      Year = g.Key.Year,
                                                      TotalNetIncomeWithoutVat = g.Sum(x => x.NetIncomeWithoutVat)
                                                  })
                                                  .ToList();

        var groupedTaxQqsAylanmalar = taxQqsAylanmalar.GroupBy(x => x.Inn)
                                                      .Select(g => new
                                                      {
                                                          Inn = g.Key,
                                                          IncomePreviousYear = g.FirstOrDefault(x => x.Year == previousYear)?.TotalNetIncomeWithoutVat ?? 0,
                                                          IncomeCurrentYear = g.FirstOrDefault(x => x.Year == currentYear)?.TotalNetIncomeWithoutVat ?? 0
                                                      })
                                                      .ToList();

        int limitCount = 0;
        if (groupedTaxQqsAylanmalar != null)
        {
            foreach (var taxQqsAylanma in groupedTaxQqsAylanmalar)
            {
                var contractor = memeshipCertificates.FirstOrDefault(x => x.Contractor.Inn == taxQqsAylanma.Inn);
                if ((contractor != null &&
                    contractor.ExpireOn < DateOnly.FromDateTime(DateTime.Now).AddMonths(1) 
                    && contractor.ExpireOn > DateOnly.FromDateTime(DateTime.Now))
                    || (contractor != null && contractor.ExpireOn < DateOnly.FromDateTime(DateTime.Now)))
                {
                    if (taxQqsAylanma.IncomePreviousYear < 100000000000 && taxQqsAylanma.IncomeCurrentYear < 100000000000)
                    {
                        if (limitCount <= 5)
                        {
                            var memshipContract = Repository.Context.Set<MemshipContract>()
                                                                .Include(c => c.Contractor)
                                                                .Include(c => c.MemshipContractType)
                                                                .Include(c => c.Application)
                                                                .ThenInclude(c => c.MemshipApplication)
                                                                .Include(c => c.ContractorSettlementAccount)
                                                                .FirstOrDefault(a => a.Id == contractor.MemshipContractId);

                            var orgByRegion = UnitOfWork.Context.Set<Organization>()
                                                                .FirstOrDefault(org =>
                                                                ContractorCategoryIdConst.IsPayed(memshipContract.ContractorCategoryId ?? 0, memshipContract.Contractor.OpfId ?? 0) ?
                                                                org.Id == OrganizationIdConst.SSP : org.RegionId == memshipContract.RegionId &&
                                                                (org.OrganizationGroupId == OrganizationGroupIdConst.SSP || org.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

                            var districtId = memshipContract.DistrictId;

                            var NewDocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_CERTIFICATE,
                                orgByRegion.Id,
                                districtId: memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ? 0 : districtId,
                                regionId: memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ? 0 : orgByRegion.RegionId).Item2;

                            contractor.ExpireOn = contractor.ExpireOn.AddYears(1);
                            contractor.DocNumber = NewDocNumber;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            if (IsValid)
            {
                UnitOfWork.Save();
                return;
            }
        }
        else
            return;
    }
    public void ProlongExpireOnForPaid(long id, long paymentOrderId, string message)
    {
        var entity = _unitOfWork.Context.Set<MemshipCertificate>()
            .Include(a => a.MemshipContract)
            .ThenInclude(a => a.AdditionalAgreements)
            .Include(a => a.MemshipContract.PaymentOrders)
            .FirstOrDefault(a => a.Id == id);

        if (entity == null)
        {
            AddError("Malumot topilmadi");
            return;
        }

        if (entity.MemshipContract == null
            //|| entity?.MemshipContract?.AdditionalAgreements == null
            //|| (!entity.MemshipContract.AdditionalAgreements.Any())
            )
        {
            AddError("Нет доступа");
            return;
        }
        var additionalAgrement = entity.MemshipContract?.AdditionalAgreements?.FirstOrDefault(x => x.StatusId == StatusIdConst.SIGNED);
        //if (additionalAgrement == null)
        //{
        //    AddError("Нет доступа/(additionalAgrement is null)");
        //    return;
        //}
        var paymentOrder = entity.MemshipContract?.PaymentOrders?.FirstOrDefault(x => x.Id == paymentOrderId);
        if (paymentOrder == null)
        {
            AddError("Нет доступа/(paymentOrder is null)");
            return;
        }
        var realBHM = (additionalAgrement?.BaseFixedMinimumValue ?? 0) > 0
            ? additionalAgrement.BaseFixedMinimumValue
            : entity.MemshipContract.BaseFixedMinimumValue;
        var bhm = UnitOfWork.FixedMinimumValueRepository
                .GetBhmByDate(paymentOrder.DocOn, MinimumValueTypeIdConst.BRV)?.FixedValue ?? 0;

        //dto.ExpireOn = contract.DocOn
        //    .AddDays((int)(payments / (bhm * realBHM / 365)));


        //AddError($"paymentOrder.Amount {paymentOrder.Amount}");
        //AddError($"additionalAgrement {bhm}");
        //AddError($"additionalAgrement Amount {realBHM}");
        //return;
        //int days = (int)(paymentOrder.Amount / (additionalAgrement.Amount / 365));
        int days = (int)(paymentOrder.Amount / (bhm * realBHM / 365));
        entity.ExpireOn = entity.ExpireOn.AddDays(days);
        message = message + $" (Days:{days})";
        var logRes = CreateDocumentChangeLog(entity.Id, entity.StatusId, message: message);
        if (IsValid)
            UnitOfWork.Save();
    }
	public async Task SendToStat()
	{
		DateOnly today = DateOnly.FromDateTime(DateTime.Now);
		var result = _unitOfWork.Context.Set<MemshipCertificate>().
            Include(i=>i.Contractor).Where(q=>q.DocOn > new DateOnly(2023,10,1) && q.ExpireOn >= today && q.StatId == null).AsQueryable();

        foreach (var item in result)
        {
            if(item.StatId is null)
            {
				var data =  await _statService.ImportAcquisition(new Integration.Stat.AuthModels.StatRequest()
				{
                    contractorname = item.Contractor.FullName,
                    expireOn = item.ExpireOn,
                    inn = item.Contractor.Inn,
                    telNumber = GetContractorPhoneNumber(item.Contractor),
                    docNumber = item.DocNumber
				});

				if (data.Code == 0)
				{
					item.StatId = data.Import_id;
				}
			}
           
        }

		_unitOfWork.Save();
	}
	public override void Delete(long id)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusMemshipCertificateDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                    
                };
                Repository.UpdateStatus(statusDto);
                UnitOfWork.Save();

                if (IsValid)
                {
                    transaction.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusMemshipCertificateDlDto dto, Action<MemshipCertificate> validation)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.UpdateStatus(dto);
                CombineStatuses(Repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, null, dto.Message);
                if (IsValid)
                    transaction.Commit();
                return res;
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
        return null;
    }
    private HaveId<long> CreateDocumentChangeLog(long id, int? statusId, string message = null, string userIp = null, string userAgent = null)
    {
        var entityDto = Repository.ById<MemshipCertificateDto>(id, applyFilter: false);
        int finalStatusId = statusId ?? entityDto.StatusId;
        _documentChangeLogService.Create(
            dto: entityDto,
            tableId: TableIdConst.MEMSHIP__DOC_MEMSHIP_CERTIFICATE,
            organizationId: null,
            statusId: finalStatusId,
            message: message,
            userIp: userIp,
            userAgent: userAgent);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private void Validation<TDto>(MemshipCertificateDlDto<TDto> dto, MemshipCertificate entity)
          where TDto : MemshipCertificateDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        if (query.ByDocNumber(dto.DocNumber).Any())
            Repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));
    }

    #region FOR IMPORT VAQTINCHALIKKA
    public async Task<string> ImportYuridik(List<ImportYurMemshipCertificateDlDto> listDto)
    {
        listDto = listDto.Where(a => a.Inn != 0).ToList();

        var contractorIds = _unitOfWork.Context.Set<Contractor>()
            .Where(a => listDto.Select(b => b.Inn.ToString()).Contains(a.Inn))
            .IsActive()
            .AsEnumerable()
            .ToDictionary(
                a => a.Inn,
                a => a.Id
            );
        int insertCount = 0;
        StringBuilder errors = new();
        foreach (var dto in listDto)
        {
            try
            {
                if (!contractorIds.ContainsKey(dto.Inn.ToString()))
                {
                    errors.AppendLine(dto.Inn.ToString() + " info_contractor mavjud emas v1");
                    continue;
                }

                foreach (var con in UnitOfWork.Context.Set<MemshipContract>()
                    .Where(x => x.StatusId == StatusIdConst.SIGNED && x.ContractorId == contractorIds[dto.Inn.ToString()]).ToList())
                {
                    if (con is null)
                    { errors.AppendLine($"{dto.Inn} бунақа memship_contract мавжуд емас ! v1"); continue; }

                    if (UnitOfWork.Context.Set<MemshipCertificate>().Any(mc => mc.MemshipContractId == con.Id))
                    {
                        errors.AppendLine($"{dto.Inn} bu memship_certificate ni {con.Id} Id li bu memship_contract mavjud (unique) v1");
                        continue;
                    }

                    if (dto.DocOn.IsNullOrEmpty())
                    {
                        dto.DocOn = con.DocOn.ToString();
                        dto.DocDay = con.DocOn.Day.ToString();
                        dto.DocMonth = con.DocOn.Month.ToString();
                        dto.DocYear = con.DocOn.Year.ToString();
                    }

                    if (dto.ExpireOn.IsNullOrEmpty())
                    {
                        dto.ExpireOn = con.DocOn.ToString();
                        dto.ExpireDay = "01";
                        dto.ExpireMonth = "01";
                        dto.ExpireYear = DateTime.Now.AddYears(1).Year.ToString();
                    }

                    var entity = Repository.Create(new CreateMemshipCertificateDlDto
                    {
                        DocOn = new DateOnly(int.Parse(dto.DocYear), int.Parse(dto.DocMonth), int.Parse(dto.DocDay)),
                        ExpireOn = new DateOnly(int.Parse(dto.ExpireYear), int.Parse(dto.ExpireMonth), int.Parse(dto.ExpireDay)),
                        DocNumber = dto.DocNumber,
                        MemshipContractId = con.Id,
                        ContractorSettlementAccountId = con.ContractorSettlementAccountId ?? null,
                        ContractorId = contractorIds[dto.Inn.ToString()],
                        Message = string.Empty
                    });

                    UnitOfWork.Save();
                    insertCount++;

                    CombineStatuses(Repository);
                    if (HasErrors)
                        errors.AppendLine($"{dto.Inn} // XATO REPOSITORYDA");
                }
            }
            catch (Exception exception)
            {
                errors.AppendLine($"{dto.Inn} // CATCH DA");
            }
        }

        return errors.ToString() + $"\n\n{insertCount}";
    }
    public async Task<string> ImportJismoniy(List<ImportJISMemshipCertificateDlDto> listDto)
    {
        int insertCount = 0;
        listDto = listDto.Where(a => a.Pinfl != 0).ToList();

        var contractorIds = _unitOfWork.Context.Set<Contractor>()
            .Where(a => listDto.Select(b => b.Pinfl.ToString()).Contains(a.Pinfl))
            .IsActive()
            .AsEnumerable()
            .ToDictionary(
                a => a.Pinfl,
                a => a.Id
            );

        StringBuilder errors = new();
        foreach (var dto in listDto)
        {
            try
            {
                if (!contractorIds.ContainsKey(dto.Pinfl.ToString()))
                {
                    errors.AppendLine(dto.Pinfl.ToString() + " info_contractor mavjud emas");
                    continue;
                }

                var memshipContract = UnitOfWork.Context
                    .Set<MemshipContract>()
                    .FirstOrDefault(mc =>
                        mc.ContractorId == contractorIds[dto.Pinfl.ToString()] && mc.StatusId == StatusIdConst.SIGNED);

                if (memshipContract is null)
                {
                    errors.AppendLine($"{dto.Pinfl} bunday memship_contract yuq");
                    continue;
                }

                if (UnitOfWork.Context.Set<MemshipCertificate>().Any(mc => mc.MemshipContractId == memshipContract.Id))
                {
                    errors.AppendLine($"{dto.Pinfl} bu memship_certificate ni {memshipContract.Id} Id li bu memship_contract mavjud (unique)");
                    continue;
                }

                if (dto.DocOn == string.Empty)
                {
                    dto.DocDay = memshipContract.DocOn.Day.ToString();
                    dto.DocMonth = memshipContract.DocOn.Month.ToString();
                    dto.DocYear = memshipContract.DocOn.Year.ToString();
                }
                else if (dto.ExpireOn == string.Empty)
                {
                    dto.ExpireDay = dto.DocDay;
                    dto.ExpireMonth = dto.DocMonth;
                    dto.ExpireYear = (int.Parse(dto.DocYear) + 1).ToString();
                }

                var newMemshipCertificates = new CreateMemshipCertificateDlDto
                {
                    DocOn = new DateOnly(int.Parse(dto.DocYear), int.Parse(dto.DocMonth), int.Parse(dto.DocDay)),
                    ExpireOn = new DateOnly(int.Parse(dto.ExpireYear), int.Parse(dto.ExpireMonth), int.Parse(dto.ExpireDay)),
                    DocNumber = dto.DocNumber,
                    MemshipContractId = memshipContract.Id,
                    ContractorSettlementAccountId = memshipContract.ContractorSettlementAccountId ?? null,
                    ContractorId = contractorIds[dto.Pinfl.ToString()],
                    Message = string.Empty
                };

                var entity = Repository.Create(newMemshipCertificates);
                entity.StatusId = StatusIdConst.FORMED;
                UnitOfWork.Save();

                CombineStatuses(Repository);
                if (HasErrors)
                    errors.AppendLine($"{dto.Pinfl} // XATO REPOSITORYDA");

                insertCount++;
            }
            catch (Exception exception)
            {
                errors.AppendLine($"{dto.Pinfl} // {exception.Message} // {exception.InnerException}");
            }
        }
        return $"{errors}\n\n\n{insertCount}";
    }
    #endregion
    private static bool IsPayed(int contractorCategoryId, int opfId)
    {
        return (contractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA
                    || opfId == OpfIdConst.UYUSHMA);
    }
    public Stream SaveAsExcel(MemshipCertificateSortFilterOptions options)
    {
        var data = Repository.ReadAsNoTracked<MemshipCertificateListDto>()
            .SortFilter(options).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.MEMSHIP_CERTIFICATE));

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
                ws.Cells[currentRow, column++].Value = item.DocNumber;
                ws.Cells[currentRow, column++].Value = item.DocOn.ToString(Constants.DATE_FORMAT);
                ws.Cells[currentRow, column++].Value = item.ExpireOn.ToString(Constants.DATE_FORMAT);
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.ContractorInn;
                ws.Cells[currentRow, column++].Value = item.Contractor;
                ws.Cells[currentRow, column++].Value = item.ContractorOked;
                ws.Cells[currentRow, column++].Value = item.PhoneNumber;
                ws.Cells[currentRow, column++].Value = item.Director;
                ws.Cells[currentRow, column++].Value = item.Status;
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public void Update(UpdatingBirdMemshipCertificateDlDto dto)
    {

        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(dto.Id);
                if (entity == null)
                    AddError("Not Found");

                entity.Details = dto.Detail;
                UnitOfWork.Save();
                CombineStatuses(Repository);
                if (IsValid)
                    transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }

    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context
                .Set<MemshipCertificateFile>()
                .FirstOrDefault(a => a.Id == fileId);

        return Download(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_CERTIFICATE_FILES);
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
            if (file == null)
                AddError("По вашему запросу запись не найдено");

            CombineStatuses(_storageService);

            if (IsValid)
                file.FileName = entity.FileName;
        }

        return file;
    }
    public object p(StorageFile[] files)
    {
        if (files == null || files.Length <= 0)
        {
            AddError("Empty file");
            return null;
        }

        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_MEMSHIP_CERTIFICATE_FILES, files)
            .Select(x => new MemshipCertificateFileDto
            {
                Id = x.FileId,
                FileName = x.FileName,
                CreatedAt = DateTime.Now
            });

        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public MemshipCertificateExpiredPaymentsDto MemshipCertificateExpiredPayment()
    {
        if(_authService.User.IsAdmin || _authService.User.OrganizationId == 1)
        {
            var options = new MemshipCertificateSortFilterOptions
            {
                ByExpireOn = true,
                MemshipContractTypeId = 1,
                ContractorCategoryId = 4,
                OrderType = "asc",
                IsOld = null
            };

            var memshipList = Repository.ReadAsNoTracked<MemshipCertificateListDto>()
                                        .SortFilter(options)
                                        .AsPagedResult(options);

            var result = new MemshipCertificateExpiredPaymentsDto();
        
            string baseUrl = _systemConf.IsTest ? "http://sspuis.apptest.uz" : "https://erp.chamber.uz";
            string expiredMessage = "Muddati tugagan pullik a'zolik guvohnomalari mavjud, ko'rish uchun: ";
            string expiringMessage = "Muddati tugayotgan pullik a'zolik guvohnomalari mavjud, ko'rish uchun: ";
            string notExpiredMessage = "Muddati tugagan pullik a'zolik guvohnomalari mavjud emas!";
            string notExpiringMessage = "Muddati tugayotgan pullik a'zolik guvohnomalari mavjud emas!";

            void AddDeadline(bool isExpired, int expiredCount, string message, string url)
            {
                result.Deadlines.Add(new MemshipCertificateExpiredPaymentDto
                {
                    IsExpired = isExpired,
                    ExpiredCount = expiredCount,
                    Message = message,
                    Url = url
                });
            }

            if (memshipList != null && memshipList.Total > 0)
            {
                AddDeadline(true, (int)memshipList.Total, expiredMessage, $"{baseUrl}/document/memshipcertificate?memshipContractTypeId=1&tab=2&contractorCategoryId=4");
            }
            else if (memshipList.Total == 0)
            {
                AddDeadline(false, 0, notExpiredMessage, null);
            }

            if (!options.Less1MonthLeft.HasValue)
            {
                options.Less1MonthLeft = DateOnly.FromDateTime(DateTime.Today);
                options.ByExpireOn = false;
                var memshipListDeadline = Repository.ReadAsNoTracked<MemshipCertificateListDto>()
                                                    .SortFilter(options)
                                                    .AsPagedResult(options);

                if (memshipListDeadline != null && memshipListDeadline.Total > 0)
                {
                    AddDeadline(true, (int)memshipListDeadline.Total, expiringMessage, $"{baseUrl}/document/memshipcertificate?memshipContractTypeId=1&tab=0&contractorCategoryId=4");
                }
                else if (memshipListDeadline.Total == 0)
                {
                    AddDeadline(false, 0, notExpiringMessage, null);
                }
            }

            return result;
        }
        else
        {
            return new MemshipCertificateExpiredPaymentsDto
            {
                Deadlines = null
            };
        }
    }
    public MemshipCertificateToPaidNotificationsDto MemshipCertificateToPaidNotification()
    {
        if (_authService.User.IsAdmin || _authService.User.OrganizationId == 1)
        {
            var memeshipCertificates = _unitOfWork.Context.Set<MemshipCertificate>()
                .Include(a => a.MemshipContract)
                .Include(a => a.Contractor)
                .Where(x => x.StatusId == StatusIdConst.FORMED && x.MemshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.FREE);

            if (memeshipCertificates == null)
            {
                AddError("Ma'lumot topilmadi!");
                return null;
            }

            int currentYear = DateTime.Now.Year;
            int previousYear = currentYear - 1;

            var taxQqsAylanmalar = _unitOfWork.Context.Set<QqsAylanma>()
                                                      .Where(x => x.Year == currentYear || x.Year == previousYear)
                                                      .GroupBy(x => new { x.Inn, x.Year })
                                                      .Select(g => new
                                                      {
                                                          Inn = g.Key.Inn,
                                                          Year = g.Key.Year,
                                                          TotalNetIncomeWithoutVat = g.Sum(x => x.NetIncomeWithoutVat)
                                                      })
                                                      .ToList();
        
            var groupedTaxQqsAylanmalar = taxQqsAylanmalar.GroupBy(x => x.Inn)
                                                          .Select(g => new
                                                          {
                                                              Inn = g.Key,
                                                              IncomePreviousYear = g.FirstOrDefault(x => x.Year == previousYear)?.TotalNetIncomeWithoutVat ?? 0,
                                                              IncomeCurrentYear = g.FirstOrDefault(x => x.Year == currentYear)?.TotalNetIncomeWithoutVat ?? 0
                                                          })
                                                          .Where(x => x.IncomePreviousYear < 100000000000 && x.IncomeCurrentYear >= 100000000000)
                                                          .ToList();

            var notificationsDto = new MemshipCertificateToPaidNotificationsDto();

            foreach (var taxQqsAylanma in groupedTaxQqsAylanmalar)
            {
                var matchingCertificates = memeshipCertificates
                    .Where(x => x.Contractor.Inn == taxQqsAylanma.Inn)
                    .ToList();

                foreach (var certificate in matchingCertificates)
                {
                    var notificationDto = new MemshipCertificateToPaidNotificationDto
                    {
                        Id = certificate.Id,
                        IsNotification = true,
                        DocNumber = certificate.DocNumber,
                        Contractor = certificate.Contractor.FullName,
                        ContractorInn = certificate.Contractor.Inn
                    };

                    notificationsDto.Notifications.Add(notificationDto);
                }
            }

            return notificationsDto;
        }
        else
        {
            return new MemshipCertificateToPaidNotificationsDto
            {
                Notifications = null,
            };
        }
    }

    public object UploadFiles(StorageFile[] files)
    {
        throw new NotImplementedException();
    }
}