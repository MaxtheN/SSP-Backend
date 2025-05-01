using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class MassPlannedCalculationService
    : BaseEntityService<long, MassPlannedCalculation, MassPlannedCalculationListDto, MassPlannedCalculationDto, CreateMassPlannedCalculationDlDto, UpdateMassPlannedCalculationDlDto, IMassPlannedCalculationRepository, MassPlannedCalculationSortFilterOptions>
    , IMassPlannedCalculationService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    IDocumentChangeLogService _documentChangeLogService;
    private readonly IMassPlannedCalculationRepository _repository;
    private readonly INumberService _numberService;
    public MassPlannedCalculationService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService)
        : base(unitOfWork)
    {
        this._repository = unitOfWork.MassPlannedCalculationRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
    }
    public PagedResult<MassPlannedCalculationListDto> GetList(MassPlannedCalculationSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<MassPlannedCalculationListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }

    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public MassPlannedCalculationDto Get()
    {
        return new MassPlannedCalculationDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MASS_PLANNED_CALCULATION, 1).Item2
        };
    }

    public override MassPlannedCalculationDto Get(long id)
    {
        var dto = _repository.ById<MassPlannedCalculationDto>(id);
        CombineStatuses(_repository);
        if (IsValid)
        {
            dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.MassPlannedCalculationEdit);
            dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.MassPlannedCalculationAccept);
            dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.MassPlannedCalculationCancel);
            dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.MassPlannedCalculationDelete);
        }
        return dto;
    }
    
    public override HaveId<long> Create(CreateMassPlannedCalculationDlDto dto)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Create(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                CombineStatuses(Repository);
                if(IsValid)
                    transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            catch(DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
            return null;
        }
    }

    public override void Update(UpdateMassPlannedCalculationDlDto dto)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                CombineStatuses(Repository);
                if(IsValid)
                    transaction.Commit();
            }
            catch(DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }
    public void Accept(UpdateStatusMassPlannedCalculationDto dTo)
    {
        var dto = new UpdateStatusMassPlannedCalculationDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }

    public void Cancel(UpdateStatusMassPlannedCalculationDto dTo)
    {
        var dto = new UpdateStatusMassPlannedCalculationDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public override void Delete(long id)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusMassPlannedCalculationDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, statusDto.StatusId))
                        _repository.AddError("Нет доступа");
                });
                UnitOfWork.Save();

                if(IsValid)
                {
                    transaction.Commit();
                }
            }
            catch(DbUpdateException ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusMassPlannedCalculationDlDto dto, Action<MassPlannedCalculation> validation)
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
                var res = CreateDocumentChangeLog(entity.Id, "MassPlannedCalculation");
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

    private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
    {
        var entityDto = Repository.ById<EmployeeSickLeaveDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: entityDto,
            tableId: TableIdConst.DOC_EMPLOYEE_SICK_LEAVE,
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
    private void Validation<TDto>(MassPlannedCalculationDlDto<TDto> dto, MassPlannedCalculation entity)
          where TDto : MassPlannedCalculationDlDto<TDto>
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
