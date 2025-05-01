using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.JoinAntiCorruptionResultServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Corruption;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Corruption
{
    public class JoinAntiCorruptionResultService : BaseEntityService<long, JoinAntiCorruptionResult, JoinAntiCorruptionResultListDto, JoinAntiCorruptionResultDto, CreateJoinAntiCorruptionResultDlDto, UpdateJoinAntiCorruptionResultDlDto, IJoinAntiCorruptionResultRepository, JoinAntiCorruptionResultSortFilterOptions>
        , IJoinAntiCorruptionResultService
    {
        private readonly IJoinAntiCorruptionResultRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly INumberService _numberService;
        private readonly IStorageService _storageService;
        private readonly IEImzoService _eImzoService;
        private readonly IConvertService _pdfConverter;

        public JoinAntiCorruptionResultService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IStorageService storageService,
            IEImzoService eImzoService,
            IConvertService pdfConverter) : base(unitOfWork)
        {
            this._repository = unitOfWork.JoinAntiCorruptionResultRepository;
            this._unitOfWork = unitOfWork;
            this._authService = authService;
            this._documentChangeLogService = documentChangeLogService;
            this._numberService = numberService;
            _storageService = storageService;
            _eImzoService = eImzoService;
            this._pdfConverter = pdfConverter;
        }

        public PagedResult<JoinAntiCorruptionResultListDto> GetList(JoinAntiCorruptionResultSortFilterOptions dto)
        {
            var result = _repository.ReadAsNoTracked<JoinAntiCorruptionResultListDto>()
                                    .SortFilter(dto)
                                    .AsPagedResult(dto);
            return result;
        }

        public JoinAntiCorruptionResultDto Get()
        {
            return new JoinAntiCorruptionResultDto()
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext(
                    nameof(TableIdConst.CORRUPTION__DOC_JOIN_ANTI_CORRUPTION_RESULT),
                    organizationId: _authService.IsAuthenticated ? _authService.Organization.Id : OrganizationIdConst.SSP)
                    .Item2,
            };
        }

        public JoinAntiCorruptionResultDto Get(long id)
        {
            var dto = _repository.ById<JoinAntiCorruptionResultDto>(id);
            CombineStatuses(_repository);
            if (IsValid)
            {
                dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.JoinAntiCorruptionResultEdit);
                dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.JoinAntiCorruptionResultAccept);
                dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.JoinAntiCorruptionResultCancel);
                dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.JoinAntiCorruptionResultDelete);
            }
            return dto;
        }

        public SelectList<long> AsSelectList()
        {
            return _repository.AllAsQueryable
                            .AsSelectList();
        }

        public HaveId<long> Create(CreateJoinAntiCorruptionResultDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.Create(dto, ent => Validation(dto, ent));
                    CombineStatuses(_repository);
                    if (HasErrors) return null;

                    _unitOfWork.Save();

                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);

                    if (IsValid)
                    {
                        transaction.Commit();
                        return res;
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} // {ex.InnerException}");
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
            return null;
        }

        public void Update(UpdateJoinAntiCorruptionResultDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = _repository.Update(dto, ent => Validation(dto, ent));
                CombineStatuses(_repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
                if (IsValid)
                    transaction.Commit();
            }
        }

        public void Delete(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.ById(id);
					if (entity == null)
					{
						AddError("Ma'lumot topilmadi!");
						transaction.Rollback();
						return;
					}
					else if (entity.StatusId == StatusIdConst.ACCEPTED)
					{
						AddError("O'chirish mumkin emas !");
						transaction.Rollback();
						return;
					}
					entity.StatusId = StatusIdConst.DELETED;
                    CombineStatuses(_repository);
                    var dto = new UpdateStatusJoinAntiCorruptionResultDlDto { Id = id, StatusId = StatusIdConst.DELETED };

                    Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanApplyAntiCorruptionResultStatus(ent.StatusId, dto.StatusId))
                            _repository.AddError("Нет доступа");
                    });

                    if (HasErrors) return;

                    if (IsValid)
                    {
                        _unitOfWork.Save();
                        var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteJoinAntiCorruptionResult");

                        if (IsValid)
                            transaction.Commit();
                    }
                }
                catch (DbUpdateException)
                {
                    AddError("Запись не может быть удален");
                    transaction.Rollback();
                }
            }
        }

        private HaveId<long> UpdateStatus(UpdateStatusJoinAntiCorruptionResultDlDto dto, Action<JoinAntiCorruptionResult> validation)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
            try
            {
                var entity = Repository.UpdateStatus(dto, validation);
                CombineStatuses(_repository);
                if (HasErrors) return null;

                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "JoinAntiCorruptionResult");

                if (IsValid && canCommit)
                    transaction.Commit();

                return res;
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public async Task<HaveId<long>> Accept(UpdateStatusJoinAntiCorruptionResultDto dto)
        {
            var query = _repository.AllAsQueryable;

            var entityForChek = _unitOfWork.Context.Set<JoinAntiCorruptionResult>().FirstOrDefault(a => a.Id == dto.Id);

            foreach (var table in entityForChek.Tables)
            {
                if (query.Any(a => a.Tables.Any(a => a.ApplicationId == table.ApplicationId && a.JoinAntiCorruptionResultTypeId == JoinAntiCorruptionResultTypeIdConst.CERTIFICATE_ACCEPTED)))
                {
                    var application = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>().Include(a => a.Owner).FirstOrDefault(a => a.ApplicationId == table.ApplicationId);
                    _repository.AddError($"Ushbu ariza {application.Owner.DocNumber} raqamli hujjatda sertifikat berilgan");
                }
                else if (query.Any(a => a.Tables.Any(a => a.ApplicationId == table.ApplicationId && a.JoinAntiCorruptionResultTypeId == JoinAntiCorruptionResultTypeIdConst.CERTIFICATE_CANCELED)))
                {
                    var application = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>().Include(a => a.Owner).FirstOrDefault(a => a.ApplicationId == table.ApplicationId);
                    _repository.AddError($"Ushbu ariza {application.Owner.DocNumber} raqamli hujjatda sertifikat rad etilgan");
                }
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = await _unitOfWork.Context.Set<JoinAntiCorruptionResult>()
                                .Include(x => x.Signs)
                                .FirstOrDefaultAsync(x => x.Id == dto.Id);

                    if (entity == null)
                    {
                        AddError("Not found");
                        transaction.Rollback();
                        return null;
                    }

                    var members = new List<int?>
                    {
                        entity.ChairmenId,
                        entity.Member1Id,
                        entity.Member2Id,
                        entity.Member3Id,
                        entity.Member4Id
                    };

                    var memberCount = members.Count(x => x.HasValue);
                    var signedCount = entity.Signs.Count(x => x.SignedAt != null);

                    var eImzoTimstampDto = new EImzoTimeStampDto
                    {
                        SignData = dto.SignedData,
                        Inn = null,
                        Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
                    }; 
                    var timeStamp = await _eImzoService.TimeStamp(eImzoTimstampDto);

                    CombineStatuses(_eImzoService);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    };

                    var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                    {
                        SignData = timeStamp.Pkcs7b64,
                        Inn = null,
                        Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
                    });

                    var signer = new JoinAntiCorruptionResultSign()
                    {
                        SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt"),
                        DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt"),
                        SignedAt = DateTime.Now,
                        SignedUserInfo = _authService.User != null
                            ? _authService.User.ToTextForDocumentLog()
                            : _authService.Contractor.FullName + " - " + _authService.Contractor.Inn,
                        StatusId = dto.StatusId,
                    };
                    entity.Signs.Add(signer);

                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, dto.Message);

                    memberCount = memberCount - 1;
                    if(signedCount == memberCount)
                    {
                        var statusDto = new UpdateStatusJoinAntiCorruptionResultDlDto
                        {
                            Id = dto.Id,
                            StatusId = StatusIdConst.ACCEPTED
                        };

                        Repository.UpdateStatus(statusDto);

                        CombineStatuses(_repository);
                        if (HasErrors) return null;

                        _unitOfWork.Save();

                        var result = CreateDocumentChangeLog(statusDto.Id, statusDto.StatusId);

                        CreateJoinAntiCorruptionCertificateDlDto modelcha = new();

                        var corruptionResult = UnitOfWork.Context.Set<JoinAntiCorruptionResult>()
                            .Include(r => r.Tables)
                            .ThenInclude(t => t.Application)
                            .FirstOrDefault(r => r.Id == dto.Id);

                        foreach (var table in corruptionResult.Tables)
                        {
                            if (table.Application.StatusId == StatusIdConst.ACCEPTED && table.JoinAntiCorruptionResultTypeId != 2)
                            {
                                UnitOfWork.JoinAntiCorruptionCertificateRepository.Create(new()
                                {
                                    DocOn = (DateOnly)table.CorruptionCertificateOn,
                                    DocNumber = table.CorruptionCertificateNumber,
                                    ExpireOn = table.CorruptionCertificateExpireOn == null ? null : table.CorruptionCertificateExpireOn,
                                    ContractorId = table.Application.ContractorId.Value,
                                    ResultId = table.Id,
                                });
                            }
                        }

                        UnitOfWork.Save();
                        CombineStatuses(UnitOfWork.JoinAntiCorruptionCertificateRepository);
                        if (HasErrors) return null;

                        if (IsValid)
                            transaction.Commit();

                        return result;
                    }

                    _unitOfWork.Save();
                    transaction.Commit();

                    return null;
                }
                catch (Exception ex)
                {
                    AddError(ex.Message + " // " + ex.InnerException);
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public HaveId<long> Cancel(UpdateStatusJoinAntiCorruptionResultDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var statusDto = new UpdateStatusJoinAntiCorruptionResultDlDto { Id = dto.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
                var entity = _repository.UpdateStatus(statusDto);
                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();

                var result = CreateDocumentChangeLog(statusDto.Id, statusDto.StatusId, dto.Message);
                if (result != null)
                    transaction.Commit();

                return result;
            }
        }

        private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            var moveDto = _repository.ById<JoinAntiCorruptionResultDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: moveDto,
                tableId: TableIdConst.CORRUPTION__DOC_JOIN_ANTI_CORRUPTION_RESULT,
                organizationId: null,
                statusId: statusId,
                message: message);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }
        
        public IEnumerable<JoinAntiCorruptionResultFileDto> UploadFiles(params StorageFile[] files)
        {
            var result = _storageService.SaveTemp(DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_RESULT_FILES, files).Select(a => new JoinAntiCorruptionResultFileDto
            {
                Id = a.FileId,
                FileName = a.FileName
            });
            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }

        public StorageFile DownloadFile(Guid fileId)
        {
            var entity = _unitOfWork.Context.Set<JoinAntiCorruptionResultFile>().FirstOrDefault(a => a.Id == fileId);
            return Download(fileId, entity, DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_RESULT_FILES);
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

        public void DeleteFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<JoinAntiCorruptionApplicationFile>()
                .FirstOrDefault(a => a.Id == fileId);

            Delete(fileId, entity, DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_RESULT_FILES);
        }
        
        private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
        {
            if (entity != null)
            {
                _storageService.DeleteTemp(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
        }
        
        private void Validation<TDto>(JoinAntiCorruptionResultDlDto<TDto> dto, JoinAntiCorruptionResult entity)
           where TDto : JoinAntiCorruptionResultDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (dto is UpdateJoinAntiCorruptionResultDlDto update)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                    _repository.AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
            }
            else
            { }

            if (dto.Tables.DistinctBy(x => x.ApplicationId).Count() != dto.Tables.Count())
                _repository.AddError($"Коррупцияга қарши дастурга қўшилишдаги бир хил ариза қайта такрорланмаслиги керак !");

            if (query.ByDocNumber(dto.DocNumber).Any())
                _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));
            if (dto is CreateJoinAntiCorruptionResultDlDto createDto)
            {
                foreach (var table in dto.Tables)
                {
                    if (query.Any(a => a.Tables.Any(a => a.ApplicationId == table.ApplicationId && a.JoinAntiCorruptionResultTypeId == JoinAntiCorruptionResultTypeIdConst.CERTIFICATE_ACCEPTED)))
                    {
                        var application = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>().Include(a => a.Owner).FirstOrDefault(a => a.ApplicationId == table.ApplicationId);
                        _repository.AddError($"Ushbu ariza {application.Owner.DocNumber} raqamli hujjatda sertifikat berilgan");
                    }
                    else if (query.Any(a => a.Tables.Any(a => a.ApplicationId == table.ApplicationId && a.JoinAntiCorruptionResultTypeId == JoinAntiCorruptionResultTypeIdConst.CERTIFICATE_CANCELED)))
                    {
                        var application = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>().Include(a => a.Owner).FirstOrDefault(a => a.ApplicationId == table.ApplicationId);
                        _repository.AddError($"Ushbu ariza {application.Owner.DocNumber} raqamli hujjatda sertifikat rad etilgan");
                    }
                }
            }
        }

        public async Task<byte[]> DownloadPdf(Guid id2, string lang)
        {
            var language = lang ?? "uz-latn";
            var languageId = UnitOfWork.Context.Set<Language>()
                .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;

            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(language,
                    StaticFileConst.WordTemplate.JOIN_ANTI_CORRUPTION_RESULT));

            var entity = await _unitOfWork.Context.Set<JoinAntiCorruptionResult>()
                .Include(x => x.Signs)
                .FirstOrDefaultAsync(x => x.Id2 == id2);

            var sspChairmen = await _unitOfWork.Context.Set<Employee>()
                .Where(x => x.EmployeeManage.Position.Id == PositionIdConst.CHAIRMEN)
                .Select(x => new {
                    x.Id,
                    PersonFullName = x.Person.FullName,
                    PositionFullName = x.EmployeeManage.Position.FullName,
                    Department = x.EmployeeManage.Department.FullName,
                })
                .FirstOrDefaultAsync();

            var joinAntiApplication = await _unitOfWork.Context.Set<JoinAntiCorruptionApplication>()
                .Include(x => x.Application)
                    .ThenInclude(x => x.Contractor)
                .Where(x => x.Application.StatusId == StatusIdConst.ACCEPTED)
                .ToListAsync();

            string? acceptApplicationCount = joinAntiApplication.Count.ToString();

            var plh = new Placeholders();

            plh.TextPlaceholders.Add(nameof(entity.ChairmenFio), entity.ChairmenFio);
            plh.TextPlaceholders.Add(nameof(entity.DocNumber), entity.DocNumber);
            plh.TextPlaceholders.Add(nameof(entity.DocOn), entity.DocOn.ToString());
            
            plh.TextPlaceholders.Add(nameof(sspChairmen.Department), sspChairmen.Department + " " + sspChairmen.PositionFullName);
            plh.TextPlaceholders.Add(nameof(sspChairmen.PersonFullName), sspChairmen.PersonFullName);

            plh.TextPlaceholders.Add(nameof(entity.Member1Fio), entity.Member1Fio);
            plh.TextPlaceholders.Add(nameof(entity.Member2Fio), entity.Member2Fio);
            plh.TextPlaceholders.Add(nameof(entity.Member3Fio), entity.Member3Fio ?? "");
            
            plh.TextPlaceholders.Add(nameof(acceptApplicationCount), acceptApplicationCount ?? "");

            if (joinAntiApplication.Count > 0 && joinAntiApplication != null)
            {
                plh.TablePlaceholders.Add(new Dictionary<string, List<string>>
                {
                    { "Number", joinAntiApplication.Select((x, index) => (index + 1).ToString()).ToList() },
                    { "ContractorFullName", joinAntiApplication.Select(x => x.Application.Contractor?.FullName + x.Application.Contractor.Inn ?? "").ToList() },
                    { "Voted", joinAntiApplication.Select(x => x.Application.Contractor?.FullName + x.Application.Contractor.Inn ?? "-").ToList() },
                    { "Opponents", joinAntiApplication.Select(x => x.Application.Contractor?.FullName + x.Application.Contractor.Inn ?? "-").ToList() },
                });
            }

            wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();

            var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
            CombineStatuses(_pdfConverter);
            return res;
        }

        public Guid SaveFile(long docId, string data, string fileName)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;
            var fileInfo = _storageService.Save(
                $"{nameof(TableIdConst.CORRUPTION__DOC_JOIN_ANTI_CORRUPTION_RESULT)}_SIGN_DATA",
                docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

            return fileInfo.FirstOrDefault().FileId;
        }
    }
}