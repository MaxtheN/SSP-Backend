using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.Srv.Doc.SrvApplicationService.Dtos;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationService : BaseApplicationService
        <ServiceApplication,
        SrvApplicationListDto,
        SrvApplicationDto,
        CreateServiceApplicationDlDto,
        UpdateServiceApplicationDlDto,
        IServiceApplicationRepository,
        SrvApplicationSortFilterOptions>, ISrvApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly INumberService _numberService;
        private readonly IStorageService _storageService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IConvertService _pdfConverter;
        private readonly SystemConf _systemConf;
        private readonly ICultureHelper _cultureHelper;
        private readonly ISrvCompleteService _completeService;
        public SrvApplicationService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IStorageService storageService,
            ICultureHelper cultureHelper,
            IConvertService pdfConverter,
            SystemConf systemConf,
            ISrvCompleteService completeService)
            : base(unitOfWork, documentChangeLogService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _numberService = numberService;
            _storageService = storageService;
            _cultureHelper = cultureHelper;
            _documentChangeLogService = documentChangeLogService;
            _pdfConverter = pdfConverter;
            _systemConf = systemConf;
            _completeService = completeService;
        }

        #region C O R E
        public SelectList<long> AsSelectList(SrvApplicationSortFilterOptions options)
        {
            return Repository.ReadAsNoTracked<SrvApplicationListDto>()
                .SortFilter(options)
                .AsSelectList();
        }
        public override WEBASE.Models.PagedResult<SrvApplicationListDto> GetList(SrvApplicationSortFilterOptions options)
        {
            var rows = Repository.ReadAsNoTracked<SrvApplicationListDto>()
                            .SortFilter(options)
                            .AsPagedResult(options);
            return rows;
        }

		public int GetCount()
		{
			 SrvApplicationSortFilterOptions srvApplicationSortFilterOptions = 
                new SrvApplicationSortFilterOptions();
            var data = Repository.ReadAsNoTracked<SrvApplicationListDto>()
                            .SortFilter(srvApplicationSortFilterOptions).Count();

			return data;
			
		}

		public int GetPayedCount()
		{

			SrvApplicationSortFilterOptions srvApplicationSortFilterOptions =
			   new SrvApplicationSortFilterOptions()
               {
                   IsFree = false
               };
			var data = Repository.ReadAsNoTracked<SrvApplicationListDto>()
							.SortFilter(srvApplicationSortFilterOptions).Count();

			return data;
		}

		public int GetFreeCount()
		{
			SrvApplicationSortFilterOptions srvApplicationSortFilterOptions =
			   new SrvApplicationSortFilterOptions()
			   {
				   IsFree = true
			   };
			var data = Repository.ReadAsNoTracked<SrvApplicationListDto>()
							.SortFilter(srvApplicationSortFilterOptions).Count();

			return data;
		}
		public override SrvApplicationDto Get()
        {
            if (_authService.Contractor == null)
            {
                AddError("Bunday shartnomachi mavjud emas !");
                return null;
            }

            var region = _unitOfWork.RegionRepository.ById<RegionListDto>(_authService.Contractor.RegionId);

            var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(_authService.Contractor.DistrictId);

            var contractor = _unitOfWork.ContractorRepository.ById<ContractorListDto>(_authService.Contractor.Id);

            return new SrvApplicationDto
            {
                Application = new()
                {
                    Id2 = Guid.NewGuid(),
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
                    ApplicationTypeId = ApplicationTypeIdConst.CLAIM,
                    DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                },
                Groups = new(),
                CreatedAt = DateTime.Now
            };
        }
        public override SrvApplicationDto Get(long id)
        {
            var dto = Repository.ById<SrvApplicationDto>(id);

            if (dto == null)
                return null;
            return dto;
        }
        public SrvApplicationDto Get(Guid id2)
        {
            var dto = Repository.ReadAsNoTracked<SrvApplicationDto>()
                .FirstOrDefault(a => a.Application.Id2 == id2);

            if (dto == null)
                AddError("Ariza topilmadi / Заявление не найдено!");

            return dto;
        }
        public async ValueTask<HaveId<long>> CreateSrv(CreateServiceApplicationDlDto dto)
        {
            if (dto.ToRegionalOffice is true) dto.DistrictId = null;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {

                    ServiceApplication entity = Repository.Create(dto, ent => SrvValidation(dto, ent));
                    CombineStatuses(Repository);
                    if (HasErrors)
                        return null;

                    entity.Groups.SelectMany(group => group.Tables).ToList()
                        .ForEach(table =>
                        {
                            _storageService.MoveToPersistent(
                                DocumentStorageConst.DOC_SERVICE_APPLICATION_TABLE_FILES,
                                $"{table.Id}",
                                table.Files.Select(a => a.Id).ToArray());

                            CombineStatuses(_storageService);
                        });

                    if (HasErrors)
                    {
                        AddError("");
                        return null;
                    }

                    UnitOfWork.Save();

                    await this.SendAsync(new SendStatusSrvApplicationDto()
                    {
                        Id = entity.Id,
                        Message = "Юборилди",
                    }, true);

                    var res = CreateDocumentChangeLog(entity.Id);

                    CombineStatuses(_documentChangeLogService);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

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
                finally
                {
                    transaction.Dispose();
                }
            }
            return null;
        }
        public override void Update(UpdateServiceApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = Repository.Update(dto, ent =>
                {
                    if (!StatusIdConst.CanEditStatuses.Contains(ent.Application.StatusId))
                        AddError("Tahrirlash mumkin emas / Невозможно редактировать");
                });

                CombineStatuses(Repository);

                foreach (var item in dto.Groups)
                    _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES, $"{item.Id}");

                if (IsValid)
                    UnitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);

                if (IsValid)
                    transaction.Commit();
            }
        }
        public override void Delete(long id)
        {
            var dto = Repository.ById<SrvApplicationDto>(id);
            if(dto == null)
            {
                AddError("Ariza topilmadi / Заявление не найдено!");
                return;
            }

            if(dto.Application.StatusId != StatusIdConst.REJECTED)
            {
                AddError("Ariza rad etilmagan");
                return;
            }

            base.Delete(id);
        }
        #endregion

        #region S T A T U S
        public void Accept(AcceptStatusSrvApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.Context.Set<ServiceApplication>().Lock(dto.Id);

                var entity = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });
                if (entity.IsFree)
                { AddError("Bu ariza yaxlit pulli ariza emas IsFree si TRUE turibdi !"); return; }

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} - {ex.InnerException}");
                return;
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public void AcceptForFree(AcceptStatusSrvApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.Context.Set<ServiceApplication>().Lock(dto.Id);
                var entity = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });
                if (!entity.IsFree)
                { AddError("Bu ariza yaxlit tekin ariza emas IsFree si FALSE turibdi !"); return; }

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                var complete = _completeService.Create(new CreateCompletedServiceDlDto
                {
                    DocNumber = entity.Application.DocNumber,
                    DocOn = DateOnly.FromDateTime(DateTime.Now),
                    ContractorId = entity.Application.ContractorId.Value,
                    ServiceContractId = null,
                    ServiceApplicationId = entity.Id,
                    EmployeeManageId = entity.EmployeeManageId,
                });
                CombineStatuses(_completeService);
                if (HasErrors)
                    return;

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} - {ex.InnerException}");
                return;
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public void Received(RecievedStatusSrvApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.Context.Set<ServiceApplication>().Lock(dto.Id);

                var entity = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });
                entity.EmployeeManageId = _authService.User.EmployeeManageId;
                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} - {ex.InnerException}");
                return;
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public void Cancel(CancelStatusSrvApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                var update = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                UnitOfWork.Save();

                var log = CreateDocumentChangeLog(id: dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} - {ex.InnerException}");
                return;
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public void Reject(RejectStatusSrvApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                var update = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();

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
        public async Task SendAsync(SendStatusSrvApplicationDto dto, bool isCreate)
        {
            if (!isCreate)
            {
                var doc = _unitOfWork.Context.Set<ServiceApplication>().Include(x => x.Application)
                    .Where(x => x.Application.StatusId == StatusIdConst.DELETED
                        && x.Application.StatusId == StatusIdConst.REJECTED
                        && x.Application.StatusId == StatusIdConst.CANCELED)
                    .FirstOrDefault(x => x.ApplicationId == dto.Id);

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
            }

            try
            {
                var res = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                UnitOfWork.Save();
            }
            catch (DbUpdateException e)
            {
                AddError($"{e.Message} - {e.InnerException}");
            }
        }
        #endregion

        #region F I L E
        public void DeleteFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<ClaimApplicationFile>()
                .FirstOrDefault(a => a.Id == fileId);

            Delete(fileId, entity, DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES);
        }
        public StorageFile DownloadFile(Guid fileId)
        {
            var entity = _unitOfWork.Context.Set<ServiceApplicationTableFile>()
                .Include(a => a.Owner)
                .FirstOrDefault(a => a.Id == fileId);

            return Download(fileId, entity, DocumentStorageConst.DOC_SERVICE_APPLICATION_TABLE_FILES);
        }
        public IEnumerable<SrvApplicationTableFileDto> UploadFiles(params StorageFile[] files)
        {
            var result = _storageService.SaveTemp(DocumentStorageConst.DOC_SERVICE_APPLICATION_TABLE_FILES, files).Select(a => new SrvApplicationTableFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                CreatedAt = DateTime.Now
            });
            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }
        private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
        {
            if (entity != null)
            {
                _storageService.DeleteTemp(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
        }
        private StorageFile Download(Guid fileId, ServiceApplicationTableFile entity, string storageDocument)
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
        public byte[] DownloadPdf(Guid id2, string? lang)
        {
            var language = lang ?? "uz-latn";
            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                   language,
                    StaticFileConst.WordTemplate.SERVICE_APPLICATION));

            var application = UnitOfWork.Context.Set<ServiceApplication>()
                .Include(x => x.Application).ThenInclude(x => x.Contractor)
                .Include(x => x.Groups).ThenInclude(x => x.Tables).ThenInclude(x => x.NeedChamberService)
                .Include(x => x.Region).Include(x => x.District)
                .AsSplitQuery()
                .FirstOrDefault(a => a.Application.Id2 == id2);

            if (application is null)
            { AddError("Приложение не найдено !"); return null; }

            var plh = new Placeholders();
            var link = _systemConf.QrImagePrintMy + "/Service/ServiceApplication/DownloadPdf?id2=" + application.Application.Id2.ToString();
            var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
            plh.ImagePlaceholders.Add("QrCode", new() { Dpi = 512, MemStream = qrDownload });

            plh.TextPlaceholders.Add("DocOn", application.Application.DocOn.ToString());
            plh.TextPlaceholders.Add("DocNumber", application.Application.DocNumber);

            plh.TextPlaceholders.Add("FullName", application.Application.Contractor.FullName);

            plh.TextPlaceholders.Add("ContractorFullName", application.Application.Contractor.Director);
            plh.TextPlaceholders.Add("ContractorInn", application.Application.Contractor.Inn.IsNullOrEmpty()
                    ? application.Application.Contractor.Pinfl
                    : application.Application.Contractor.Inn);

            var sett = Repository.Context.Set<ContractorSettlementAccount>()
                .FirstOrDefault(x => x.IsMain && x.OwnerId == application.Application.ContractorId);
            plh.TextPlaceholders.Add("ContractorSettlementAccount", (sett != null ? sett.AccountCode : string.Empty));

            plh.TextPlaceholders.Add("OrgRegion", application.Region.FullName);
            if (application.District != null)
            {
                plh.TextPlaceholders.Add("OrgDistrict", application.District.FullName);
            }
            else
            {
                plh.TextPlaceholders.Add("OrgDistrict", "");
            }

            // Xizmat nomi chiqdi faqat.
            var chamberServices = application.Groups
                .SelectMany(group => group.Tables)
                .Select(table => table.NeedChamberService.FullName)
                .ToList();

            var servicesPlh = new List<Placeholders>();
            foreach (var service in chamberServices)
            {
                var itemPlh = new Placeholders();
                itemPlh.TextPlaceholders.Add(nameof(service), service);
                servicesPlh.Add(itemPlh);
            }
            plh.TemplateListPlaceholders.Add("ChamberServices", servicesPlh);

            wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
            return _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
        }
        #endregion

        #region H E L P E R
        private void SrvValidation<TDto>(ServiceApplicationDlDto<TDto> dto, ServiceApplication entity)
            where TDto : ServiceApplicationDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (dto is CreateServiceApplicationDlDto createDto)
            {
                var query1 = _unitOfWork.Context.Set<NeedChamberService>()
                    .Where(x => GetNeedChamberServiceIds(dto).Contains(x.Id));

                var need = query1.ToDictionary(c => c.Id, c => new
                {
                    IsOffer = c.IsOffer,
                    ServicePriceTypeId = c.ServicePriceTypeId
                });

                if (dto.IsFree
                    && entity.Groups.Any(x => x.Tables.Any(y => need[y.NeedChamberServiceId].ServicePriceTypeId != ServicePriceTypeIdConst.FREE)))
                {
                    AddError("Tekin xizmatdan foydalanmay turib #FRONT IsFree polyani true bera olmaydi.\nERROR: #FRONT");
                    return;
                }

                foreach (var group in entity.Groups)
                {
                    foreach (var table in group.Tables)
                    {
                        if (need[table.NeedChamberServiceId].IsOffer && table.OfferServiceText.IsNullOrEmpty())
                            AddError($"Шу {table.Id} ид ли model учун. Сиз офферта си бор хизматдан фойдаланмоқдасиз. Офферта техт беришингиз шарт !");
                        else if (!need[table.NeedChamberServiceId].IsOffer && !table.OfferServiceText.IsNullOrEmpty())
                            table.OfferServiceText = null;
                    }
                }
            }
            else
            {

            }
        }
        public int[] GetNeedChamberServiceIds<TDto>(ServiceApplicationDlDto<TDto> dto)
            where TDto : ServiceApplicationDlDto<TDto>
        {
            List<int> res = new();
            foreach (var item in dto.Groups)
                res.AddRange(item.Tables.Select(x => x.NeedChamberServiceId));

            return res.ToArray();
        }
        private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = Repository.ById<SrvApplicationDto>(id: id, applyFilter: false);

            _documentChangeLogService.CreateApplication(entityDto, null, message);

            CombineStatuses(_documentChangeLogService);
            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }
        #endregion
        public Stream PrinSrvApplicationExcel(SrvApplicationSortFilterOptions dto)
        {
            //var data = GetList(dto);
            var data = Repository.ReadAsNoTracked<SrvApplicationListDto>()
                           .SortFilter(dto)
                           .Take(1000000)
                           .ToList();


            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.DOC_SRV_APPLICATION_LIST));

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
                    ws.Cells[currentRow, column++].Value = item.Region;
                    ws.Cells[currentRow, column++].Value = item.Id;
                    ws.Cells[currentRow, column++].Value = item.Application.DocNumber;
                    ws.Cells[currentRow, column++].Value = item.Application.DocOn.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.Application.ContractorInn + " - " + item.Application.Contractor;
                    ws.Cells[currentRow, column++].Value = item.Application.ContractorPhoneNumber;
                    ws.Cells[currentRow, column++].Value = item.IsFree ? "Ha" : "Yo'q";
                    ws.Cells[currentRow, column++].Value = item.District;
                    ws.Cells[currentRow, column++].Value = item.Application.ContractorAdress;
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

        public SrvStatisticsDto GetStatisticsDto()
        {
           

           var contractorId = _authService.Contractor.Id;

           var application = _unitOfWork.Context.Set<ServiceApplication>().
                Include(a=>a.Application).
                Where(a=>a.Application.ContractorId == contractorId).ToList();

            var contractor = _unitOfWork.Context.Set<ServiceContract>().
                Include(a => a.Application).
                Where(a => a.Application.ContractorId == contractorId ).ToList();
            var deed = _unitOfWork.Context.Set<ServiceDeed>().
                Include(a=>a.Application).
                Where(a => a.Application.ContractorId == contractorId).ToList();


            SrvStatisticsDto srvStatisticsDto = new SrvStatisticsDto()
            {
                Application = new Srv.Doc.SrvApplicationService.Dtos.ApplicationDto()
                {
                    ApplicationCount = application.Where(a => a.IsFree == false).Count(),
                    FreeApplicationCount = application.Where(a => a.IsFree == true).Count(),
                    RejectApplicationCount = application.Where(a=>a.Application.StatusId == StatusIdConst.REJECTED).Count()
                },
                Contract = new Contract()
                {
                    ContractCount = contractor.Count(),
                    FreeContractnCount = 0,
                    RejectContractCount = contractor.Where(a=>a.Application.StatusId == StatusIdConst.REJECTED).Count()

                },
                Deed = new Deed()
                {
                    DeedCount = deed.Count(),
                    FreeDeedCount = 0,
                    RejectDeedCount = deed.Where(a=>a.Application.StatusId == StatusIdConst.REJECTED).Count()
                }
            };


           return srvStatisticsDto;


        }
	}
}