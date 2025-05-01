using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.Linq;

using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class MemshipNewContractorRepository : BaseEntityRepository<long, MemshipNewContractor, CreateMemshipNewContractorDlDto, UpdateMemshipNewContractorDlDto, UpdateStatusMemshipNewContractorDlDto>
    , IMemshipNewContractorRepository
{
    private readonly IAuthService _authService;


    public MemshipNewContractorRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(MemshipNewContractor entity, CreateMemshipNewContractorDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<MemshipNewContractor> InjectFilter(IQueryable<MemshipNewContractor> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<MemshipNewContractor> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Tables);
    }
}
