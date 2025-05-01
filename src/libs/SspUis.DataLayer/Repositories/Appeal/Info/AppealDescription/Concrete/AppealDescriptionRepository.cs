using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Appeal;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AppealDescriptionRepository : BaseEntityRepository<int, AppealDescription, CreateAppealDescriptionDlDto, UpdateAppealDescriptionDlDto>, IAppealDescriptionRepository
    {
        private readonly IAuthService _authService;

        public AppealDescriptionRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<AppealDescription> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
