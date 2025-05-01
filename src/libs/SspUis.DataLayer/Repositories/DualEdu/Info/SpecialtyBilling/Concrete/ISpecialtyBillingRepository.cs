using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface ISpecialtyBillingRepository : IBaseEntityRepository<int, SpecialtyBilling, CreateSpecialtyBillingDlDto, UpdateSpecialtyBillingDlDto>
{
}