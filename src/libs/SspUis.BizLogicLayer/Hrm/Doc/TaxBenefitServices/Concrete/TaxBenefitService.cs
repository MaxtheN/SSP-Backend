using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Models;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm 
{
    public class TaxBenefitService : StatusGenericHandler, ITaxBenefitService
    {

        private readonly ITaxBenefitRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IDocumentChangeLogService _documentChangeLogService;

        public TaxBenefitService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IDocumentChangeLogService documentChangeLogService)
        {
            this._repository = unitOfWork.TaxBenefitRepository;
            this._unitOfWork = unitOfWork;
            this._authService = authService;
            this._documentChangeLogService = documentChangeLogService;
        }

        public SelectList<long> AsSelectList(TaxBenefitSortFilterOptions options)
        {
            return _repository.ReadAsNoTracked<TaxBenefitListDto>()
                .SortFilter(options)
                .AsSelectList();
        }

        public PagedResult<TaxBenefitListDto> GetList(TaxBenefitSortFilterOptions options)
        {
            return _repository.ReadAsNoTracked<TaxBenefitListDto>()
                .SortFilter(options)
                .AsPagedResult(options);
        }
        public TaxBenefitDto Get()
        {
            return new TaxBenefitDto()
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                //DocNumber = _numberTemplateService.GetNext(TableIdConst.HRM__DOC_TAX_BENEFIT, isOrdinary: true)
            };
        }
        public TaxBenefitDto Get(long id)
        {
            //return _repository.ById<TaxBenefitDto>(id);
            var dto = _repository.ById<TaxBenefitDto>(id);
            CombineStatuses(_repository);
            if (IsValid)
            {
                dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.TaxBenefitEdit);
                dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.TaxBenefitAccept);
                dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.TaxBenefitCancel);
                dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.TaxBenefitDelete);
            }
            return dto;
        }

        public HaveId<long> Create(CreateTaxBenefitDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.Create(dto, ent => Validation(dto, ent));
                    CombineStatuses(_repository);
                    _unitOfWork.Save();
                    if (IsValid)
                    {
                        _unitOfWork.Save();
                        var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CreateTaxBenefit");
                        transaction.Commit();
                        return HaveId.Create(entity.Id);
                    }
                }
                catch (DbUpdateException e)
                {
                    AddError(e.Message);
                    if (e.InnerException != null)
                        AddError(e.InnerException.Message);
                    transaction.Rollback();
                }
                return null;
            }
        }
        public void Accept(UpdateStatusTaxBenefitDto dTo)
        {
            var dto = new UpdateStatusTaxBenefitDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");
            });
        }

        public void Cancel(UpdateStatusTaxBenefitDto dTo)
        {
            var dto = new UpdateStatusTaxBenefitDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");
            });
        }
        public void Update(UpdateTaxBenefitDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.Update(dto, ent => Validation(dto, ent));
                    CombineStatuses(_repository);
                    _unitOfWork.Save();
                    if (IsValid)
                    {
                        _unitOfWork.Save();
                        var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateTaxBenefit");
                        transaction.Commit();
                    }
                }
                catch (DbUpdateException e)
                {
                    AddError(e.Message);
                    transaction.Rollback();
                }
            }
        }
        private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            var moveDto = _repository.ById<TaxBenefitDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: moveDto,
                tableId: TableIdConst.DOC_TAXBENEFIT,
                organizationId: null,
                statusId: statusId,
                message: message);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }
        public void Delete(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.ById(id);
                    var statusDto = new UpdateStatusTaxBenefitDlDto()
                    {
                        Id = id,
                        StatusId = StatusIdConst.DELETED
                    };
                    UpdateStatus(statusDto, ent =>
                    {
                        if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, statusDto.StatusId))
                            _repository.AddError("Нет доступа");
                    });
                    _unitOfWork.Save();
                    var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteTaxBenefit");
                    _repository.UpdateStatus(statusDto);

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
        private HaveId<long> UpdateStatus(UpdateStatusTaxBenefitDlDto dto, Action<TaxBenefit> validation)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.UpdateStatus(dto);
                    CombineStatuses(_repository);
                    if (HasErrors)
                        return null;
                    _unitOfWork.Save();
                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
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
        private void Validation<TDto>(TaxBenefitDlDto<TDto> dto, TaxBenefit entity)
            where TDto : TaxBenefitDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                    AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
            }
            if (query.ByDocNumber(dto.DocNumber).Any())
                _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

        }
    }
}


