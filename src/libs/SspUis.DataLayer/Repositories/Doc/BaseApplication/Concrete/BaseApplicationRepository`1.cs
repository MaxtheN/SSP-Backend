using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class BaseApplicationRepository<
    TApplicationEntity,
    TCreateApplicationDto,
    TUpdateApplicationDto,
    TUpdateStatusDto,
    TUpdateStepDto>

    : BaseEntityRepository<long, TApplicationEntity, TCreateApplicationDto, TUpdateApplicationDto>

    , IBaseApplicationRepository<long,
        TApplicationEntity,
        TCreateApplicationDto,
        TUpdateApplicationDto,
        TUpdateStatusDto,
        TUpdateStepDto>
        where TApplicationEntity : class, IBaseApplicationEntity, IHaveIdProp<long>
        where TCreateApplicationDto : BaseApplicationDlDto<TCreateApplicationDto, TApplicationEntity>
        where TUpdateApplicationDto : BaseApplicationDlDto<TUpdateApplicationDto, TApplicationEntity>, IHaveIdProp<long>
        where TUpdateStatusDto : UpdateStatusApplicationDlDto<TUpdateStatusDto, TApplicationEntity>
        where TUpdateStepDto : UpdateStepApplicationDlDto<TUpdateStepDto, TApplicationEntity>

{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public BaseApplicationRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
        : base(crudServices)
    {
        _authService = authService;
        this._unitOfWork = unitOfWork;
    }

    protected override void OnCreate(TApplicationEntity entity, TCreateApplicationDto dto)
    {
        SetEntityProperties(entity, dto);
    }

    protected override void OnUpdate(TApplicationEntity entity, TUpdateApplicationDto dto)
    {
        //SetEntityProperties(entity, dto);
    }

    protected virtual void SetEntityProperties<TDto>(TApplicationEntity entity, BaseApplicationDlDto<TDto, TApplicationEntity> dto)
       where TDto : BaseApplicationDlDto<TDto, TApplicationEntity>
    {
        if (dto is CreateClaimApplicationDlDto claimApplicationDlDto && claimApplicationDlDto.ContractIdentificationNumber is not null)
        {
            entity.Application.DistrictId = claimApplicationDlDto.IntegrationContractor.DistrictId;
            entity.Application.RegionId = claimApplicationDlDto.IntegrationContractor.RegionId;
            entity.Application.DistrictName = claimApplicationDlDto.IntegrationContractor.District.FullName;
            entity.Application.RegionName = claimApplicationDlDto.IntegrationContractor.Region.FullName;
            entity.Application.ContractorId = claimApplicationDlDto.IntegrationContractor.Id;
        }
        else
        {
            var region = _unitOfWork.RegionRepository.ById(_authService.Contractor.RegionId);
            var district = _unitOfWork.DistrictRepository.ById(_authService.Contractor.DistrictId);
            entity.Application.DistrictId = _authService.Contractor.DistrictId;
            entity.Application.RegionId = _authService.Contractor.RegionId;
            entity.Application.DistrictName = district.FullName;
            entity.Application.RegionName = region.FullName;
            entity.Application.ContractorId = _authService.Contractor.Id;
        }
    }

    protected override IQueryable<TApplicationEntity> ByIdQuery()
        => AllAsQueryable.Include(a => a.Application);
    protected override IQueryable<TApplicationEntity> ByIdQuery(bool applyFilter)
    {
        if (!applyFilter)
        {
            return DbSet.Include(x => x.Application).AsQueryable();
        }

        return AllAsQueryable.Include(x => x.Application);
    }
    protected override IQueryable<TApplicationEntity> InjectFilter(IQueryable<TApplicationEntity> query)
    {
        query = query.Where(a => a.Application.StatusId != StatusIdConst.DELETED);
        if (_authService.Contractor != null)
            query = query.Where(a => a.Application.ContractorId == _authService.Contractor.Id);
        return query;
    }

    public TApplicationEntity UpdateStatus(TUpdateStatusDto updateStatusDto, Action<TApplicationEntity> validation = null)
    {
        TApplicationEntity val = ById(updateStatusDto.Id);
        if (base.IsValid)
        {
            validation?.Invoke(val);
        }

        if (base.HasErrors)
        {
            return null;
        }

        updateStatusDto.UpdateEntity(val);
        base.Context.Entry(val).State = EntityState.Modified;
        return val;
    }

    public TApplicationEntity UpdateStatus(TUpdateStatusDto updateStatusDto, Action<TApplicationEntity> validation, bool applyFilter)
    {
        TApplicationEntity val = ById(updateStatusDto.Id, applyFilter);
        if (base.IsValid)
        {
            validation?.Invoke(val);
        }

        if (base.HasErrors)
        {
            return null;
        }

        updateStatusDto.UpdateEntity(val);
        base.Context.Entry(val).State = EntityState.Modified;
        return val;
    }
    public TApplicationEntity UpdateStep(TUpdateStepDto updateStepDto, Action<TApplicationEntity> validation = null)
    //where TUpdateStepDto : UpdateStepApplicationDlDto<TUpdateStepDto, TApplicationEntity>
    {
        TApplicationEntity val = ById(updateStepDto.Id);
        if (base.IsValid)
        {
            validation?.Invoke(val);
        }

        if (base.HasErrors)
        {
            return null;
        }

        updateStepDto.UpdateEntity(val);
        base.Context.Entry(val).State = EntityState.Modified;
        return val;
    }
    public TApplicationEntity UpdateStep(TUpdateStepDto updateStepDto, Action<TApplicationEntity> validation, bool applyFilter)
    //where TUpdateStepDto : UpdateStepApplicationDlDto<TUpdateStepDto, TApplicationEntity>
    {
        TApplicationEntity val = ById(updateStepDto.Id, applyFilter);
        if (base.IsValid)
        {
            validation?.Invoke(val);
        }

        if (base.HasErrors)
        {
            return null;
        }

        updateStepDto.UpdateEntity(val);
        base.Context.Entry(val).State = EntityState.Modified;
        return val;
    }

}
