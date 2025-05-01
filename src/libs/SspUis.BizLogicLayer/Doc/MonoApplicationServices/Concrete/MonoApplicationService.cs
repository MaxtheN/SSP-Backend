using iText.Kernel.Crypto.Securityhandler;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.Doc;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using SspUis.Integration.Bandlik.Models;
using SspUis.Integration.Bandlik.Services;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    public class MonoApplicationService : BaseApplicationService
        <MonoApplication,
        MonoApplicationListDto,
        MonoApplicationDto,
        CreateMonoApplicationDlDto,
        UpdateMonoApplicationDlDto,
        IMonoApplicationRepository,
        MonoApplicationSortFilterOptions>, IMonoApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly INumberService _numberService;
        private readonly IEImzoService _eImzoService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IBaseReportService _baseReportService;
        private readonly SystemConf _systemConf;
        private readonly IMemshipContractRepository _memshipContractRepository;
        private readonly DbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiRequestLogRepository _apiRequestLogRepository;
        private readonly IStorageService _storageService;
        private readonly IConvertService _pdfConverter;
        private readonly IBandlikService _bandlikService;

        public MonoApplicationService(IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IBaseReportService baseReportService,
            SystemConf systemConf,
            IEImzoService eImzoService,
            IMemshipContractRepository memshipContractRepository,
            DbContext context,
            IHttpContextAccessor httpContextAccessor,
            IApiRequestLogRepository apiRequestLogRepository,
            IStorageService storageService,
            IBandlikService bandlikService,
            IConvertService pdfConverter)
            : base(unitOfWork, documentChangeLogService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _numberService = numberService;
            _documentChangeLogService = documentChangeLogService;
            _baseReportService = baseReportService;
            _systemConf = systemConf;
            _eImzoService = eImzoService;
            _memshipContractRepository = memshipContractRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _apiRequestLogRepository = apiRequestLogRepository;
            _storageService = storageService;
            _bandlikService = bandlikService;
            this._pdfConverter = pdfConverter;
        }

        public override PagedResult<MonoApplicationListDto> GetList(MonoApplicationSortFilterOptions options)
        {
            //return base.GetList(options);

            var t = GetQuery<MonoApplicationListDto>();

            return t.SortFilter(options)
             .AsPagedResult(options);
        }

        public int GetCount()
        {
            MonoApplicationSortFilterOptions monoApplicationSortFilterOptions  = new MonoApplicationSortFilterOptions();
            return GetList(monoApplicationSortFilterOptions).Rows.Count();
		}
        protected override IQueryable<MonoApplicationListDto> SortFilter(
            IQueryable<MonoApplicationListDto> query, MonoApplicationSortFilterOptions options)
        {
            return base.SortFilter(query, options)
                .SortFilter(options)
                .Where(a => a.Application.ApplicationTypeId == ApplicationTypeIdConst.MONO);
        }

        private IQueryable<TDto> GetQuery<TDto>() where TDto : class, IMonoApplicationListModel, IHaveIdProp<long>
        {
            var list = Repository.ReadAsNoTracked<TDto>().ToList();

            // Null checks for lists and dictionaries
            var entity = _unitOfWork.Context.MonoApplications?.ToList();
            List<MonoApplicationBandlikStatus> bandlikStatus = _unitOfWork.Context.MonoApplicationIntegrationStatuses?.ToList();
            var bandlikres = _unitOfWork.Context.MonoApplicationBandlikResults?.ToDictionary(br => br.ApplicationId, br => br.Status);

            foreach (var item in list)
            {
                if (bandlikres.TryGetValue(item.Id, out var status))
                {
                    MonoApplication model = new();

                    if (entity != null)
                    {
                        model = entity.FirstOrDefault(a => a.Id == item.Id);
                    }


                    if (status == 1 && model != null)
                    {
                        if (model.StatusId != StatusIdConst.ACCEPTED)
                        {
                            AcceptStatusMonoApplicationDto updateStatus = new();
                            updateStatus.Id = model.Id;
                            Accept(updateStatus);
                        }
                    }
                     if(status > 1 && model != null)
                    {
                        if (model.StatusId != StatusIdConst.REJECTED)
                        {
                            RejectStatusMonoApplicationDto updateStatus = new();
                            updateStatus.Id = model.Id;
                            Reject(updateStatus);
                        }
                    }

                    item.BandlikResponseStatusId = status;
                    if (ServiceProvider.CultureHelper.CurrentCulture.Id == 2)
                        item.BandlikResponseStatus = status == 1 ? "Тасдиқланган" : "Рад этилган";
                     if (ServiceProvider.CultureHelper.CurrentCulture.Id == 3)
                        item.BandlikResponseStatus = status == 1 ? "Tasdiqlangan" : "Rad etilgan";
                    else
                        item.BandlikResponseStatus = bandlikStatus.FirstOrDefault(x => x.Id == status)?.FullName;
                }
            }
            return list.AsQueryable();
        }

        public override MonoApplicationDto Get()
        {
            var region = _unitOfWork.RegionRepository.ById<RegionListDto>(_authService.Contractor.RegionId);

            var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(_authService.Contractor.DistrictId);

            var contractor = _unitOfWork.ContractorRepository.ById<ContractorListDto>(_authService.Contractor.Id);

            return new MonoApplicationDto
            {
                Application = new()
                {
                    Id2 = Guid.NewGuid(),
                    Contractor = _authService.Contractor.FullName,
                    ContractorInn = _authService.Contractor.Inn,
                    DocOn = DateTime.Now.AsDateOnly(),
                    ContractorId = _authService.Contractor.Id,
                    ContractorPositionName = "Директор",
                    ContractorAddress = contractor.Address,
                    ContractorDirector = contractor.Director,
                    RegionId = _authService.Contractor.RegionId,
                    Region = region.FullName,
                    DistrictId = _authService.Contractor.DistrictId,
                    District = district.FullName,
                    ApplicationTypeId = ApplicationTypeIdConst.MONO,
                    DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                },
                StudentTables = new(),
                ItemTables = new(),
                Files = new(),
                CanEdit = true,
            };
        }

        public override MonoApplicationDto Get(long id)
        {
            var dto = Repository.ById<MonoApplicationDto>(id);

            if (dto == null)
                return null;

            if (_authService.Contractor != null)
            {
                dto.CanEdit = StatusIdConst.CanMonoApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.MODIFIED);
                dto.CanSend = StatusIdConst.CanMonoApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.SENT);
                dto.CanRevoke = StatusIdConst.CanMonoApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REVOKED);
            }
            else
            {
                //dto.CanAccept = StatusIdConst.CanMonoApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED)
                //                    && _authService.HasPermission(ModuleCode.MonoApplicationAccept);
                //dto.CanReject = StatusIdConst.CanMonoApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED)
                //                    && _authService.HasPermission(ModuleCode.MonoApplicationReject);
                //dto.CanCancel = StatusIdConst.CanMonoApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.CANCELED)
                //                    && _authService.HasPermission(ModuleCode.MonoApplicationCancel);
            }
            return dto;
        }

        public MonoApplicationDto Get(Guid id2)
        {
            var dto = Repository.ReadAsNoTracked<MonoApplicationDto>()
                .FirstOrDefault(a => a.Application.Id2 == id2 && new int[] { StatusIdConst.SENT, StatusIdConst.ACCEPTED }.Contains(a.Application.StatusId));
            if (dto == null)
                AddError("Ariza topilmadi / Заявление не найдено!");
            return dto;
        }

        public async Task<HaveId<long>> Create(CreateMonoApplicationDlDto dto)
        {
            string[] Names = new string[] { "deed", "confdoc", "auditoriesphotos", "depschema" };
            bool validationFiles = Names.All(name => dto.Files.Any(file => file.ColumnName == name));
            if (!validationFiles)
            {
                AddError("Barcha fayllar yuklanmagan");
                return null;
            }

            var groupedfiles = dto.Files.
                        GroupBy(a => new { a.ColumnName }).
                        Select(t => new { t.Key.ColumnName, FileName = t.ToList(), ids = t.ToList() }).ToList();

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {

                    foreach (var item in groupedfiles)
                    {
                        List<InMemoryFile> files = new List<InMemoryFile>();
                        for (int i = 0; i < item.ids.Count(); i++)
                        {
                            StorageFile storagefiles = _storageService.GetTempFile(DocumentStorageConst.DOC_MONO_APPLICATION_FILES, item.ids[i].Id);
                            Stream stream = storagefiles.GetStream();
                            files.Add(new InMemoryFile
                            {
                                FileName = Path.GetFileName($"{item.ids[i].FileName}"),
                                Content = await ConvertStreamToByteArrayAsync(stream)
                            });
                        }
                        byte[] zipArchive = GetZipArchive(files);

                        Stream fileStream = new MemoryStream(zipArchive);
                        StorageFile storageFile = new StorageFile($"{item.ColumnName}.zip", fileStream);

                        IEnumerable<MonoApplicationFileDto> uploadFiles = UploadFiles(storageFile);
                        var subUploadFile = uploadFiles.FirstOrDefault();
                        subUploadFile.ColumnName = $"{item.ColumnName}_zip";

                        dto.Files.Add(subUploadFile);
                    }


                    MonoValidation(dto, null);
                    foreach (MonoApplicationStudentTableDlDto item in dto.StudentTables)
                    {
                        Person person = _unitOfWork.PersonRepository.ByPinfl(item.PersonInfo.Pinfl);
                        if (person == null)
                        {
                            person = _unitOfWork.PersonRepository.Create(item.PersonInfo);
                            CombineStatuses(_unitOfWork.PersonRepository);
                            if (HasErrors)
                                return null;
                            _unitOfWork.Save();
                        };
                        item.PersonId = person.Id;
                    }

                    MonoApplication entity = Repository.Create(dto);
                    CombineStatuses(Repository);
                    if (HasErrors) return null;

                    UnitOfWork.Save();

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }



                    _storageService.MoveToPersistent(DocumentStorageConst.DOC_MONO_APPLICATION_FILES, entity.Id.ToString(), entity.Files.Select(x => x.Id).ToArray());
                    CombineStatuses(Repository);
                    var res = CreateDocumentChangeLog(entity.Id);
                    if (IsValid)
                    {
                        transaction.Commit();
                        return HaveId.Create(entity.Id);
                    }



                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    AddError($"{ex.Message} : {ex.InnerException}");
                }
            }
            return null;
        }

        public async Task<byte[]> ConvertStreamToByteArrayAsync(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // Ensure the stream is at the beginning
            stream.Position = 0;

            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);

            return buffer;
        }

        public class InMemoryFile
        {
            public string FileName { get; set; }
            public byte[] Content { get; set; }
        }

        public static byte[] GetZipArchive(List<InMemoryFile> files)
        {
            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new System.IO.Compression.ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)


                    { var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest); using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(file.Content, 0, file.Content.Length); }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }
        public static byte[] GetZipArchiveFromStreams(List<Stream> streams)
        {
            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new System.IO.Compression.ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < streams.Count; i++)
                    {
                        var stream = streams[i];
                        var fileName = $"file_{i + 1}.bin";  // Generate unique names for each stream (e.g., file_1.bin, file_2.bin)

                        var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                        {
                            stream.CopyTo(zipStream); // Copy the stream content into the zip entry
                        }
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }

        public bool CanCreate(string inn = null)
        {
            inn ??= _unitOfWork.Context.Set<PrtnCertificate>().FirstOrDefault(a => a.ContractorInn == _authService.Contractor.Inn && a.StatusId == StatusIdConst.FORMED)?.ContractorInn;

            var dto = Repository.CrudServices.ReadManyNoTracked<MonoApplicationDto>()
                        .Where(a => a.Application.ContractorInn == inn
                                && !new int[] { StatusIdConst.DELETED, StatusIdConst.REJECTED, StatusIdConst.CANCELED }.Contains(a.Application.StatusId))
                        .ToList();

            if (dto.Any()) return true;
            return false;
        }

        public override void Update(UpdateMonoApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = Repository.Update(dto, ent =>
                {
                    if (!StatusIdConst.CanEditStatuses.Contains(ent.Application.StatusId))
                        AddError("Tahrirlash mumkin emas / Невозможно редактировать");
                });

                CombineStatuses(Repository);

                _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_MONO_APPLICATION_FILES, $"{dto.Id}");

                if (IsValid)
                    UnitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);

                if (IsValid) transaction.Commit();
            }
        }

        public async Task Send(SendStatusMonoApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

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

            var timeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : _authService.Contractor?.Inn,
                Pinfl = dto.IsPinfl ? _authService.User.Pinfl : null
            });

            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            if (_authService.Contractor != null)
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                    Pinfl = _authService.Contractor.Pinfl
                });
            }
            else
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = null,
                    Pinfl = _authService.User.Pinfl
                });
            }

            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            dto.SignFile = SaveFile(doc.Id, timeStamp.Pkcs7b64, "sign.txt");
            dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            try
            {
                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanMonoApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
                });

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

        private Guid SaveFile(long docId, string data, string fileName)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;
            var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_MONO_APPLICATION)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

            return fileInfo.FirstOrDefault().FileId;
        }

        public void Reject(RejectStatusMonoApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);
                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanMonoApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
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

        public void Accept(AcceptStatusMonoApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<MonoApplication>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanMonoApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

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

        public void Cancel(CancelStatusMonoApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<MonoApplication>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanMonoApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
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

        public async Task PostApplication(BandlikRequestMonoPostDto dto, long? id = null)
        {
            var log = new CreateApiRequestLogDlDto();

            try
            {
                log.DocumentId = id;
                log.TableId = TableIdConst.DOC_MONO_APPLICATION;
                log.UserId = (int)_authService.UserId;
                log.UserInfo = _authService.User.ToString() ?? "";
                log.RequestAt = DateTime.Now;

                var result = await _bandlikService.PostMonoApplication(dto);

                CombineStatuses(_bandlikService);

                log.IsSuccess = IsValid;
                log.ResponseAt = DateTime.Now;
                log.RequestContent = JsonConvert.SerializeObject(dto);
                log.ResponseStatus = _bandlikService.IsValid ? 200 : 500;
                log.RequestUrl = result.Item1;
                log.ResponseContent = JsonConvert.SerializeObject(result.Item2);
                log.Exception = _bandlikService.GetAllErrors();
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
        }

        public async Task SentForReview(long id, string userIp = null, string userAgent = null)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Repository.AllAsQueryable.Lock(id);


                    var entity = _unitOfWork.Context.MonoApplications
                        .Include(a => a.MonoMfy)
                        .Include(a => a.StudentTables)
                            .ThenInclude(a => a.Person)
                        .Include(a => a.ItemTables)
                            .ThenInclude(a => a.EducationItem)
                        .Include(a => a.Files)
                        .Include(a => a.Application)
                        .FirstOrDefault(a => a.Id == id);

                    if (entity == null)
                    {
                        AddError("Document not found error");
                        return;
                    }

                    if (IsValid)
                    {
                        try
                        {
                            if (!_systemConf.IsTest)
                            {
                                var contractor = _unitOfWork.Context.Set<Contractor>()
                                    .Include(a => a.SettlementAccounts)
                                    .Include(a => a.BusinessmanUserInContractors)
                                        .ThenInclude(b => b.BusinessmanUser)
                                    .Include(a => a.District)
                                    .FirstOrDefault(a => a.Id == entity.Application.ContractorId);

                                var monoApplicationStudents = entity.StudentTables;
                                var monoApplicationEduItems = entity.ItemTables;
                                var monoApplicationEduFiles = entity.Files;

                                var monoConstructionData = new BandlikRequestMonoPostItemDto
                                {
                                    TotalCost = Convert.ToInt32(entity.TotalCost),
                                    DepSchemaUrl = _systemConf.QrImagePrintPath + "/MonoApplication/DownloadPdf?Id2=" + (monoApplicationEduFiles.FirstOrDefault(a => a.ColumnName == "depschema_zip")?.Id.ToString() ?? "") + "&lang=uz-cyrl",
                                    AuditoriesPhotoUrl = _systemConf.QrImagePrintPath + "/MonoApplication/DownloadPdf?Id2=" + (monoApplicationEduFiles.FirstOrDefault(a => a.ColumnName == "auditoriesphotos_zip")?.Id.ToString() ?? " ") + "&lang=uz-cyrl",
                                    ConfDocUrl = _systemConf.QrImagePrintPath + "/MonoApplication/DownloadPdf?Id2=" + (monoApplicationEduFiles.FirstOrDefault(a => a.ColumnName == "confdoc_zip")?.Id.ToString() ?? " ") + "&lang=uz-cyrl",
                                    DeedUrl = _systemConf.QrImagePrintPath + "/MonoApplication/DownloadPdf?Id2=" + (monoApplicationEduFiles.FirstOrDefault(a => a.ColumnName == "deed_zip")?.Id.ToString() ?? " ") + "&lang=uz-cyrl",
                                };
                                var monoEquipmentData = new BandlikRequestMonoPostItem2Dto
                                {
                                    ConfDocUrl = _systemConf.QrImagePrintPath + "/MonoApplication/DownloadPdf?Id2=" + (monoApplicationEduFiles.FirstOrDefault(a => a.ColumnName == "confdoc_zip")?.Id.ToString() ?? " ") + "&lang=uz-cyrl",
                                    Equipments = monoApplicationEduItems.Select(pag4 => new BandlikRequestMonoPostEquipmentsDto
                                    {
                                        Name = pag4.EducationItem.FullName,
                                        Amount = Convert.ToInt32(pag4.EducationItemCount),
                                        TotalCost = Convert.ToInt32(pag4.EducationItemAmount)
                                    }).ToList(),
                                };
                                await PostApplication(new BandlikRequestMonoPostDto
                                {
                                    TypeId = 1,
                                    ApplicationId = entity.Application.Id,
                                    ContractorInn = Convert.ToInt32(contractor.Inn),
                                    ContractorName = contractor.FullName,
                                    ContractorAdress = contractor.Address,
                                    ContractorSoato = Convert.ToInt32(contractor.District.Soato),
                                    ContractorPhone = contractor.BusinessmanUserInContractors.FirstOrDefault()?.BusinessmanUser.UserName ?? " ",
                                    ContractorDirector = contractor.Director ?? " ",
                                    ContractorFax = contractor.BusinessmanUserInContractors.FirstOrDefault()?.BusinessmanUser.UserName ?? " ",
                                    ContractorEmail = contractor.BusinessmanUserInContractors.FirstOrDefault()?.BusinessmanUser.Email ?? " ",
                                    RegistrationNumber = contractor.RegistrationNumber ?? "A17",
                                    RegistrationDate = contractor.RegistrationDate.ToString("yyyy-MM-dd"),
                                    RegistrationFrom = contractor.SooguRegistrator ?? "Bandlik M",
                                    BankAccount = contractor.SettlementAccounts.FirstOrDefault()?.AccountCode ?? " ",
                                    MonoMfyId = Convert.ToInt32(entity.MonoMfy.ExternalId),
                                    MonoAdress = entity.MonoAdress,
                                    DepartmentCount = entity.BuildingCount,
                                    TotalArea = entity.TotalArea.ToString(),
                                    EduArea = entity.LearningArea.ToString(),
                                    Item2s = monoEquipmentData,
                                    Studens = monoApplicationStudents.Any() ?
                                        monoApplicationStudents.Select(pag1 => new BandlikRequestMonoPostStudentDto
                                        {

                                            Passport = pag1.Person.PassportSeria + "" + pag1.Person.PassportNumber,
                                            Pinfl = long.Parse(pag1.Person.Pinfl),
                                            FullName = pag1.Person.FullName
                                        }).ToList() :
                                        new(),
                                    Items = monoConstructionData,
                                }, entity.Id);

                                if (HasErrors)
                                {
                                    entity.Message = GetAllErrors();
                                    _unitOfWork.Save();
                                    return;
                                }

                                if (IsValid && !HasErrors)
                                {
                                    entity.StatusId = StatusIdConst.SENT_FOR_REVIEW;

                                    _unitOfWork.Save();

                                    CombineStatuses(Repository);

                                    if (IsValid)
                                        transaction.Commit();
                                }
                            }
                            else
                            {
                                entity.StatusId = StatusIdConst.SENT;
                                CombineStatuses(Repository);
                                if (IsValid)
                                    transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            entity.StatusId = StatusIdConst.FAILED;
                            entity.Message = $"{ex.Source}: {ex.Message}. {ex.InnerException}";
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message}: {ex.InnerException}");
                }
            }
        }

        private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = Repository.ById<MonoApplicationDto>(id, applyFilter: false);
            _documentChangeLogService.CreateApplication(
                dto: entityDto,
                organizationId: null,
                message: message);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }

        private void MonoValidation<TDto>(MonoApplicationDlDto<TDto> dto, MonoApplication entity)
            where TDto : MonoApplicationDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            var emp = query.Include(a => a.StudentTables).ThenInclude(a => a.Person).FirstOrDefault(a => a.StudentTables.FirstOrDefault().PersonId == dto.StudentTables.FirstOrDefault().PersonId);
            if (emp != null)
                AddError($"Это сотрудник уже существует.", $"{emp.StudentTables.FirstOrDefault().Person.PassportSeria}{emp.StudentTables.FirstOrDefault().Person.PassportNumber}");
        }
        public async ValueTask<byte[]> DownloadPdf(Guid id2, string? lang)
        {
            lang = lang ?? "uz-cyrl";

            var languageId = UnitOfWork.Context.Set<Language>()
                .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;

            MemoryStream wordFile = new MemoryStream();
            if (id2 == Guid.Empty)
            {
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                        ServiceProvider.CultureHelper.CurrentCulture.Code,
                        StaticFileConst.WordTemplate.CLAIM_APPLICATION_ADMINISTRATIVE_COURT)
                    );
                return await _pdfConverter.DocxToPdfAsync(wordFile, new object());
            }

            var claimApplication = await _unitOfWork.Context
                .Set<ClaimApplication>()
                .Include(c => c.Application)
                .Include(c => c.ClaimApplicationType)
                .Include(c => c.Currency)
                .FirstOrDefaultAsync(c => c.Application.Id2 == id2);

            if (claimApplication == null)
            {
                AddError("Ariza topilmadi / Заявление не найдено!");
                return null;
            }

            var tables = _unitOfWork.Context.Set<ClaimApplicationTable>()
                 .Include(t => t.ClaimResponsibleType)
                 .ThenInclude(t => t.Translates)
                 .Where(t => t.OwnerId == claimApplication.Id);


            if (claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APPLICATION_FOR_COURT)
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                        ServiceProvider.CultureHelper.CurrentCulture.Code,
                        StaticFileConst.WordTemplate.CLAIM_APPLICATION_ADMINISTRATIVE_COURT)
                    );

            else if (claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_APPLICATION || claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.COUNTER_CLAIM)
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                        ServiceProvider.CultureHelper.CurrentCulture.Code,
                        StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION)
                    );

            else if (claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION || claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION)
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                        ServiceProvider.CultureHelper.CurrentCulture.Code,
                        StaticFileConst.WordTemplate.CLAIM_APPLICATION_APILATION_OR_CASSATION)
                    );
            if (wordFile == null)
            {
                AddError("Undefined document Type.");
                return null;
            }

            var contractor = await UnitOfWork.Context.Set<DataLayer.EfClasses.Contractor>()
                .Include(c => c.SettlementAccounts)
                .FirstOrDefaultAsync(c => c.Id == claimApplication.Application.ContractorId);

            var region = await UnitOfWork.Context.Set<Region>()
                .Include(r => r.Translates)
                .FirstOrDefaultAsync(r => r.Id == contractor.RegionId);

            var district = await UnitOfWork.Context.Set<District>()
                .Include(d => d.Translates)
                .FirstOrDefaultAsync(d => d.Id == contractor.DistrictId);

            var bank = await UnitOfWork.Context.Set<DataLayer.EfClasses.Bank>()
                .Include(b => b.Translates)
                .FirstOrDefaultAsync(b => b.Id == contractor.BankId);

            var theme = await UnitOfWork.Context.Set<ClaimTheme>()
                .Include(r => r.Translates)
                .FirstOrDefaultAsync(t => t.Id == claimApplication.ClaimThemeId);

            var applicationForCourt = await UnitOfWork.Context.Set<ApplicationForCourt>()
                .Include(ac => ac.ClaimOrganization)
                .Include(afc => afc.Mediation)
                .ThenInclude(m => m.MediationPlan)
                .ThenInclude(mp => mp.Application)
                .ThenInclude(a => a.ClaimApplication)
                .FirstOrDefaultAsync(ac => claimApplication.Id == ac.Mediation.MediationPlan.Application.ClaimApplication.Id);

            var plh = new Placeholders();
            var link = _systemConf.QrImagePrintMy + "/ClaimApplication/DownloadPdf?id2=" + claimApplication.Application.Id2.ToString();
            var qrCode = new MemoryStream(QRCodeHelper.GeneratePng(link));
            plh.ImagePlaceholders.Add("QrCode", new() { Dpi = 512, MemStream = qrCode });

            plh.TextPlaceholders.Add(nameof(contractor.Region), region.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? region.FullName ?? "");

            plh.TextPlaceholders.Add(nameof(contractor.District), district.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? district.FullName ?? "");

            plh.TextPlaceholders.Add(nameof(contractor.Bank), bank.Translates
                    .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == BankTranslateColumn.bank_name.ToString()
                    )?.TranslateText ?? bank.BankName
                    + "  " + contractor.SettlementAccounts.FirstOrDefault(a => a.IsMain)?.AccountCode ?? "");

            plh.TextPlaceholders.Add(nameof(contractor.FullName), contractor.FullName ?? "");
            plh.TextPlaceholders.Add(nameof(contractor.Address), contractor.Address ?? "");
            plh.TextPlaceholders.Add(nameof(contractor.Inn), contractor.Inn ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Application.DocNumber), claimApplication.Application.DocNumber ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Application.DocOn), claimApplication.Application.DocOn.ToString("dd.MM.yyyy") ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.ClaimTheme),
                theme.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? theme.FullName ?? "");
            plh.TextPlaceholders.Add("CourtAppData", applicationForCourt?.DocOn.ToString("dd.MM.yyyy") ?? "");
            plh.TextPlaceholders.Add("CourtDocNumber", applicationForCourt?.DocNumber ?? "");
            plh.TextPlaceholders.Add("ClaimAppTypeName",
                claimApplication.ClaimApplicationType.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? claimApplication.ClaimApplicationType.FullName ?? "");

            plh.TextPlaceholders.Add(nameof(applicationForCourt.ClaimOrganization),
                applicationForCourt?.ClaimOrganization.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? applicationForCourt?.ClaimOrganization.FullName ?? "");

            plh.TextPlaceholders.Add(nameof(claimApplication.Tables), String.Join(", ", tables.Select(t => t.FullName)));

            var items = new List<Placeholders>();

            foreach (var item in tables)
            {
                var lplh = new Placeholders();
                lplh.TextPlaceholders.Add("ResponsibleName", item.FullName ?? "");
                lplh.TextPlaceholders.Add("ResponsibleAddress", item.Address ?? "");
                lplh.TextPlaceholders.Add("ResponsiblePhone", item.PhoneNumber ?? "");
                lplh.TextPlaceholders.Add("ResponsibleInn", item.InnOrPinfl ?? "");
                items.Add(lplh);
            }
            plh.TemplateListPlaceholders.Add(nameof(claimApplication.Tables), items);
            plh.TextPlaceholders.Add(nameof(claimApplication.TotalAmount), claimApplication.TotalAmount?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.MainDebt), claimApplication.MainDebt?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.CalculedPenalty), claimApplication.CalculedPenalty?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Penalty), claimApplication.Penalty?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Percent), claimApplication.Percent?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Currency), claimApplication?.Currency.FullName ?? "");
            plh.TextPlaceholders.Add("ContractorDirector", contractor.Director);

            wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
            return await _pdfConverter.DocxToPdfAsync(wordFile, new());
        }

        #region Files

        public void Test()
        {
            var dbData = _unitOfWork.Context.Set<MonoApplicationFile>()
                    .Include(a => a.Owner)
                    .Where(a => a.OwnerId == 110);

            List<StorageFile> unZipfiles = new List<StorageFile>();
            foreach (var item in dbData)
            {
                var data = _storageService.GetFile(DocumentStorageConst.DOC_MONO_APPLICATION_FILES, 110.ToString(), item.Id);

                if (IsValid)
                    data.FileName = item.FileName;

                unZipfiles.Add(data);
            }
        }


        public IEnumerable<MonoApplicationFileDto> UploadFiles(params StorageFile[] files)
        {
            var result = _storageService.SaveTemp(DocumentStorageConst.DOC_MONO_APPLICATION_FILES, files).Select(a => new MonoApplicationFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                CreatedAt = DateTime.Now
            });
            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }
        public StorageFile DownloadFile(Guid fileId)
        {
            var entity = _unitOfWork.Context.Set<MonoApplicationFile>()
                .Include(a => a.Owner)
                .FirstOrDefault(a => a.Id == fileId);


            return Download(fileId, entity, DocumentStorageConst.DOC_MONO_APPLICATION_FILES);
        }
        public void DeleteFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<MonoApplicationFile>()
                .FirstOrDefault(a => a.Id == fileId);

            Delete(fileId, entity, DocumentStorageConst.DOC_MONO_APPLICATION_FILES);
        }
        private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
        {
            if (entity != null)
            {
                _storageService.DeleteTemp(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
        }
        private StorageFile Download(Guid fileId, MonoApplicationFile entity, string storageDocument)
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


		#endregion
	}
}
