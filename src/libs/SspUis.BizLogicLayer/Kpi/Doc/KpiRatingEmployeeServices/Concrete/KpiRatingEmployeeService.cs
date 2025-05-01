using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Hrm.EmployeeManageServices;
using SspUis.BizLogicLayer.Kpi;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Integration.Manuals.Models;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class KpiRatingEmployeeService
    : BaseEntityService<long, KpiRatingEmployee, KpiRatingEmployeeListDto, KpiRatingEmployeeDto, CreateKpiRatingEmployeeDlDto, UpdateKpiRatingEmployeeDlDto, IKpiRatingEmployeeRepository, KpiRatingEmployeeSortFilterOptions>
    , IKpiRatingEmployeeService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    IDocumentChangeLogService _documentChangeLogService;
    private readonly IKpiRatingEmployeeRepository _repository;
    private readonly INumberService _numberService;
    private readonly IStorageService _storageService;
    private readonly IManualService _manualService;
    public KpiRatingEmployeeService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService,
        IManualService manualService,
        IStorageService storageService)
        : base(unitOfWork)
    {
        this._repository = unitOfWork.KpiRatingEmployeeRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
        _storageService = storageService;
        _manualService = manualService;
    }
    public PagedResult<KpiRatingEmployeeListDto> GetList(KpiRatingEmployeeSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<KpiRatingEmployeeListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }
    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public KpiRatingEmployeeDto Get()
    {
        return new KpiRatingEmployeeDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_KPI_RATING_EMPLOYEE, 1).Item2,
            OrganizationId = _authService.User.OrganizationId
        };
    }
    public HaveId<long> Create(CreateKpiRatingEmployeeDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (HasErrors)
                return null;
            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
            if (IsValid)
            {
                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
        }
        return null;
    }
    public KpiRatingEmployeeDto Get(long id)
    {
        var dto = _repository.ById<KpiRatingEmployeeDto>(id);
        CombineStatuses(_repository);
        if (dto is not null)
        {
            dto.CanModify = StatusIdConst.CanKpiRatingEmployee(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.KpiRatingEmployeeEdit);
            dto.CanAccept = StatusIdConst.CanKpiRatingEmployee(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.KpiRatingEmployeeAccept);
            dto.CanCancel = StatusIdConst.CanKpiRatingEmployee(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.KpiRatingEmployeeCancel);
            dto.CanDelete = StatusIdConst.CanKpiRatingEmployee(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.KpiRatingEmployeeDelete);
        }
        return dto;
    }
    public override void Update(UpdateKpiRatingEmployeeDlDto dto)
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
    public void Accept(UpdateStatusKpiRatingEmployeeDto dTo)
    {
        var dto = new UpdateStatusKpiRatingEmployeeDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public void Cancel(UpdateStatusKpiRatingEmployeeDto dTo)
    {
        var dto = new UpdateStatusKpiRatingEmployeeDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public override void Delete(long id)
    {
        var dto = new UpdateStatusKpiRatingEmployeeDlDto { Id = id, StatusId = StatusIdConst.DELETED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    private HaveId<long> UpdateStatus(UpdateStatusKpiRatingEmployeeDlDto dto, Action<KpiRatingEmployee> validation)
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
                var res = CreateDocumentChangeLog(entity.Id, dto.StatusId, "KpiRatingEmployee");
                if (IsValid)
                    transaction.Commit();
                //return res;
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
        return null;
    }
    private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = _repository.ById<KpiRatingEmployeeDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.KPI_RATING_EMPLOYEE,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private void Validation<TDto>(KpiRatingEmployeeDlDto<TDto> dto, KpiRatingEmployee entity)
          where TDto : KpiRatingEmployeeDlDto<TDto>
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
    
    public List<KpiRatingEmployeeTableDto> FillTable(int kpiPlanId)
    {
        var kpiPlan = UnitOfWork.KpiPlanForEmployeeRepository.ReadAsNoTracked<KpiPlanForEmployeeDto>().FirstOrDefault(s => s.Id == kpiPlanId);
       

        return kpiPlan.Tables.Select(s => new KpiRatingEmployeeTableDto
        {
            EmployeeManage = s.EmployeeManage,
            EmployeeManageId = s.EmployeeManageId,
            Points = s.Creates.Select(p => new KpiRatingEmployeePointDto
            {
                Coreamount = p.Amount,
                Corecount = p.Count,
                Indicator = p.Indicator,
                IndicatorId = p.IndicatorId
                
                
            }).ToList(),
        }).ToList();

        
    }
}
