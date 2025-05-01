using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ContractorRatingRepository : BaseEntityRepository<int, ContractorRating, CreateContractorRatingDlDto, UpdateContractorRatingDlDto>, IContractorRatingRepository
{
    private readonly IAuthService _authService;

    public ContractorRatingRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override IQueryable<ContractorRating> ByIdQuery()
        => AllAsQueryable.Include(a => a.Translates);

}