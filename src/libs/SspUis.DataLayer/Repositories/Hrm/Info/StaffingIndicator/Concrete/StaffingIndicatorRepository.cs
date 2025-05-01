using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class StaffingIndicatorRepository : BaseEntityRepository<int, StaffingIndicator, CreateStaffingIndicatorDlDto, UpdateStaffingIndicatorDlDto>, IStaffingIndicatorRepository
    {
        private readonly IAuthService _authService;

        public StaffingIndicatorRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<StaffingIndicator> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
