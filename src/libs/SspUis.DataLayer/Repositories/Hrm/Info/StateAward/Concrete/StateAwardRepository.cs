using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class StateAwardRepository : BaseEntityRepository<int, StateAward, CreateStateAwardDlDto, UpdateStateAwardDlDto>, IStateAwardRepository
{
    public StateAwardRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<StateAward> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
