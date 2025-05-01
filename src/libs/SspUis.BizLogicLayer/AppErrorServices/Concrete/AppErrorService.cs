using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using WEBASE;

namespace SspUis.BizLogicLayer.AppErrorServices
{
    public class AppErrorService
        : BaseEntityService<long, AppError, AppErrorListDto, AppErrorDto, CreateAppErrorDlDto, UpdateAppErrorDlDto, IAppErrorRepository>
        , IAppErrorService
    {
        private readonly IAuthService _authService;

        public AppErrorService(IUnitOfWork unitOfWork, IAuthService authService)
            : base(unitOfWork)
        {
            _authService = authService;
        }


        public PagedResult<AppErrorListDto> GetList(SortFilterPageOptions dto)
        {
            var result = Repository.ReadAsNoTracked<AppErrorListDto>()
                            .SortFilter(dto)
                            .AsPagedResult(dto);
            return result;
        }

    }
}
