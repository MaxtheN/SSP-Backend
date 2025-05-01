using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IMfyRepository : 
        IBaseEntityRepository<long, Mfy, CreateMfyDlDto, UpdateMfyDlDto>
    {
        Mfy ByExternalId(long externalId);
    }
}
