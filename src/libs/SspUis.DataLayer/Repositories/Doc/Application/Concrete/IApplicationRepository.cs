using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IApplicationRepository : IBaseEntityRepository<long, Application, CreateApplicationDlDto, UpdateApplicationDlDto, UpdateStatusApplicationDlDto>
    {
        Application SetSend(long id,long? externalId = null);
        //Application UpdatePrtn(UpdatePrtnApplicationDlDto updateDto, Action<Application> validation = null);
    }
}
