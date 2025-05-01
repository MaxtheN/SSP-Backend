using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class PlannedCalculationService
    : BaseEntityService<long, PlannedCalculation, PlannedCalculationListDto, PlannedCalculationDto, CreatePlannedCalculationDlDto, UpdatePlannedCalculationDlDto, IPlannedCalculationRepository, PlannedCalculationSortFilterOptions>
    , IPlannedCalculationService
{
    IAuthService _authService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    public PlannedCalculationService(
        IUnitOfWork unitOfWork,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService)
        : base(unitOfWork)
    {
        this._authService = authService;
        this._documentChangeLogService = documentChangeLogService;
    }

    public SelectList<long> AsSelectList(PlannedCalculationSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<PlannedCalculationListDto>()
            .SortFilter(options)
            .AsSelectList();
    }

    public PagedResult<PlannedCalculationListDto> GetList(PlannedCalculationSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<PlannedCalculationListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }

    public override PlannedCalculationDto Get(long id)
    {
         return Repository.ById<PlannedCalculationDto>(id);
    }
    
    public override HaveId<long> Create(CreatePlannedCalculationDlDto dto)
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

    public override void Update(UpdatePlannedCalculationDlDto dto)
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
    public async Task<byte[]> DownloadPdf(Guid id2)
    {
        return null;
    }
    public override void Delete(long id)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusPlannedCalculationDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, statusDto.StatusId))
                        Repository.AddError("Нет доступа");
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
    private HaveId<long> UpdateStatus(UpdateStatusPlannedCalculationDlDto dto, Action<PlannedCalculation> validation)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            var entity = Repository.UpdateStatus(dto, validation);
            CombineStatuses(Repository);
            if (HasErrors)
                return null;
            UnitOfWork.Save();
            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "PlannedCalculation");
            if (IsValid)
                transaction.Commit();
            return res;
        }
        return null;
    }
    private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<PlannedCalculationDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.HRM__DOC_PLANNED_CALCULATION,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private void Validation<TDto>(PlannedCalculationDlDto<TDto> dto, PlannedCalculation entity)
      where TDto : PlannedCalculationDlDto<TDto>
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

        var employeeManageIds = entity.Tables.Select(t => t.EmployeeManageId).ToList();

        var manages = UnitOfWork.Context.Set<EmployeeManage>()
             .Include(x => x.Table)
             .Where(m => employeeManageIds.Contains(m.Id))
             .ToList();

        if (manages.Select(a => a.DepartmentId).Distinct().Count() > 1)
            AddError("Турли бўлимлардан ходимлар танланиши мумкин эмас / Вы не можете выбрать сотрудников из разных отделов");
    }
}
