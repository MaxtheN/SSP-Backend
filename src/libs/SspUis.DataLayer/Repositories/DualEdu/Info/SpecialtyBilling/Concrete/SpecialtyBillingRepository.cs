using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class SpecialtyBillingRepository
    : BaseEntityRepository<int, SpecialtyBilling, CreateSpecialtyBillingDlDto, UpdateSpecialtyBillingDlDto>,
    ISpecialtyBillingRepository
{
    public SpecialtyBillingRepository(ICrudServices crudServices) : base(crudServices)
    {
    }
}