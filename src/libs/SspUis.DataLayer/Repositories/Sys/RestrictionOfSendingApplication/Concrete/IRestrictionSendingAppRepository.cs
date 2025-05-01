using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IRestrictionSendingAppRepository 
        : IBaseEntityRepository<long, RestrictionOfSendingApplication, CreateRestrictionSendingAppDlDto, UpdateRestrictionSendingAppDlDto>
    {
        void Passive(RestrictionOfSendingApplication entity);
    }
}
