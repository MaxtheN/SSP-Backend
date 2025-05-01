using SspUis.DataLayer.EfClasses;
using GenericServices;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AppErrorRepository : BaseEntityRepository<long, AppError, CreateAppErrorDlDto, UpdateAppErrorDlDto>, IAppErrorRepository
    {
        public AppErrorRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }
    }
}
