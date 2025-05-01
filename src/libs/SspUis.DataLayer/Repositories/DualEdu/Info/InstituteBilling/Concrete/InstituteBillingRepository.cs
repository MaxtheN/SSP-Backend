using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class InstituteBillingRepository :
    BaseEntityRepository<int, InstituteBilling, CreateInstituteBillingDlDto, UpdateInstituteBillingDlDto>,
    IInstituteBillingRepository
{
    public InstituteBillingRepository(ICrudServices crudServices) : base(crudServices)
    {
    }

    protected override IQueryable<InstituteBilling> ByIdQuery()
    {
        return base.ByIdQuery();
    }
}