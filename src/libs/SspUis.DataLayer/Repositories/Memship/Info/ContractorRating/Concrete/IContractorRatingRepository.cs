

using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IContractorRatingRepository :
    IBaseEntityRepository<int, ContractorRating,
        CreateContractorRatingDlDto, UpdateContractorRatingDlDto>
{
}
