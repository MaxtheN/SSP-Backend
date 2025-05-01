using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SettlementAccountSourceRepository : BaseEntityRepository<int, SettlementAccountSource, CreateSettlementAccountSourceDlDto, UpdateSettlementAccountSourceDlDto>, ISettlementAccountSourceRepository
    {
        public SettlementAccountSourceRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<SettlementAccountSource> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates)
            .Include(x => x.Children);

        protected override void OnCreate(SettlementAccountSource entity, CreateSettlementAccountSourceDlDto dto)
        {
            base.OnCreate(entity, dto);
            entity.Code = dto.Code1 + dto.Code2 + dto.Code3 + dto.Code4;
        }

        protected override void OnUpdate(SettlementAccountSource entity, UpdateSettlementAccountSourceDlDto dto)
        {
            base.OnUpdate(entity, dto);
            entity.Code = dto.Code1 + dto.Code2 + dto.Code3 + dto.Code4;
        }
    }
}
