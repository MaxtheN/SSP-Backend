using GenericServices;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.DualEdu;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using System.Linq;

namespace SspUis.DataLayer.Repositories;

public class DualApplicationRepository
        : BaseApplicationRepository<DualApplication, CreateDualApplicationDlDto, UpdateDualApplicationDlDto, UpdateStatusDualApplicationDlDto>,
        IDualApplicationRepository
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    public DualApplicationRepository(
        ICrudServices crudServices,
        IAuthService authService,
        IUnitOfWork unitOfWork)
        : base(crudServices, authService, unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }
    protected override void OnCreate(DualApplication entity, CreateDualApplicationDlDto dto)
    {
        base.OnCreate(entity, dto);
        entity.OrganizationId = OrganizationIdConst.SSP;
    }

    protected override void OnUpdate(DualApplication entity, UpdateDualApplicationDlDto dto)
    {
        base.OnUpdate(entity, dto);
        entity.OrganizationId = OrganizationIdConst.SSP;
    }

    protected override IQueryable<DualApplication> InjectFilter(IQueryable<DualApplication> query)
    {
        query = base.InjectFilter(query);
        if (_authService.Contractor != null)
            return query.Where(x => x.Application.ContractorId == _authService.Contractor.Id);
      
        //if (_authService.User.OrganizationId != OrganizationIdConst.SSP)
        //    return query.Where(x => x.OrganizationId == _authService.User.OrganizationId);

        return query;
    }

    protected override IQueryable<DualApplication> ByIdQuery()
    {
        return AllAsQueryable
            .Include(a => a.Application)
            .Include(c => c.Tables);
    }
}
