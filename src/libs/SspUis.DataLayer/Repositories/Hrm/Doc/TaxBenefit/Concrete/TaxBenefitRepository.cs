using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class TaxBenefitRepository
    : BaseEntityRepository<long, TaxBenefit, CreateTaxBenefitDlDto, UpdateTaxBenefitDlDto, UpdateStatusTaxBenefitDlDto>
    , ITaxBenefitRepository
{
    private readonly IAuthService _authService;

    public TaxBenefitRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(TaxBenefit entity, CreateTaxBenefitDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<TaxBenefit> InjectFilter(IQueryable<TaxBenefit> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<TaxBenefit> ByIdQuery()
    {
        return AllAsQueryable;
    }
}
