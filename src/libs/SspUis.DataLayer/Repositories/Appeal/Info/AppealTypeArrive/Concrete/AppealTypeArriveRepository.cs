using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Appeal;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AppealTypeArriveRepository : BaseEntityRepository<int, AppealTypeArrive, CreateAppealTypeArriveDlDto, UpdateAppealTypeArriveDlDto>, IAppealTypeArriveRepository
    {
        private readonly IAuthService _authService;

        public AppealTypeArriveRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<AppealTypeArrive> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
