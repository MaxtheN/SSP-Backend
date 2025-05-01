using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Hrm;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Handlers;
using WEBASE.OfficeTools;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer
{
    public class CandidatesConfirmationService : BaseEntityService<
             long,
             CandidatesConfirmation,
             CandidatesConfirmationListDto,
             CandidatesConfirmationDto,
             CreateCandidatesConfirmationDlDto,
             UpdateCandidatesConfirmationDlDto,
             ICandidatesConfirmationRepository,
             CandidatesConfirmationSortFilterPageOption>
        , ICandidatesConfirmationService
    {
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly ICandidatesConfirmationRepository _repository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INumberService _numberService;
        private readonly IEImzoService _eImzoService;
        private readonly IStorageService _storageService;

        public CandidatesConfirmationService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
            IEImzoService eImzoService,
            IStorageService storageService,
            IDocumentChangeLogService documentChangeLogService)
            : base(unitOfWork)
        {
            this._repository = unitOfWork.CandidatesConfirmationRepository;
            this._unitOfWork = unitOfWork;
            this._documentChangeLogService = documentChangeLogService;
            this._numberService = numberService;
            this._authService = authService;
            this._eImzoService = eImzoService;
            this._storageService = storageService;
        }

        public override PagedResult<CandidatesConfirmationListDto> GetList(CandidatesConfirmationSortFilterPageOption options)
        {
            var result = Repository.ReadAsNoTracked<CandidatesConfirmationListDto>()
                .SortFilter(options)
                .AsPagedResult(options);

            return result;
        }
        public override CandidatesConfirmationDto Get(long id)
        {
            var dto = Repository.ById<CandidatesConfirmationDto>(id);

            CombineStatuses(Repository);
            if (IsValid)
            {
                dto.CanModify = StatusIdConst.CanCandidateConfirmationStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.CandidatesConfirmationEdit);
                dto.CanAccept = StatusIdConst.CanCandidateConfirmationStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.CandidatesConfirmationAccept);
                dto.CanCancel = StatusIdConst.CanCandidateConfirmationStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.CandidatesConfirmationCancel);
                dto.CanDelete = StatusIdConst.CanCandidateConfirmationStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.CandidatesConfirmationDelete);
            }

            return dto;
        }
        public override CandidatesConfirmationDto Get()
        {
            return new CandidatesConfirmationDto
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_CONDITATES_COMFIRMED, 1).Item2
            };
        }
        public override HaveId<long> Create(CreateCandidatesConfirmationDlDto dto)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Create(dto, ent => Validation(dto, ent));
                    CombineStatuses(Repository);
                    if(HasErrors) return null;

                    if(IsValid) UnitOfWork.Save();

                    var files = entity.Tables.SelectMany(x => x.Files).Select(x => x.Id).ToArray();
                    if(files.Length > 0)
                    {
                        _storageService.MoveToPersistent(
                                DocumentStorageConst.DOC_CANDITATES_CONFIRMATION_TABLE_FILES,
                                $"{entity.Id}",
                                files);
                        CombineStatuses(_storageService);
                    }

                    if (IsValid)
                    {
                        var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CandidatesConfirmation");
                        transaction.Commit();
                        return HaveId.Create(entity.Id);
                    }
                }
                catch (DbException e)
                {
                    AddError(e.Message);
                    if (e.InnerException != null)
                        AddError(e.InnerException.Message);
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    AddError(e.Message);
                    if (e.InnerException != null)
                        AddError(e.InnerException.Message);
                    transaction.Rollback();
                }

                return null;
            }
        }
        public override void Update(UpdateCandidatesConfirmationDlDto dto)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Update(dto, ent => Validation(dto, ent));
                    CombineStatuses(Repository);
                    if (IsValid)
                    {
                        UnitOfWork.Save();
                        var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateCandidatesConfirmation");
                        transaction.Commit();
                    }
                }
                catch (DbUpdateException e)
                {
                    AddError(e.Message);
                    if (e.InnerException != null) AddError(e.InnerException.Message);
                    transaction.Rollback();
                }
                catch(Exception e)
                {
                    AddError(e.Message);
                    if (e.InnerException != null) AddError(e.InnerException.Message);
                    transaction.Rollback();
                }
            }
        }
        public void Accept(UpdateStatusCandidatesConfirmationDto dTo)
        {
            var dto = new UpdateStatusCandidatesConfirmationDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanCandidateConfirmationStatus(ent.StatusId, dto.StatusId))
                    Repository.AddError("Нет доступа");
            });
        }
        public void Cancel(CancelStatusCandidatesConfirmationDto dTo)
        {
            var dto = new UpdateStatusCandidatesConfirmationDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanCandidateConfirmationStatus(ent.StatusId, dto.StatusId))
                    Repository.AddError("Нет доступа");

                CombineStatuses(_repository);
            });
        }
        public void ReceiveTable(UpdateStatusCandidatesConfirmationTableDto dTo)
        {
            var entity = _unitOfWork.Context.Set<CandidatesConfirmationTable>().FirstOrDefault(a => a.Id == dTo.Id);

            entity.StatusId = StatusIdConst.RECEIVED;

            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CandidatesConfirmationTable");
        }
        public void Send(UpdateStatusCandidatesConfirmationDto dTo)
        {
            //var ent = _repository.UpdateStatus(dTo, ent =>
            //{
            //    if (!StatusIdConst.CanCandidateConfirmationStatus(ent.StatusId, StatusIdConst.SENT_FOR_REVIEW))
            //    {
            //        _repository.AddError("Нет доступа");
            //    }
            //});

            //CombineStatuses(_repository);
            //if (HasErrors)
            //    return;

            //UnitOfWork.Save();

            var entity = _unitOfWork.Context.Set<CandidatesConfirmation>().FirstOrDefault(a => a.Id == dTo.Id);

            entity.StatusId = StatusIdConst.SENT_FOR_REVIEW;

            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CandidatesConfirmation");
        }
        public void RejectTable(CancelStatusCandidatesConfirmationTableDto dTo)
        {
            var entity = _unitOfWork.Context.Set<CandidatesConfirmationTable>().FirstOrDefault(a => a.Id == dTo.Id);

            entity.StatusId = StatusIdConst.REJECTED;

            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CandidatesConfirmationTable");
        }
        public override void Delete(long id)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.ById(id);

                    entity.StatusId = StatusIdConst.DELETED;
                    CombineStatuses(Repository);

                    var dto = new UpdateStatusCandidatesConfirmationDlDto { Id = id, StatusId = StatusIdConst.DELETED };
                    var ent = Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanCandidateConfirmationStatus(ent.StatusId, dto.StatusId))
                            _repository.AddError("Нет доступа");
                    });

                    if (IsValid)
                    {
                        UnitOfWork.Save();
                        var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteChastisement");
                        transaction.Commit();
                    }
                }
                catch (DbUpdateException)
                {
                    AddError("Запись не может быть удален");
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    AddError($"{e.Message} - Запись не может быть удален");
                    transaction.Rollback();
                }
            }
        }

        #region HELPER
        private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            var moveDto = Repository.ById<CandidatesConfirmationDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: moveDto,
                tableId: TableIdConst.HRM__DOC_CANDIDATES_CONFIRMATION,
                organizationId: null,
                statusId: statusId,
                message: message);

            CombineStatuses(_documentChangeLogService);
            if (HasErrors) return null;

            return HaveId.Create(id);
        }
        private HaveId<long> UpdateStatus(UpdateStatusCandidatesConfirmationDlDto dto, Action<CandidatesConfirmation> validation)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.UpdateStatus(dto, validation);

                    CombineStatuses(_repository);
                    if (HasErrors) return null;

                    _unitOfWork.Save();
                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CandidatesConfirmation");

                    if (IsValid)transaction.Commit();
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
        private void Validation<TDto>(CandidatesConfirmationDlDto<TDto> dto, CandidatesConfirmation entity)
          where TDto : CandidatesConfirmationDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;
            if (dto is CreateCandidatesConfirmationDlDto)
            {
                if (query.Any(x => x.DocNumber == dto.DocNumber))
                    _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));
            }
            else
            {
                if (entity != null)
                {
                    query = query.Where(a => a.Id != entity.Id);

                    if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                        AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
                }
            }
        }
        #endregion

        #region FILES
        public StorageFile DownloadFile(Guid fileId)
        {
            StorageFile file;

            var entity = UnitOfWork.Context.Set<CandidatesConfirmationTableFile>()
                .Include(a => a.Owner)
                .FirstOrDefault(a => a.Id == fileId);
            if (entity == null)
            {
                file = _storageService.GetTempFile(DocumentStorageConst.DOC_CANDITATES_CONFIRMATION_TABLE_FILES, fileId);
                CombineStatuses(_storageService);
            }
            else
            {
                file = _storageService.GetFile(DocumentStorageConst.DOC_CANDITATES_CONFIRMATION_TABLE_FILES, entity.Owner.OwnerId.ToString(), fileId);
                CombineStatuses(_storageService);

                if (IsValid)
                    file.FileName = entity.FileName;
            }

            return file;
        }
        public IEnumerable<IStorageFileInfo> UploadFiles(params StorageFile[] files)
        {
            var result = _storageService
                .SaveTemp(DocumentStorageConst.DOC_CANDITATES_CONFIRMATION_TABLE_FILES, files);

            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }
        public void DeleteFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<CandidatesConfirmationTableFile>()
                .FirstOrDefault(a => a.Id == fileId);

            Delete(fileId, entity, DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES);
        }
        private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
        {
            if (entity != null)
            {
                _storageService.DeleteTemp(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
        }
        #endregion
    }
}
