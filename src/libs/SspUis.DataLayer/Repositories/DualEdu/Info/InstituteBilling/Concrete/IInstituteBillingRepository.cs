using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IInstituteBillingRepository : 
    IBaseEntityRepository<int, InstituteBilling, CreateInstituteBillingDlDto, UpdateInstituteBillingDlDto>
{
}