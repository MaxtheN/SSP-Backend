using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class BaseApplicationRepository<TApplicationEntity, TCreateApplicationDto, TUpdateApplicationDto, TUpdateStatusDto>
    : BaseEntityRepository<long, TApplicationEntity, TCreateApplicationDto, TUpdateApplicationDto>
    , IBaseApplicationRepository<long, TApplicationEntity, TCreateApplicationDto, TUpdateApplicationDto, TUpdateStatusDto>
        where TApplicationEntity : class, IBaseApplicationEntity, IHaveIdProp<long>
        where TCreateApplicationDto : BaseApplicationDlDto<TCreateApplicationDto, TApplicationEntity>
        where TUpdateApplicationDto : BaseApplicationDlDto<TUpdateApplicationDto, TApplicationEntity>, IHaveIdProp<long>
        where TUpdateStatusDto : UpdateStatusApplicationDlDto<TUpdateStatusDto, TApplicationEntity>

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
        else if (dto is CreateMemshipApplicationDlDto memshipApplicationDlDto && _authService.Contractor == null)
        {
            var contractor = _unitOfWork.Context.Set<Contractor>().Include(r => r.Region).Include(d => d.District).FirstOrDefault(a => a.Inn == memshipApplicationDlDto.ContractorInn);

            entity.Application.DistrictId = contractor.DistrictId;
            entity.Application.RegionId = contractor.RegionId;
            entity.Application.DistrictName = contractor.District.FullName;
            entity.Application.RegionName = contractor.Region.FullName;
            entity.Application.ContractorId = contractor.Id;
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

}
