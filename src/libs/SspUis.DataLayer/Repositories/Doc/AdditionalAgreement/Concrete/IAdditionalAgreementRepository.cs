using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IAdditionalAgreementRepository : IBaseEntityRepository<long, AdditionalAgreement, CreateAdditionalAgreementDlDto, UpdateAdditionalAgreementDlDto, UpdateStatusAdditionalAgreementDlDto>
    {
    }
}
