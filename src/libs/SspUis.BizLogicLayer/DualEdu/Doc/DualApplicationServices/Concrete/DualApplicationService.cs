using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Billing.Models;
using SspUis.Integration.Billing.Services;
using SspUis.Integration.DocxToPdf;
using SspUis.Integration.Dual.Services;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public class DualApplicationService
        : BaseApplicationService<DualApplication, DualApplicationListDto,
            DualApplicationDto, CreateDualApplicationDlDto,
            UpdateDualApplicationDlDto, IDualApplicationRepository,
            DualApplicationSortFilterOptions>, IDualApplicationService
    {
        private readonly IAuthService _authService;
        private readonly IEImzoService _eImzoService;
        private readonly INumberService _numberService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IBaseReportService _baseReportService;
        private readonly SystemConf _systemConf;
        private readonly IContractorService _contractorService;
        private readonly IConvertService _pdfConverter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;
        private readonly IWbImzoService _wbImzoService;
        private readonly WbImzoConfig _wbImzoConfig;
        private readonly LinkConfig _linkConfig;
        private readonly IBillingService _integrationService;
        private readonly IDualService _integrationDualService;
        private readonly IApiRequestLogRepository _apiRequestLogRepository;

        public DualApplicationService(IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IBaseReportService baseReportService,
            IEImzoService eImzoService,
            IContractorService contractorService,
            SystemConf systemConf,
            IConvertService pdfConverter,
            IStorageService storageService,
            ICultureHelper cultureHelper,
            IBillingService integrationService,
            IDualService integrationDualService,
            WbImzoConfig wbImzoConfig,
            List<LinkConfig> linkConfigs,
            IWbImzoService wbImzoService,
            IApiRequestLogRepository apiRequestLogRepository)
            : base(unitOfWork, documentChangeLogService)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _numberService = numberService;
            _documentChangeLogService = documentChangeLogService;
            _contractorService = contractorService;
            _storageService = storageService;
            _baseReportService = baseReportService;
            _eImzoService = eImzoService;
            _systemConf = systemConf;
            _pdfConverter = pdfConverter;
            _storageService = storageService;
            _cultureHelper = cultureHelper;
            _wbImzoService = wbImzoService;
            _wbImzoConfig = wbImzoConfig;
            _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "DualApplication");
            _integrationDualService = integrationDualService;
            _integrationService = integrationService;
            _apiRequestLogRepository = apiRequestLogRepository;
        }

        protected override IQueryable<DualApplicationListDto> SortFilter(IQueryable<DualApplicationListDto> query, DualApplicationSortFilterOptions options)
        {
            return base.SortFilter(query, options)
                .SortFilter(options).Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.DUALEDU);
        }
        public override PagedResult<DualApplicationListDto> GetList(DualApplicationSortFilterOptions options)
        {
            PagedResult<DualApplicationListDto> res = GetDualApplicationListDtoList(options).AsPagedResult(options);
            // test uchun bu yerda chaqirilgan
            //var result = _integrationDualService.RejectDualApplication(new DualApplicationRejectDto()
            //{
            //    Id = 21,
            //    Message = "test"
            //});

            return res;

            //return Repository.ReadAsNoTracked<DualApplicationListDto>()
            //    .SortFilter(options)
            //    .AsPagedResult(options);
        }

		public int GetCount()
		{
			DualApplicationSortFilterOptions options = new DualApplicationSortFilterOptions();
            var result  = GetDualApplicationListDtoList(options).Count();
            return result;
		}

		public IQueryable<DualApplicationListDto> GetDualApplicationListDtoList(DualApplicationSortFilterOptions options)
        {
            IQueryable<DualApplicationListDto> res = Repository.ReadAsNoTracked<DualApplicationListDto>()
               .SortFilter(options);
            return res;
		}

        public Stream SaveExcelDualApplication(DualApplicationSortFilterOptions options)
        {
            var data = GetDualApplicationListDtoList(options).ToList();
            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.DUAL_APPLICATION_GETLIST));
            if(IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage excelPackage = new ExcelPackage(template);
                var importRow = excelPackage.Workbook.Names["ImportRow"];
                var currentRow = importRow.Start.Row + 1;
                var ws = importRow.Worksheet;
				int index = data.Count;
				data.Reverse();
				foreach (var item in data)
                {
                    var dualApplicationDto = Get(item.Id);
					int column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index--;
                    ws.Cells[currentRow, column++].Value = item.DocNumber;
                    ws.Cells[currentRow, column++].Value = item.DocOn;
                    ws.Cells[currentRow, column++].Value = item.Contractor;
                    ws.Cells[currentRow, column++].Value = item.ContractorPhoneNumber;
                    ws.Cells[currentRow, column++].Value = item.Region;
                    ws.Cells[currentRow, column++].Value = item.District;
                    ws.Cells[currentRow, column++].Value = (dualApplicationDto != null && dualApplicationDto.Tables.Any()) 
                        ? dualApplicationDto.Tables.FirstOrDefault().Institute : string.Empty;

					ws.Cells[currentRow, column++].Value = (dualApplicationDto != null && dualApplicationDto.Tables.Any())
				     ? dualApplicationDto.Tables.FirstOrDefault().Specialty : string.Empty;

					ws.Cells[currentRow, column++].Value = (dualApplicationDto != null && dualApplicationDto.Tables.Any())
					 ? dualApplicationDto.Tables.FirstOrDefault().PositionClassification : string.Empty;

					ws.Cells[currentRow, column++].Value = (dualApplicationDto != null && dualApplicationDto.Tables.Any())
					 ? dualApplicationDto.Tables.FirstOrDefault().EmptyPositionsCount : string.Empty;


					ws.Cells[currentRow, column++].Value = (dualApplicationDto != null && dualApplicationDto.Tables.Any())
					 ? dualApplicationDto.Tables.FirstOrDefault().Details : string.Empty;

				}

                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }

	        return result;
        }
		public override DualApplicationDto Get()
        {
            var region = UnitOfWork.RegionRepository.ById<RegionListDto>(_authService.Contractor.RegionId);
            var district = UnitOfWork.DistrictRepository.ById<DistrictListDto>(_authService.Contractor.DistrictId);
            var contractor = UnitOfWork.ContractorRepository.ById<ContractorListDto>(_authService.Contractor.Id);
            return new DualApplicationDto
            {
                Application = new()
                {
                    DocOn = DateTime.Now.AsDateOnly(),
                    ContractorPositionName = "Директор",
                    DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                    Contractor = _authService.Contractor.FullName,
                    ContractorInn = _authService.Contractor.Inn,
                    ContractorId = _authService.Contractor.Id,
                    ContractorAddress = contractor.Address,
                    ContractorDirector = contractor.Director,
                    RegionId = _authService.Contractor.RegionId,
                    Region = region.FullName,
                    DistrictId = _authService.Contractor.DistrictId,
                    District = district.FullName,
                    ApplicationTypeId = ApplicationTypeIdConst.DUALEDU,
                },
                CanEdit = true,
            };
        }
        public override DualApplicationDto Get(long id)
        {
            var dto = Repository.ById<DualApplicationDto>(id);

            if (dto == null)
                return null;
            if (_authService.Contractor != null)
            {
                dto.CanAccept = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED);
                dto.CanReject = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED);
                dto.CanCancel = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.CANCELED);
                dto.CanEdit = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.MODIFIED);
                dto.CanSend = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.SENT);
                dto.CanRevoke = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REVOKED);
                dto.CanDelete = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.DELETED);
            }
            else
            {
                dto.CanAccept = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED)
                                  && _authService.HasPermission(ModuleCode.DualApplicationAccept);
                dto.CanReject = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED)
                                  && _authService.HasPermission(ModuleCode.DualApplicationReject);
                dto.CanCancel = StatusIdConst.CanDualApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.CANCELED)
                                  && _authService.HasPermission(ModuleCode.DualApplicationCancel);
            }

            //dto.MemshipContractId = UnitOfWork.Context.Set<MemshipContract>()
            //    .FirstOrDefault(c =>
            //    c.ApplicationId == dto.ApplicationId
            //    && c.StatusId != StatusIdConst.DELETED)?.Id;
            return dto;
        }
        private IQueryable<TDto> GetQuery<TDto>()
            where TDto : class
        {
            return Repository.ReadAsNoTracked<TDto>();
        }
        public DualApplicationDto Get(Guid id2)
        {
            var dto = GetQuery<DualApplicationDto>()
                     .FirstOrDefault(a => a.Application.Id2 == id2 && new int[] { StatusIdConst.SENT, StatusIdConst.ACCEPTED }.Contains(a.Application.StatusId));
            if (dto == null)
                AddError("Ariza topilmadi / Заявление не найдено!");
            return dto;
        }
        public override HaveId<long> Create(CreateDualApplicationDlDto dto)
        {
            //if (Repository.AllAsQueryable.Any(a => a.Application.StatusId != StatusIdConst.REJECTED && a.Application.StatusId != StatusIdConst.CANCELED && a.Application.ApplicationTypeId == ApplicationTypeIdConst.DUALEDU))
            //{
            //    AddError("Ariza allaqachon yaratilgan / Заявка уже создана");
            //    return null;
            //}

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Create(dto, ent =>
                    {
                    });
                    CombineStatuses(Repository);
                    if (HasErrors)
                        return null;
                    UnitOfWork.Save();

                    Repository.UpdateStatus(new()
                    {
                        Id = entity.Id,
                        StatusId = StatusIdConst.CREATED,
                        Message = "Created and automatic sent."
                    });
                    var res = CreateDocumentChangeLog(entity.Id);
                    if (IsValid)
                    {
                        transaction.Commit();
                        return HaveId.Create(entity.Id);
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} : {ex.InnerException}");
                }
            }
            return null;
        }
        public override void Update(UpdateDualApplicationDlDto dto)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                var entity = Repository.Update(dto, ent =>
                {

                });

                CombineStatuses(Repository);
                if (IsValid)
                    UnitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);
                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }
        public bool CanCreate(string inn = null)
        {
            inn ??= _authService.Contractor.Inn;
            var dto = Repository.CrudServices.ReadManyNoTracked<DualApplicationDto>()
                        .Where(a => a.Application.ContractorInn == inn
                                && a.Application.ApplicationTypeId == ApplicationTypeIdConst.DUALEDU
                                //&& a.Application.StatusId != StatusIdConst.DELETED
                                //&& a.Application.StatusId != StatusIdConst.REJECTED
                                //&& a.Application.StatusId != StatusIdConst.CANCELED
                                ).ToList();
            //if (dto.Any())
            //    return false;
            return true;
        }
        public void Accept(AcceptStatusDualApplicationDto dto)
        {
            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();
            try
            {
                UnitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);
                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public void Reject(RejectStatusDualApplicationDto dto)
        {
            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

            try
            {
                UnitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public void Cancel(CancelStatusDualApplicationDto dto)
        {
            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

            try
            {
                UnitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public void Revoke(RevokeStatusDualApplicationDto dto)
        {
            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

            try
            {
                UnitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public async Task SendToHierEdu(long id)
        {
            var log = new CreateApiRequestLogDlDto
            {
                DocumentId = id,
                TableId = TableIdConst.DUALEDU__DOC_DUAL_APPLICATION,
                RequestAt = DateTime.Now,
            };

            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

            try
            {
                var entity = _unitOfWork.Context.Set<DualApplication>()
                                .Include(x => x.Tables)
                                .ThenInclude(s => s.Specialty)
                                .Include(a => a.Application)
                                .ThenInclude(c => c.Contractor)
                                .FirstOrDefault(a => a.Id == id);

                if (entity == null)
                {
                    AddError("Ma'lumot topilmadi");
                    throw new Exception("Document not found");
                }

                var groupedByInstitute = entity.Tables
                    .GroupBy(t => t.InstituteId)
                    .Select(group => new DualApplicationCreateDto1
                    {
                        OrganizationId = group.Key,
                        ContractorInn = entity.Application.Contractor.Inn,
                        ExternalApplicationId = entity.ApplicationId,
                        Tables = group.Select(table => new SpecialtyApplication
                        {
                            EduTypeId = 1,
                            EduSpecialityId = table.Specialty.Id,
                            EmptyPositionsCount = table.EmptyPositionsCount,
                            Message = entity.Message
                        }).ToList()
                    }).ToList();

                foreach (var dualApplicationCreate in groupedByInstitute)
                {
                    await _integrationService.CreateDualApplication(dualApplicationCreate);
                    CombineStatuses(_integrationService);

                    if (HasErrors)
                    {
                        var errorMessages = GetAllErrors();
                        AddError($"Errors encountered: {string.Join(", ", errorMessages)}");
                        transaction.Rollback();
                        return;
                    }

                    log.IsSuccess = true;
                    log.ResponseAt = DateTime.Now;
                    log.ResponseContent = JsonConvert.SerializeObject(dualApplicationCreate);
                }

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"Exception occurred: {ex.Message}");
                transaction.Rollback();
                log.Exception = ex.Message;
                throw;
            }
            finally
            {
                _apiRequestLogRepository.Create(log);
                UnitOfWork.Save();
            }
        }
        public async Task Send(SendStatusDualApplicationDto dto)
        {
            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

            var doc = Get(dto.Id);

            if (doc == null)
            {
                AddError("По вашему запросу запись не найдено");
                return;
            }
            else if (doc.Application.ContractorId != _authService.Contractor.Id)
            {
                AddError("Нет доступа");
                return;
            }

            var eImzoTimstampDto = new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = _authService.User.Inn,
                Pinfl = _authService.User.Pinfl
            };

            var timeStamp = _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : _authService.Contractor?.Inn,
                Pinfl = dto.IsPinfl
                  ? (_authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl)
                  : null
            }).Result;

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            if (_authService.Contractor != null)
            {
                var eImzoVerifyAttached = _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                    Pinfl = dto.IsPinfl ? _authService.Contractor.Pinfl : null
                }).Result;
            }
            else
            {
                var eImzoVerifyAttached = _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = null,
                    Pinfl = _authService.User.Pinfl
                }).Result;
            }
            CombineStatuses(_eImzoService);
            if (HasErrors)

            dto.SignFile = /*Guid.NewGuid();//*/  SaveFile(doc.Id, timeStamp.Pkcs7b64, "sign.txt");
            dto.DataFile = /*Guid.NewGuid();//*/  SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
            dto.SignedUserInfo = _authService.Contractor.Inn + " - " + _authService.Contractor.FullName;

            try
            {
                var res = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                UnitOfWork.Save();

                await SendToHierEdu(dto.Id);
                
                CombineStatuses(Repository);
                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
        //public byte[] DownloadPdf(Guid id2, string? lang)
        //{
        //    lang = lang ?? "uz-cyrl";

        //    var wordFile = _storageService.GetStaticFile(
        //        StaticFileConst.WordTemplate.GetFileName(
        //            lang,
        //            StaticFileConst.WordTemplate.MEMSHIP_APPLICATION)
        //        );
        //    var languageId = UnitOfWork.Context.Set<Language>()
        //        .FirstOrDefault(l => l.Code == lang)?.Id ?? 1;

        //    var memshipApplication = UnitOfWork.Context.Set<DualApplication>()
        //        .Include(a => a.Application)
        //        .FirstOrDefault(a => a.Application.Id2 == id2);

        //    if (memshipApplication == null)
        //    {
        //        AddError("not found");
        //        return null;
        //    }

        //    var contractorId = memshipApplication.Application.ContractorId;
        //    var contractor = UnitOfWork.Context.Set<Contractor>()
        //        .Include(c => c.Oked)
        //        .Include(c => c.SettlementAccounts)
        //        .Include(c => c.Bank)
        //        .Include(c => c.Region.Translates)
        //        .Include(c => c.District.Translates)
        //        .Include(c => c.Bank.Translates)
        //        .FirstOrDefault(c => c.Id == contractorId);

        //    //var sattlementAcc = UnitOfWork.Context.Set<ContractorSettlementAccount>()
        //    //    .FirstOrDefault(a => a.OwnerId == contractor.Id).AccountCode;

        //    var serviceIds = memshipApplication.ChamberServices
        //        .Select(s => s.NeedChamberServiceId)
        //        .ToList();

        //    var services = UnitOfWork.Context.Set<NeedChamberService>()
        //        .Where(t => serviceIds.Contains(t.Id)).AsEnumerable();
        //    var link = "https://my-api.chamber.uz/api/Memship/DualApplication/DownloadPdf?id2=";
        //    link = link + memshipApplication.Application.Id2.ToString();
        //    var qrCode = QRCodeHelper.GeneratePng(link, 300, 300);
        //    var qrImage = new MemoryStream(qrCode);
        //    var plh = new Placeholders();
        //    plh.ImagePlaceholders.Add("QrCode", new ImageElement
        //    {
        //        Dpi = 300,
        //        MemStream = qrImage,
        //    });
        //    plh.TextPlaceholders.Add("ContractorFullName", contractor.FullName);
        //    plh.TextPlaceholders.Add("ContractorDirector", contractor.Director);
        //    wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        //    //ChangeImage(wordFile, qrCode);
        //    var model = new DualApplicationForPdf
        //    {
        //        Id2 = id2,
        //        DocNumber = memshipApplication.Application.DocNumber ?? "-",
        //        DocOn = memshipApplication.Application.DocOn.ToString("dd.MM.yyyy") ?? "-",
        //        ContractorFullName = contractor.FullName ?? "-",
        //        ContractorDirector = contractor.Director ?? "-",
        //        RegionName = contractor.Region.Translates
        //            .FirstOrDefault(t =>
        //                t.LanguageId == languageId
        //                && t.ColumnName == TranslateColumn.full_name.ToString()
        //                )?.TranslateText ?? contractor.Region.FullName ?? "-",

        //        ContractorDistrict = contractor.Address ?? "-",
        //        DistrictName = contractor.District.Translates
        //            .FirstOrDefault(t =>
        //            t.LanguageId == languageId
        //            && t.ColumnName == TranslateColumn.full_name.ToString()
        //            )?.TranslateText ?? contractor.District.FullName ?? "-",

        //        ContractorWorkPhoneNumber = memshipApplication.ContractorWorkPhoneNumber ?? "-",
        //        ContractorMobilePhoneNumber = memshipApplication.ContractorMobilePhoneNumber ?? "-",
        //        ContractorFaks = memshipApplication.ContractorFaks ?? "-",
        //        ContractorAdditionalPhoneNumber = memshipApplication.ContractorAdditionalPhoneNumber ?? "-",
        //        ContractorEmail = memshipApplication.ContractorEmail ?? "-",
        //        ContractorWebSite = memshipApplication.ContractorWebSite ?? "-",
        //        ContractorSkype = memshipApplication.ContractorSkype ?? "-",
        //        ContractorFacebook = memshipApplication.ContractorFacebook ?? "-",
        //        ContractorTelegram = memshipApplication.ContractorTelegram ?? "-",
        //        RegDocOn = contractor.RegistrationDate.ToString("dd.MM.yyyy") ?? "-",
        //        RegDocNumber = contractor?.RegistrationNumber ?? "-",
        //        Inn = contractor.Inn ?? "-",
        //        OkedCode = contractor.Oked.Code ?? "-",
        //        ActivityTypeFullName = memshipApplication.ContractorActivityType.Translates
        //            .FirstOrDefault(c =>
        //            c.LanguageId == languageId
        //            && c.ColumnName == TranslateColumn.full_name.ToString()
        //            )?.TranslateText ?? memshipApplication.ContractorActivityType.FullName ?? "-",

        //        ContractorCategoryFullName = memshipApplication.ContractorCategory.Translates
        //            .FirstOrDefault(c =>
        //                c.LanguageId == languageId
        //                && c.ColumnName == TranslateColumn.full_name.ToString()
        //                )?.TranslateText ?? memshipApplication.ContractorCategory.FullName ?? "-",

        //        EmployeesCount = memshipApplication.EmployeesCount.ToString() ?? "-",
        //        BankName = contractor.Bank?.Translates.FirstOrDefault(c =>
        //            c.LanguageId == languageId
        //            && c.ColumnName == TranslateColumn.full_name.ToString()
        //            )?.TranslateText ?? contractor.Bank.BankName ?? "-",

        //        BankCode = contractor.Bank.BankCode ?? "-",
        //        SettlementAccount = contractor.SettlementAccounts
        //            .FirstOrDefault(a => a.IsMain)?.AccountCode ?? "-",
        //        YearlyEarnings = memshipApplication.YearlyEarnings.ToString().FormatNumber(3) ?? "-",
        //        YearlyTaxes = memshipApplication.YearlyTaxes.ToString().FormatNumber(3) ?? "-",
        //        YearlyExport = memshipApplication.YearlyExport.ToString().FormatNumber(3) ?? "-",
        //        YearlyImport = memshipApplication.YearlyImport.ToString().FormatNumber(3) ?? "-",
        //        YearlyManufacture = memshipApplication.YearlyManufacture.ToString().FormatNumber(3) ?? "-",

        //        NeedChamberService = String.Join(",", services.Select(a => a.FullName)) ?? "-"
        //    };

        //    return _pdfConverter.DocxToPdfAsync(wordFile, model);
        //}
        ////private void ChangeImage(MemoryStream wordfile, byte[] img)
        ////{
        ////    using (WordprocessingDocument doc = WordprocessingDocument.Open(wordfile, true))
        ////    {
        ////        MainDocumentPart mainPart = doc.MainDocumentPart;

        ////        // Find the image part you want to replace (assuming it's the first image)
        ////        var imagePart = mainPart.ImageParts.Skip(2).FirstOrDefault();
        ////        if (imagePart != null)
        ////        {
        ////            // Load the new image
        ////            ImagePart newImagePart = mainPart.AddImagePart(ImagePartType.Png);
        ////            using (MemoryStream stream = new MemoryStream(img))
        ////            {
        ////                newImagePart.FeedData(stream);
        ////            }

        ////            // Update the relationship of the existing image with the new image part
        ////            var blip = mainPart.Document.Body.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().FirstOrDefault();
        ////            if (blip != null)
        ////            {
        ////                var existingRelationshipId = blip.Embed;
        ////                var newRelationshipId = mainPart.GetIdOfPart(newImagePart);
        ////                blip.Embed = newRelationshipId;

        ////                // Remove the old image part and its relationship
        ////                var partToDelete = mainPart.Parts.SingleOrDefault(p => p.RelationshipId == existingRelationshipId);
        ////                if (partToDelete != null)
        ////                {
        ////                    mainPart.DeletePart(partToDelete.OpenXmlPart);
        ////                    //mainPart.DeleteReferenceRelationship(existingRelationshipId);
        ////                }
        ////                else
        ////                {
        ////                    Console.WriteLine("Existing image part not found.");
        ////                }
        ////            }
        ////            else
        ////            {
        ////                Console.WriteLine("Image element not found in the document.");
        ////            }
        ////        }
        ////        else
        ////        {
        ////            Console.WriteLine("Image part not found in the document.");
        ////        }

        ////        // Save the changes
        ////        mainPart.Document.Save();
        ////    }
        ////}
        //public byte[] DownloadPdf(DualApplicationForPdf model)
        //{
        //    model.lang = model.lang ?? "uz-cyrl";

        //    var wordFile = _storageService.GetStaticFile(
        //        StaticFileConst.WordTemplate.GetFileName(
        //            model.lang,
        //            StaticFileConst.WordTemplate.MEMSHIP_APPLICATION)
        //        );

        //    var languageId = UnitOfWork.Context.Set<Language>()
        //        .FirstOrDefault(l => l.Code == model.lang)?.Id ?? 1;

        //    var memship = UnitOfWork.Context.Set<DualApplication>()
        //        .Include(a => a.Application)
        //        //.Include(a => a.ChamberServices)
        //        .FirstOrDefault(a => a.Application.Id2 == model.Id2);

        //    if (memship is null)
        //    {
        //        AddError("not found");
        //        return null;
        //    }

        //    var contractorId = memship.Application.ContractorId;
        //    var contractor = UnitOfWork.Context.Set<Contractor>()
        //        .Include(c => c.Oked)
        //        .Include(c => c.SettlementAccounts)
        //        .Include(c => c.Bank)
        //        //.Include(c => c.Region.Translates)
        //        //.Include(c => c.District.Translates)
        //        .Include(c => c.Bank.Translates)
        //        .FirstOrDefault(c => c.Id == contractorId);

        //    var serviceIds = memship.ChamberServices
        //        .Select(s => s.NeedChamberServiceId)
        //        .ToList();

        //    var services = UnitOfWork.Context.Set<NeedChamberService>()
        //        .Where(t => serviceIds.Contains(t.Id)).AsEnumerable();

        //    model.EmployeesCount = memship.EmployeesCount.ToString() ?? "-";
        //    model.BankCode = contractor.Bank.BankCode ?? "-";

        //    model.BankName = contractor.Bank.Translates.FirstOrDefault(c =>
        //            c.LanguageId == languageId
        //            && c.ColumnName == TranslateColumn.full_name.ToString()
        //            )?.TranslateText ?? contractor.Bank.BankName ?? "-";

        //    model.OkedCode = contractor.Oked.Code ?? "-";
        //    model.SettlementAccount = contractor.SettlementAccounts
        //            .FirstOrDefault(a => a.IsMain)?.AccountCode ?? "-";
        //    model.RegDocOn = contractor.RegistrationDate.ToString("dd.MM.yyyy") ?? "-";
        //    model.RegDocNumber = contractor.RegistrationNumber ?? "-";
        //    model.NeedChamberService = String.Join(",", services.Select(a => a.FullName)) ?? "-";

        //    return _pdfConverter.DocxToPdfAsync(wordFile, model);
        //}
        private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = Repository.ById<DualApplicationDto>(id, applyFilter: false);
            _documentChangeLogService.CreateApplication(
                dto: entityDto,
                organizationId: null,
                message: message);
            CombineStatuses(_documentChangeLogService);

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
            var fileInfo = _storageService.Save($"{nameof(TableIdConst.DUALEDU__DOC_DUAL_APPLICATION)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

            return fileInfo.FirstOrDefault().FileId;
        }
        public async Task<byte[]> DownloadPdf(Guid id2, string lang)
        {
            lang = "uz-latn";
           
            var dto = Repository.ReadAsNoTracked<AppointEmployeeDto>(applyFilter: false)
                                .FirstOrDefault(x => x.Id2 == id2);

            MemoryStream wordFile = null;

            wordFile = _storageService.GetStaticFile(
               StaticFileConst.WordTemplate.GetFileName(
            lang,
            StaticFileConst.WordTemplate.APPOINT_EMPLOYEE_HIRE));

            if (dto == null)
            {
                AddError("Hujjat topilmadi.");
                return null;
            }

            var plh = WordFactory.MakePlaceholders(dto);
            var handler = new DocXHandler(wordFile, plh);
            handler.ReplaceLists();
            handler.ReplaceTexts();
            handler.ReplaceImages();
            var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
            CombineStatuses(_pdfConverter);
            return res;
        }

        public async ValueTask<string> WebImzoSign(WebImzoSignedFilter filter)
        {
            var doc = _unitOfWork.Context.Set<DualApplication>()
                                .Include(x => x.Application).ThenInclude(x => x.Contractor)
                                .FirstOrDefault(x => x.Id == filter.Id);

            if (doc == null)
            {
                AddError("По вашему запросу запись не найдено");
                return null;
            }

            return await PostToIMZOAndSentUrl(doc);
        }

        public async ValueTask<string> PostToIMZOAndSentUrl(DualApplication contract)
        {
            var canDispose = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(contract);
            signRequestCreateDto.SignRequestUsers = new List<WbImzoCreateSignRequestUserDto>
                                                    {
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(contract.Application.Contractor.Inn) ? contract.Application.Contractor.Inn : contract.Application.Contractor.Pinfl,
                                                            UserInfo = contract.Application.Contractor.FullName,
                                                            UserId = (int)contract.Application.ContractorId,
                                                            DocStatusId = StatusIdConst.SIGNED,
                                                            SignPriority = 1,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = contract.Application.Contractor.PhoneNumber
                                                        }
                                                    };

            var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
            if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
                CombineStatuses(wbImzoResult.GetStatusGeneric());

            if (HasErrors || wbImzoResult.Response is null)
            {
                AddError("Dual Ariza imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
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

            var contract = _unitOfWork.Context.Set<DualApplication>().FirstOrDefault(a => a.Id == contractId);

            if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
            {
                AddError("Dual ariza imzolash uchun yuborilayotgan jarayonda qaytgan keylani saqlashda xatolik yuz berdi");
                return null;
            }

            var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

            return url;

        }

        public WbImzoCreateSignRequestDto CreatDtoForRequestSign(DualApplication contract)
        {
            var organization = _unitOfWork.OrganizationRepository.AllAsQueryable.FirstOrDefault(a => a.Id == contract.OrganizationId);

            if (contract == null)
            {
                AddError("tashkilot topilmadi!");
            }
            string documentDataAsString = JsonConvert.SerializeObject(ConvertToDto(contract));
            //var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}&lang=uz-cyrl&__lang=uz-cyrl&langId=2";

            return new WbImzoCreateSignRequestDto
            {
                DocumentId = contract.Id,
                ApiKey = _wbImzoConfig.ApiKey,
                //Title = contract.DocNumber,
                IsForceCreate = true,
                TableId = TableIdConst.DUALEDU__DOC_DUAL_APPLICATION,
                SignData = documentDataAsString,
                OrganizationInn = organization != null ? organization.Inn : null,
                OrganizationName = organization != null ? organization.FullName : null,
               // PrintableLink = filePrintableLink,
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
        public WebImzoDto ConvertToDto(DualApplication dto)
        => new()
        {
            DocOn = dto.CreatedAt.AsDateOnly(),
            Id = dto.Id,
        };
	}
}