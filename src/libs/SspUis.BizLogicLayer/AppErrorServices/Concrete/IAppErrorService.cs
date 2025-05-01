using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.AppErrorServices
{
    public interface IAppErrorService : IBaseEntityService<long, AppError, AppErrorListDto, AppErrorDto, CreateAppErrorDlDto, UpdateAppErrorDlDto>
    {
    }
}
