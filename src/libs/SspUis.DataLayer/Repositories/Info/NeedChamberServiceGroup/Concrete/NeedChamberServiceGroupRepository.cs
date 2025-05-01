using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NeedChamberServiceGroupRepository
        : BaseEntityRepository<int, NeedChamberServiceGroup, CreateNeedChamberServiceGroupDlDto, UpdateNeedChamberServiceGroupDlDto>,
        INeedChamberServiceGroupRepository
    {
        public NeedChamberServiceGroupRepository(ICrudServices crudServices)
            : base(crudServices)
        { }

        protected override IQueryable<NeedChamberServiceGroup> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

        protected override IQueryable<NeedChamberServiceGroup> InjectFilter(IQueryable<NeedChamberServiceGroup> query)
        {
            query = query.Where(nch => nch.StateId != StateIdConst.PASSIVE);

            return base.InjectFilter(query);
        }
    }
}
