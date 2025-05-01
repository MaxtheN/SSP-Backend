using GenericServices;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class SignCriterionRepository : BaseEntityRepository<int, SignCriterion, CreateSignCriterionDlDto, UpdateSignCriterionDlDto>, ISignCriterionRepository
{
    private readonly IAuthService _authService;

    public SignCriterionRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

}
