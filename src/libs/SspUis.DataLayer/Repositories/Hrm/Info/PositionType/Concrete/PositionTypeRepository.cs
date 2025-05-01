using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class PositionTypeRepository : BaseEntityRepository<int, PositionType, CreatePositionTypeDlDto, UpdatePositionTypeDlDto>, IPositionTypeRepository
{
    public PositionTypeRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<PositionType> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
