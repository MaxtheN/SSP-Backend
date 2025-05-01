using SspUis.Integration.Billing.Models;
using StatusGeneric;

namespace SspUis.Integration.Billing.Services;

public interface IBillingService: IStatusGenericHandler
{
    Task<string> CreateDualApplication(DualApplicationCreateDto1 dto);
    Task<byte[]> DownloadContract(Guid fileId);
    Task<List<UniversityListDto>> GetUniversityList();
    Task<SpecialityListDto> GetSpecialityList(int organizationId);
    Task<UniversityGetDto> GetUniversity(int organizationId);
    Task<SpecialityGetDto> GetSpeciality(int specialityId);
    Task<int?> HeldByContractor(int id);
}