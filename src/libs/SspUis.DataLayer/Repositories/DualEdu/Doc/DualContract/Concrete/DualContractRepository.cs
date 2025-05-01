using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DualContractRepository :
    BaseEntityRepository<long, DualContract, CreateDualContractDlDto, UpdateDualContractDlDto, UpdateStatusDualContractDlDto>
    , IDualContractRepository
{
    private readonly IAuthService _authService;
    public DualContractRepository(
        ICrudServices crudServices,
        IAuthService authService
        ) : base(crudServices)
    {
        this._authService = authService;
    }
    protected override void OnCreate(DualContract entity, CreateDualContractDlDto dto)
    {
        base.OnCreate(entity, dto);
    }
    protected override void OnUpdate(DualContract entity, UpdateDualContractDlDto dto)
    {
    }
    protected override IQueryable<DualContract> InjectFilter(IQueryable<DualContract> query)
    {
        query = base.InjectFilter(query);
        if (_authService.Contractor != null)
            return query.Where(x => x.Application.ContractorId == _authService.Contractor.Id);

        return query;
    }
    protected override IQueryable<DualContract> ByIdQuery()
    {
        return AllAsQueryable
            .Include(a => a.Institute)
            .Include(c => c.Speciality)
            .Include(a => a.Application);
    }
}
