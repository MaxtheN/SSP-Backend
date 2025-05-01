
using Microsoft.EntityFrameworkCore;
using SspUis.BizLayer.Hrm.StaffingTemplateServices;
using SspUis.BizLogicLayer.Hrm.StaffingServices;
using SspUis.BizLogicLayer.Kpi.Doc.KpiGratingService.QueryObjects;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Kpi;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class KpiGratingService : BaseEntityService<long, KpiGrating, KpiGratingListDto, KpiGratingDto, CreateKpiGratingDlDto, UpdateKpiGratingDlDto, IKpiGratingRepository, KpiGratingSortFilterOptions>,IKpiGratingService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    private readonly INumberService _numberService;
    private readonly IKpiGratingRepository _repository;
    private readonly IIndicatorService _indicatorService;
    
    public KpiGratingService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IAuthService authService,
        IIndicatorService indicatorService


        )
        : base(unitOfWork)
    {
        this._repository = unitOfWork.KpiGratingRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        _numberService = numberService;
        _indicatorService = indicatorService;
        
    }

    public PagedResult<KpiGratingListDto> GetList(KpiGratingSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<KpiGratingListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }

    public KpiGratingDto Get()
    {
        return new KpiGratingDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_KPI_GRATING, 1).Item2,
            OrganizationId = _authService.User.OrganizationId
        };
    }
    public KpiGratingDto Get(long id)
    {
        var dto = _repository.ById<KpiGratingDto>(id);
        CombineStatuses(_repository);
        if (dto is not null)
        {
            dto.CanModify = StatusIdConst.CanKpiGrating(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.KpiGratingEdit);
            dto.CanAccept = StatusIdConst.CanKpiGrating(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.KpiGratingAccept);
            dto.CanCancel = StatusIdConst.CanKpiGrating(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.KpiGratingCancel);
            dto.CanDelete = StatusIdConst.CanKpiGrating(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.KpiGratingDelete);
        }
        return dto;
    }

    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }

    public void Accept(UpdateStatusKpiGratingDlDto dTo)
    {
        var dto = new UpdateStatusKpiGratingDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }

    public void Cancel(UpdateStatusKpiGratingDlDto dTo)
    {
        var dto = new UpdateStatusKpiGratingDto { Id = dTo.Id, StatusId = StatusIdConst.CANCELED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }

    private HaveId<long> UpdateStatus(UpdateStatusKpiGratingDlDto dto, Action<KpiGrating> validation)
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
                //var res = CreateDocumentChangeLog(entity.Id, dto.StatusId);
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
    public HaveId<long> Create(CreateKpiGratingDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            
          
            try
            {
                var entity = _repository.Create(dto, ent => Validation(dto, ent));
                CombineStatuses(_repository);
                if (IsValid)
                {
                    _unitOfWork.Save();
                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} - {ex.InnerException} - {ex.StackTrace} - {ex.TargetSite} - {ex.Source}");
                transaction.Rollback();
            }
        }
        return null;
    }
    public override void Update(UpdateKpiGratingDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent => Validation(dto, ent));
                _unitOfWork.Save();
                CombineStatuses(Repository);
                if (IsValid)
                {
                    
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
    public List<KpiGratingIndicatorDto> FillIndicator()
    {
        //var data = _repository.CrudServices.ProjectFromEntityToDto<KpiGratingIndicator, KpiGratingIndicatorDto>();
        //var data = _indicatorRepository.CrudServices.


        var data = UnitOfWork.IndicatorRepository.ReadAsNoTracked<IndicatorDto>();
        return data.Select(a => new KpiGratingIndicatorDto
        {
           IndicatorId = a.Id,
           Indicator = a.FullName
           

        }).ToList();

    }
    private void Validation<TDto>(KpiGratingDlDto<TDto> dto, KpiGrating entity)
         where TDto : KpiGratingDlDto<TDto>
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
