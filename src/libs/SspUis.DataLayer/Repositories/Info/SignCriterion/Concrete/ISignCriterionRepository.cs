using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface ISignCriterionRepository : IBaseEntityRepository<int, SignCriterion, CreateSignCriterionDlDto, UpdateSignCriterionDlDto>
{
}
