using System.Linq;
using GenericServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ContractorCategoryCriterionRepository
    : BaseEntityRepository<long, ContractorCategoryCriterion, CreateContractorCategoryCriterionDlDto, UpdateContractorCategoryCriterionDlDto, UpdateStatusContractorCategoryCriterionDlDto>
    , IContractorCategoryCriterionRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudService;

    public ContractorCategoryCriterionRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
        _crudService = crudServices;
    }

    protected override void OnCreate(ContractorCategoryCriterion entity, CreateContractorCategoryCriterionDlDto dto)
    {
        if (_authService is null || _authService.User is null)
            entity.OrganizationId = 1;
        else
            entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<ContractorCategoryCriterion> InjectFilter(IQueryable<ContractorCategoryCriterion> query)
    {
        return query.Where(x => x.StatusId != StatusIdConst.DELETED);

    }
}
