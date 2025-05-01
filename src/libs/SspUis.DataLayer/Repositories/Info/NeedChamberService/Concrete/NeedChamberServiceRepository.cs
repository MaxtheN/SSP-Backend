using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NeedChamberServiceRepository : BaseEntityRepository<int, NeedChamberService, CreateNeedChamberServiceDlDto, UpdateNeedChamberServiceDlDto>, INeedChamberServiceRepository
    {
        private readonly IAuthService _authService;

        public NeedChamberServiceRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override void OnCreate(NeedChamberService entity, CreateNeedChamberServiceDlDto dto)
        {
            base.OnCreate(entity, dto);
        }

        protected override void OnUpdate(NeedChamberService entity, UpdateNeedChamberServiceDlDto dto)
        {
            base.OnUpdate(entity, dto);
        }

        protected override IQueryable<NeedChamberService> ByIdQuery()
        {
            return base.ByIdQuery()
            .Include(a => a.Translates)
            .Include(a => a.Files)
            .Include(a => a.MemshipApplicationChamberServices);
        }
        protected override IQueryable<NeedChamberService> InjectFilter(IQueryable<NeedChamberService> query)
        {
            return base.InjectFilter(query).Where(x => x.StateId == StateIdConst.ACTIVE);
        }
    }
}
