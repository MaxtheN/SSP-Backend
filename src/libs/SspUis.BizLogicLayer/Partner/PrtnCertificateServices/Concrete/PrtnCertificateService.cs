using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.OrganizationServices;
using SspUis.BizLogicLayer.PrtnContractServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Certificate;
using SspUis.Integration.Certificate.Services;
using SspUis.Integration.IntegrationCertificate;
using SspUis.Integration.IntegrationCertificateMB;
using SspUis.Integration.IntegrationCertificateXalqBank;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public class PrtnCertificateService
        : BaseEntityService<long, PrtnCertificate, PrtnCertificateListDto, PrtnCertificateDto, CreatePrtnCertificateDlDto, UpdatePrtnCertificateDlDto, IPrtnCertificateRepository, PrtnDocumentSortFilterOptions>
        , IPrtnCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IPrtnContractService _prtnContractService;
        private readonly IStorageService _storageService;
        private readonly IEImzoService _eImzoService;
        private readonly IPrtnCertificateSignRepository _prtnCertificateSignRepository;
        private readonly ICultureHelper _cultureHelper;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly INumberService _numberService;
        private readonly SystemConf _systemConf;
        private readonly IOrganizationService _organizationService;
        private readonly IBaseReportService _baseReportService;
        private readonly IIntegrationCertificateService _integrationCertificateService;
        private readonly IIntegrationCertificateMBService _integrationCertificateMBService;
        private readonly IIntegrationCertificateXalqBankService _integrationCertificateXalqBankService;
        private readonly IIntegrationCertificateBojxonaService _integrationCertificateBojxonaService;
        private readonly IApiRequestLogRepository _apiRequestLogRepository;
        private readonly IIntegrationCertificateMoliyaService _integrationMoliyaCertificate;

        public PrtnCertificateService(IUnitOfWork unitOfWork,
            IAuthService authService,
            IPrtnContractService prtnContractService,
            IDocumentChangeLogService documentChangeLogService,
            IPrtnCertificateSignRepository prtnCertificateSignRepository,
            IStorageService storageService,
            IEImzoService eImzoService,
            ICultureHelper cultureHelper,
            INumberService numberService,
            SystemConf systemConf,
            IOrganizationService organizationService,
            IBaseReportService baseReportService,
            IIntegrationCertificateService integrationCertificateService,
            IApiRequestLogRepository apiRequestLogRepository,
            IIntegrationCertificateMBService integrationCertificateMBService,
            IIntegrationCertificateXalqBankService integrationCertificateXalqBankService,
            IIntegrationCertificateBojxonaService integrationCertificateBojxonaService,
            IIntegrationCertificateMoliyaService integrationMoliyaCertificate)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _prtnContractService = prtnContractService;
            _cultureHelper = cultureHelper;
            _eImzoService = eImzoService;
            _prtnCertificateSignRepository = prtnCertificateSignRepository;
            _storageService = storageService;
            _documentChangeLogService = documentChangeLogService;
            _numberService = numberService;
            _systemConf = systemConf;
            _organizationService = organizationService;
            _baseReportService = baseReportService;
            _integrationCertificateService = integrationCertificateService;
            _apiRequestLogRepository = apiRequestLogRepository;
            _integrationCertificateMBService = integrationCertificateMBService;
            _integrationCertificateXalqBankService = integrationCertificateXalqBankService;
            _integrationCertificateBojxonaService = integrationCertificateBojxonaService;
            _integrationMoliyaCertificate = integrationMoliyaCertificate;
        }
        protected override IQueryable<PrtnCertificateListDto> SortFilter(IQueryable<PrtnCertificateListDto> query, PrtnDocumentSortFilterOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options);
        }

        public SelectList<long> AsSelectList()
        {
            return Repository.ReadAsNoTracked<PrtnCertificateListDto>().AsSelectList();
        }

        public override PrtnCertificateDto Get(long id)
        {
            var dto = Repository.ById<PrtnCertificateDto>(id);
            CombineStatuses(Repository);
            if (IsValid)
            {
                dto.CanCancel = new int[] { StatusIdConst.FORMED, StatusIdConst.ACCEPTED }.Contains(dto.StatusId) && _authService.HasPermission(ModuleCode.PrtnCertificateCancel);
            }
            return dto;
        }

        public PrtnCertificateDto GetByPrtnContractId(long prtnContractId)
        {
            var prtnContract = _prtnContractService.Get(prtnContractId);

            var certificatePeriodInYears = _unitOfWork.PrtnContractTypeRepository.AllAsQueryable
                    .IsActive()
                    .FirstOrDefault(a => a.Id == prtnContract.PrtnContractTypeId)?.CertificatePeriodInYears ?? 0;

            var dto = base.Get();
            dto.Id2 = Guid.NewGuid();
            dto.DocOn = DateTime.Now.AsDateOnly();
            dto.DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_PRTN_CONTRACT, 1).Item2;
            dto.ContractorId = prtnContract.ContractorId;
            dto.Contractor = prtnContract.Contractor;
            dto.ContractorInn = prtnContract.ContractorInn;
            dto.PrtnContractDocNumber = prtnContract.DocNumber;
            dto.PrtnContractDocOn = prtnContract.DocOn;
            dto.PrtnContractId = prtnContract.Id;
            dto.PrtnContractTypeId = prtnContract.PrtnContractTypeId;
            dto.PrtnContractType = prtnContract.PrtnContractType;
            dto.ExpireOn = prtnContract.DocOn.AddYears(certificatePeriodInYears);
            dto.OrganizationId = prtnContract.OrganizationId;


            var orgQuery = _unitOfWork.OrganizationRepository.AllAsQueryable
                .IsActive();

            //if (application.PrtnContractTypeId == PrtnContractTypeIdConst._50_100)
            //{
            //    orgQuery = orgQuery.Where(a => a.DistrictId == (application.ChooseLocation ? application.ChoosedDistrictId : application.DistrictId)
            //        && a.RegionId == (application.ChooseLocation ? application.ChoosedRegionId : application.RegionId)
            //        && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.DISTRICT
            //    );
            //}
            //else if (application.PrtnContractTypeId == PrtnContractTypeIdConst._101_200)
            //{
            //    orgQuery = orgQuery.Where(a => a.RegionId == (application.ChooseLocation ? application.ChoosedRegionId : application.RegionId)
            //        && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION
            //    );
            //}
            //else if (application.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
            //{
            //    orgQuery = orgQuery.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY);
            //}

            //var organizationId = orgQuery.FirstOrDefault()?.Id;
            //if (!organizationId.HasValue)
            //{
            //    AddError($"{application.PrtnContractType} uchun tashkilot topilmadi / Организация для {application.PrtnContractType} не найдено");
            //    return null;
            //}
            //dto.OrganizationId = organizationId.Value;

            return dto;
            // var dto = Repository.AllAsQueryable
            //    .FirstOrDefault(a => a.PrtnContractId == prtnContractId);

            //if (dto == null)
            //{

            //    var prtnContract = _prtnContractService.Get(prtnContractId);
            //    CombineStatuses(_prtnContractService);
            //    if (HasErrors)
            //        return null;

            //    var certificatePeriodInYears = _unitOfWork.PrtnContractTypeRepository.AllAsQueryable
            //        .IsActive()
            //        .FirstOrDefault(a => a.Id == prtnContract.PrtnContractTypeId)?.CertificatePeriodInYears ?? 0;

            //    var newDto = new PrtnCertificateDto
            //    {
            //        Id2 = Guid.NewGuid(),
            //        DocOn = DateTime.Now.AsDateOnly(),
            //        DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_PRTN_CONTRACT, 1).Item2,
            //        ContractorId = prtnContract.ContractorId,
            //        Contractor = prtnContract.Contractor,
            //        ContractorInn = prtnContract.ContractorInn,
            //        PrtnContractDocNumber = prtnContract.DocNumber,
            //        PrtnContractDocOn = prtnContract.DocOn,
            //        PrtnContractId = prtnContract.Id,
            //        PrtnContractTypeId = prtnContract.PrtnContractTypeId,
            //        PrtnContractType = prtnContract.PrtnContractType,
            //        ExpireOn = prtnContract.DocOn.AddYears(certificatePeriodInYears),
            //        OrganizationId = prtnContract.OrganizationId
            //    };
            //    return newDto;
            //    //var mc = new MapperConfiguration(cfg =>
            //    //{
            //    //    cfg.CreateMap<PrtnCertificateDto, CreatePrtnCertificateDlDto>();
            //    //});
            //    //var createDlDto = mc.CreateMapper().Map<CreatePrtnCertificateDlDto>(newDto);

            //    //var certificateId = Create(createDlDto);
            //    //if (HasErrors)
            //    //    return null;
            //    //return Get(certificateId.Id);
            //}

            //return Get(dto.Id);
        }
        public async Task Sign(SignStatusPrtnCertificateDto dto)
        {
            var doc = Get(dto.Id);

            if (!doc.CurrentPrtnCertificateSignId.HasValue)
            {
                AddError("Hujjat imzolangan / Документ уже подписан");
                return;
            }

            var timeStampResult = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Pinfl = _authService.User.Pinfl,
            });

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            if (_authService.Contractor != null)
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStampResult.Pkcs7b64,
                    Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                    Pinfl = _authService.Contractor.Pinfl
                });
            }
            else
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStampResult.Pkcs7b64,
                    Inn = null,
                    Pinfl = _authService.User.Pinfl
                });
            }

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            dto.PrtnCertificateSignId = doc.CurrentPrtnCertificateSignId.Value;
            dto.SignFile = SaveFile(doc.Id, timeStampResult.Pkcs7b64, "sign.txt");
            dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            using (var transaction = _unitOfWork.BeginTransaction())
            {

                Repository.AllAsQueryable.Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanCertificateApplyStatus(ent.StatusId, StatusIdConst.SIGNED
                        )
                    )
                        AddError("Имкони йўқ / Нет доступа");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                var signHistory = _prtnCertificateSignRepository.CreateSignHistory(new PrtnCertificatesignDlDto
                {
                    SignFile = dto.SignFile,
                    DataFile = dto.DataFile,
                    SignedUserInfo = _authService.User.ToTextForDocumentLog(),
                    OwnerId = dto.Id,
                    StatusId = dto.StatusId,
                    SignedAt = DateTime.Now,
                });
                if (IsValid)
                {
                    transaction.Commit();
                }
                if (IsValid)
                    transaction.Commit();
            }
        }
        public IntegrationCertificateRequestDto GetCertificateInfo(int? lang, Guid? Id2, string? contractorInn)
        {
            if (Id2 == null && contractorInn == null)
            {
                AddError("Sertifikatni olish uchun yetarli parametr kiritilmagan!");
                return null;
            }

            var dto = _unitOfWork.Context.Set<PrtnCertificate>()
                .Where(a => a.StatusId != StatusIdConst.CANCELED)
                .Select(a => new IntegrationCertificateRequestDto
                {
                    Guid = a.Id2,
                    NewVacanciesCount = a.PrtnContract.NewVacanciesCount,
                    CertificateLink = _systemConf.QrImagePrintPath + "/PrtnCertificate/PrintCertificatePdf?Id2=" + a.Id2 + "&lang=uz-cyrl",
                    CertificateNumber = a.DocNumber,
                    CancelOn = a.CancelOn,
                    ExpireOn = a.ExpireOn,
                    DocOn = a.DocOn,
                    ContractInfo = new ContractInfo
                    {
                        ContractLink = _systemConf.QrImagePrintPath + "/PrtnContract/PrintPrtnContractPdf?Id2=" + a.PrtnContract.Id2 + "&lang=uz-cyrl",
                        ContractTypeId = a.PrtnContract.PrtnContractTypeId,
                        ContractType = a.PrtnContract.PrtnContractType.Translates.AsQueryable().FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, lang ?? 1)).TranslateText ?? a.PrtnContract.PrtnContractType.FullName,
                        //ContractType = a.PrtnContract.PrtnContractType.FullName,
                        ContractId = a.PrtnContractId,
                        ContractDocOn = a.PrtnContractDocOn,
                        ContractNumber = a.PrtnContractDocNumber
                    },
                    ContractorInfo = new ContractorInfo
                    {
                        Inn = a.ContractorInn,
                        Name = a.Contractor.FullName,
                        Director = a.Contractor.Director,
                        RegionId = a.Contractor.RegionId,
                        Region = a.Contractor.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, lang ?? 1)).TranslateText ?? a.Contractor.Region.FullName,
                        DistrictId = a.Contractor.DistrictId,
                        District = a.Contractor.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, lang ?? 1)).TranslateText ?? a.Contractor.District.FullName,
                        BankName = a.Contractor.Bank.Translates.AsQueryable().FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, lang ?? 1)).TranslateText ?? a.Contractor.Bank.BankName,
                        BankCode = a.Contractor.Bank.Code,
                        Oked = a.Contractor.Oked.Code,
                        OkedName = a.Contractor.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, lang ?? 1)).TranslateText ?? a.Contractor.Oked.FullName,
                        Address = a.Contractor.Address
                    },
                    ContractGraphs = a.PrtnContract.Application.PrtnApplication.Graphs.Select(a => new ContractGraph
                    {
                        YearIn = a.YearIn,
                        MonthIn = a.MonthIn,
                        NewVacanciesCount = a.NewVacanciesCount
                    }).ToList()
                }).FirstOrDefault(a => a.Guid == Id2 || a.ContractorInfo.Inn == contractorInn);

            if (dto == null)
            {
                AddError("Berilgan parametrlar bo''yicha ma'lumot topilmadi!");
                return null;
            }


            return dto;
        }

        public override HaveId<long> Create(CreatePrtnCertificateDlDto dto)
        {
            SetDefaultProps(dto);
            if (HasErrors)
                return null;
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var ent = Repository.Create(dto, ent => Validation(dto));
                CombineStatuses(Repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.CREATED);
                if (IsValid)
                {
                    transaction.Commit();
                }

                return res;
            }
        }

        private void SetDefaultProps<T>(PrtnCertificateDlDto<T> dto)
            where T : PrtnCertificateDlDto<T>
        {
            var prtnContract = _prtnContractService.Get(dto.PrtnContractId);
            CombineStatuses(_prtnContractService);
            if (HasErrors)
                return;

            var certificatePeriodInYears = _unitOfWork.PrtnContractTypeRepository.AllAsQueryable
                .IsActive()
                .FirstOrDefault(a => a.Id == prtnContract.PrtnContractTypeId)?.CertificatePeriodInYears ?? 0;

            dto.ContractorId = prtnContract.ContractorId;
            dto.PrtnContractDocNumber = prtnContract.DocNumber;
            dto.PrtnContractDocOn = prtnContract.DocOn;
            dto.PrtnContractId = prtnContract.Id;
            dto.PrtnContractTypeId = prtnContract.PrtnContractTypeId;
            dto.ExpireOn = prtnContract.DocOn.AddYears(certificatePeriodInYears);
            dto.ContractorInn = prtnContract.ContractorInn;
            dto.OrganizationId = prtnContract.OrganizationId;
        }

        public override void Update(UpdatePrtnCertificateDlDto dto)
        {
            SetDefaultProps(dto);
            if (HasErrors)
                return;
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                var ent = Repository.Update(dto, ent => Validation(dto, ent));
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.MODIFIED);
                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }

        public override void Delete(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(id);
                var dto = new UpdateStatusPrtnCertificateDlDto { Id = id, StatusId = StatusIdConst.DELETED };

                var ent = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanDeleteStatuses.Contains(ent.StatusId))
                        AddError("O'chirish mumkin emas / Невозможно удалить");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.DELETED);
                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }

        public async Task Cancel(CancelStatusPrtnCertificateDto dto)
        {
            var doc = Get(dto.Id);

            var timeStampResult = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Pinfl = _authService.User.Pinfl,
            });

            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStampResult.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.User.Pinfl
            });

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            //dto.PrtnCertificateSignId = doc.CurrentPrtnCertificateSignId.Value;
            dto.SignFile = SaveFile(doc.Id, timeStampResult.Pkcs7b64, "sign.txt");
            dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);
                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanCertificateApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    ent.CancelOn = DateOnly.FromDateTime(DateTime.Now);
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();
                if (dto.CancelContract)
                {
                    _prtnContractService.Cancel(new()
                    {
                        Id = doc.PrtnContractId,
                        Message = "Contract automatic canceled from certificcate",
                        StatusId = StatusIdConst.CANCELED,
                        CancelApplication = dto.CancelApplication
                    });

                    CombineStatuses(_prtnContractService);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return;
                    }

                    _unitOfWork.Save();
                }
                var signHistory = _prtnCertificateSignRepository.CreateSignHistory(new PrtnCertificatesignDlDto
                {
                    SignFile = dto.SignFile,
                    DataFile = dto.DataFile,
                    SignedUserInfo = _authService.User.ToTextForDocumentLog(),
                    OwnerId = dto.Id,
                    StatusId = dto.StatusId,
                    SignedAt = DateTime.Now,
                });

                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                if (IsValid)
                    transaction.Commit();
            }
        }

        public IQueryable<PrtnCertificateListDto> GetPrtnCertificateListDto(PrtnDocumentSortFilterOptions dto)
        {
            var data = Repository.ReadAsNoTracked<PrtnCertificateListDto>()
                           .SortFilter(dto)
                           .Filter(dto.Filters);
            return data;
        }
        public Stream SaveAsExecel(PrtnDocumentSortFilterOptions dto)
        {
            //var data = GetList(dto);
            var data = Repository.ReadAsNoTracked<PrtnCertificateListDto>()
                           .SortFilter(dto)
                           .Filter(dto.Filters)
                           .Take(1000000)
                           .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRNTCERTIFICATE_LIST));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var namerange = excelPackage.Workbook.Names["Organization"];
                namerange.Value = "";

                var ws = namerange.Worksheet;

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
                    ws.Cells[currentRow, column++].Value = item.ContractorRegion;
                    ws.Cells[currentRow, column++].Value = item.ContractorDistrict;
                    ws.Cells[currentRow, column++].Value = item.PrtnContractType;
                    ws.Cells[currentRow, column++].Value = item.ContractorInn;
                    ws.Cells[currentRow, column++].Value = item.Contractor;
                    ws.Cells[currentRow, column++].Value = item.ExpireOn;
                    ws.Cells[currentRow, column++].Value = item.NewVacanciesCount;
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

        public string GetHtmlTemplate(PrtnCertificateDto dto)
        {
            var contractDto = _unitOfWork.PrtnContractRepository.ById(dto.PrtnContractId);
            var applicationDto = _unitOfWork.ApplicationRepository.ById(contractDto.ApplicationId);

            var organizationNameByLocation
                = _organizationService.GetOrganizationNameByLocation(
                            applicationDto.RegionId,
                            applicationDto.DistrictId,
                            dto.PrtnContractTypeId);

            string fileName = "certificate.html";

            Dictionary<string, string> data
                = new Dictionary<string, string>()
                {
                    { "${DocNumber}", dto.DocNumber },
                    { "${DocDate}", dto.DocOn.ToString(Constants.DATE_FORMAT)},
                    { "${OrganizationNameByLocation}", organizationNameByLocation },
                    { "${ContractorName}", dto.Contractor},
                    { "${ContractDate}", contractDto.DocOn.ToString(Constants.DATE_FORMAT) },
                    { "${ContractNumber}", contractDto.DocNumber },
                    { "${ContractorInn}", dto.ContractorInn },
                    { "${ContractExpirationDate}", dto.ExpireOn.ToString(Constants.DATE_FORMAT)},
                    { "${ContractTypeName}", dto.PrtnContractType},
                    { "${prtnApplicationNewVacanciesCount}", contractDto.NewVacanciesCount.ToString() },
                    { "${QrImage}", WEBASE.QRCode.QRCodeHelper.GenerateImageAsBase64($"{_systemConf.QrImagePrintPath}/api/PrtnCertificate/PrintCertificatePdf?Id2=&__lang=uz-cyrl") }
                };

            var fileString = _baseReportService.ReturnReadyHtmlAsString(filename: fileName,
                                                      data: data,
                                                      tableData: null);

            return fileString;
        }

        public byte[] GetPdfTemplate(PrtnCertificateDto dto)
        {
            return _baseReportService.ReturnReadyPdf(GetHtmlTemplate(dto));
        }

        public byte[] GetPdfTemplateByPrtnContractId(long prtnContractId)
        {
            var dto = GetByPrtnContractId(prtnContractId);
            return _baseReportService.ReturnReadyPdf(GetHtmlTemplate(dto));
        }

        public byte[] GetPdfById2(Guid id2)
        {
            var dto = Repository.ReadAsNoTracked<PrtnCertificateDto>(false).FirstOrDefault(a => a.Id2 == id2);
            return _baseReportService.ReturnReadyPdf(GetHtmlTemplate(dto));
        }

        public byte[] GetPdfById(long id)
        {
            var dto = Repository.ById<PrtnCertificateDto>(id, false);
            return _baseReportService.ReturnReadyPdf(GetHtmlTemplate(dto));
        }

        private void Validation<TDto>(PrtnCertificateDlDto<TDto> dto, PrtnCertificate entity = null)
            where TDto : PrtnCertificateDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanEditStatuses.Contains(entity.StatusId))
                    AddError("Tahrirlash mumkin emas / Невозможно редактировать");
            }
        }
        private Guid SaveFile(long docId, string data, string fileName)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;
            var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_PRTN_CERTIFICATE)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

            return fileInfo.FirstOrDefault().FileId;
        }
        private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            int? orgId = null;
            if (_authService.Organization?.Id != null)
                orgId = _authService.Organization.Id;

            var moveDto = Repository.ById<PrtnCertificateDto>(id, applyFilter: false);

            _documentChangeLogService.Create(
                dto: moveDto,
                tableId: TableIdConst.DOC_PRTN_CERTIFICATE,
                organizationId: orgId,
                statusId: statusId,
                message: message);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }

        public async Task SentForOtherServiceFormedCertificate(long id, Enum TypePost)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Repository.AllAsQueryable.Lock(id);
                    var entity = Repository.ById(id);

                    //var statusId = StatusIdConst.FORMED;

                    try
                    {
                        if (!_systemConf.IsTest)
                        {
                            var requestDto = GetCertificateInfo(null, entity.Id2, null);
                            var responseResult = await PostCertificate(requestDto, TypePost, entity.Id);
                            
                            if (IsValid)
                            {
                                entity.SuccessPostCount += 1;
                                //Zulfiddin aka "Tashkilotlarga ketmay qolsa ham ketsa ham statusi o'zgarmasin" dedi
                                //entity.PrevStatusId = entity.StatusId;
                                //entity.StatusId = statusId;
                                CombineStatuses(Repository);
                                if(IsValid)
                                    _unitOfWork.Save();
                                    transaction.Commit();
                            }
                        }
                        else
                            transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //entity.StatusId = StatusIdConst.SENT;
                        entity.Message = $"{ex.Source}: {ex.Message}. {ex.InnerException}";
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message}: {ex.InnerException}");
                }
            }
        }

        private async Task<IntegrationCertificateResponseDto> PostCertificate(IntegrationCertificateRequestDto dto, Enum TypePost, long? id = null)
        {
            var log = new CreateApiRequestLogDlDto
            {
                DocumentId = id,
                TableId = TableIdConst.DOC_PRTN_CERTIFICATE,
                UserId = (int)_authService.UserId,
                UserInfo = _authService.User.ToString() ?? "",
                ResponseAt = DateTime.Now
            };

            try
            {
                var result = new IntegrationCertificateResponseDto { };
                switch (TypePost)
                {
                    case EnumTypeOrganization.Soliq:
                        result = await _integrationCertificateService.PostCertificate(dto);
                        CombineStatuses(_integrationCertificateService);
                        break;
                    case EnumTypeOrganization.MarkaziyBank:
                        result = await _integrationCertificateMBService.PostCertificateMB (dto);
                        CombineStatuses(_integrationCertificateMBService);
                        break;
                    case EnumTypeOrganization.Bojxona:
                        result = await _integrationCertificateBojxonaService.PostCertificateBojxona(dto);
                        CombineStatuses(_integrationCertificateBojxonaService);
                        break;
                    case EnumTypeOrganization.XalqBank:
                        result = await _integrationCertificateXalqBankService.PostCertificateXalqBank(dto);
                        CombineStatuses(_integrationCertificateXalqBankService);
                        break;
                     case EnumTypeOrganization.Moliya:
                        result = await _integrationMoliyaCertificate.PostCertificateMoliya(dto);
                        CombineStatuses(_integrationCertificateXalqBankService);
                        break;
                    default:
                        break;
                }

                log.IsSuccess = IsValid;
                if (result != null)
                {
                    log.RequestContent = result?.Message;
                    log.ResponseStatus = result?.Status;
                    log.RequestUrl = result.Path;
                }

                if (IsValid)
                {
                    log.ResponseContent = JsonConvert.SerializeObject(result);
                    return result;
                }
                else
                    log.Exception = _integrationCertificateService.GetAllErrors();
            }
            catch (Exception ex)
            {
                log.Exception = $"{ex.Message}: {ex.InnerException}";
            }
            finally
            {
                _apiRequestLogRepository.Create(log);
                _unitOfWork.Save();
            }
            return null;
        }

        public List<long> PostAllOldCertificates(int id)
        {
            var certificates = new List<long>();
            certificates = _unitOfWork.Context.Set<PrtnCertificate>()
                //.Where(a => new int[] { StatusIdConst.FORMED }.Contains(a.StatusId))
                //.Where(a => a.Id >= start && a.Id < end)
                .Where(a=> a.Id == id)
                .Select(a => a.Id)
                .ToList();

            return certificates;
        }

		public int GetCount()
		{
			PrtnDocumentSortFilterOptions prtnDocumentSortFilterOptions = 
                new PrtnDocumentSortFilterOptions();

			return GetList(prtnDocumentSortFilterOptions).Rows.Count();
		}
		public Stream PrinGraphExcel(PrtnDocumentSortFilterOptions dto)
        {
            //var data = GetList(dto);
            var data = Repository.ReadAsNoTracked<PrtnCertificateListDto>()
                           .SortFilter(dto)
                           .Filter(dto.Filters)
                           .Take(1000000)
                           .ToList();


            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRINT_GRAPH_EXCEL_LIST));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var namerange = excelPackage.Workbook.Names["Organization"];
                namerange.Value = "";

                var ws = namerange.Worksheet;

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row + 1;
                int index = 1;
                foreach (var item in data)
                {
                    var graphResult = new List<GrapthYear>();

                    var applicationGraph = _unitOfWork.Context.Set<PrtnApplicationGraph>()
                    .Where(a => a.OwnerId == item.PrtnApplicationId)
                    .ToList();

                    var applicationGraphYears = applicationGraph.Select(a => a.YearIn).OrderBy(a => a).Distinct().ToList();

                    for (int i = applicationGraphYears.Min(); i <= applicationGraphYears.Max(); i++)
                    {

                        var graphResultItem = new GrapthYear()
                        {
                            YearIn = i,
                            TotalForYear = applicationGraph.Where(a => a.YearIn == i).Sum(a => a.NewVacanciesCount),
                            Months = new List<GrapthMonth>()
                        };

                        for (int j = 1; j <= 12; j++)
                        {
                            graphResultItem.Months.Add(new GrapthMonth
                            {
                                MonthIn = j,
                                TotalForMonth = applicationGraph.Where(a => a.MonthIn == j).Sum(a => a.NewVacanciesCount),
                                NewVacanciesCount = applicationGraph.Where(a => a.YearIn == i && a.MonthIn == j).Sum(a => a.NewVacanciesCount)
                            });
                        }
                        graphResult.Add(graphResultItem);
                    }
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    ws.Cells[currentRow, column++].Value = item.ContractorRegion;
                    ws.Cells[currentRow, column++].Value = item.ContractorDistrict;
                    ws.Cells[currentRow, column++].Value = item.PrtnContractType;
                    ws.Cells[currentRow, column++].Value = item.ContractorInn;
                    ws.Cells[currentRow, column++].Value = item.Contractor;
                    ws.Cells[currentRow, column++].Value = item.OkedCode + " - " + item.Oked;
                    ws.Cells[currentRow, column++].Value = item.DocNumber;
                    ws.Cells[currentRow, column++].Value = item.DocOn.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.ExpireOn.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.CancelOn?.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.Status;
                    ws.Cells[currentRow, column++].Value = graphResult.Sum(a => a.TotalForYear);
                    foreach (var item2 in graphResult)
                    {
                        int col = column;
                        if (item2.YearIn == 2023)
                        {
                            ws.Cells[currentRow, col++].Value = item2.TotalForYear;
                            var itemMonths = item2.Months;
                            foreach (var item3 in itemMonths)
                            {
                                ws.Cells[currentRow, col++].Value = item3.NewVacanciesCount;
                            }
                        }
                        if (item2.YearIn == 2024)
                        {
                            col += 13;
                            ws.Cells[currentRow, col++].Value = item2.TotalForYear;
                            var itemMonths = item2.Months;
                            foreach (var item3 in itemMonths)
                            {
                                ws.Cells[currentRow, col++].Value = item3.NewVacanciesCount;
                            }
                        }
                        if (item2.YearIn == 2025)
                        {
                            col += 26;
                            ws.Cells[currentRow, col++].Value = item2.TotalForYear;
                            var itemMonths = item2.Months;
                            foreach (var item3 in itemMonths)
                            {
                                ws.Cells[currentRow, col++].Value = item3.NewVacanciesCount;
                            }
                        }
                        if (item2.YearIn == 2026)
                        {
                            col += 39;
                            ws.Cells[currentRow, col++].Value = item2.TotalForYear;
                            var itemMonths = item2.Months;
                            foreach (var item3 in itemMonths)
                            {
                                ws.Cells[currentRow, col++].Value = item3.NewVacanciesCount;
                            }
                        }

                    }
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }

	}

    public class GrapthYear
    {
        public int YearIn { get; set; }
        public int TotalForYear { get; set; }
        public List<GrapthMonth> Months { get; set; } = new();
    }

    public class GrapthMonth
    {
        public int MonthIn { get; set; }
        public int TotalForMonth { get; set; }
        public int NewVacanciesCount { get; set; }
    }
}
