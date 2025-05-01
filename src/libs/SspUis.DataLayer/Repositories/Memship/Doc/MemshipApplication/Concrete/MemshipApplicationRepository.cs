using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories;

public class MemshipApplicationRepository
    : BaseApplicationRepository
        <MemshipApplication,
        CreateMemshipApplicationDlDto,
        UpdateMemshipApplicationDlDto,
        UpdateStatusMemshipApplicationDlDto>,
    IMemshipApplicationRepository
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    public MemshipApplicationRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
        : base(crudServices, authService, unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    protected override void OnCreate(MemshipApplication entity, CreateMemshipApplicationDlDto dto)
    {
        dto.IsRead = false;
        base.OnCreate(entity, dto);
    }

    protected override IQueryable<MemshipApplication> InjectFilter(IQueryable<MemshipApplication> query)
    {
        query = base.InjectFilter(query);
        if (_authService.Contractor == null && _authService.Organization.Id != OrganizationIdConst.SSP)//Hududga pullik ariza ko'rinmasligi uchun
        {
            //var expression = ContractorCategoryIdConst.IsPayedExpression();
            //query = query.Where(expression);

            query = query.Where(x =>
            !(x.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA
            || x.Application.Contractor.OpfId == OpfIdConst.UYUSHMA));
            query = query.Where(a => a.ChooseLocation ? a.ChoosedRegionId == _authService.Organization.RegionId : a.Application.RegionId == _authService.Organization.RegionId);
        }
        return query;
    }

    protected override IQueryable<MemshipApplication> ByIdQuery()
    {
        return AllAsQueryable
            .Include(m => m.Application)
            .Include(m => m.Application.Contractor)
            .Include(m => m.Application.Region)
            .Include(m => m.Application.District)
            ;
    }
}
