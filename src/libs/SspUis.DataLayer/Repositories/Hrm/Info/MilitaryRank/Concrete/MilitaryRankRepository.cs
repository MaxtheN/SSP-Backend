using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class MilitaryRankRepository : BaseEntityRepository<int, MilitaryRank, CreateMilitaryRankDlDto, UpdateMilitaryRankDlDto>, IMilitaryRankRepository
{
    public MilitaryRankRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<MilitaryRank> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
