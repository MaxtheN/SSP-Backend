using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class IndicatorRepository :
	BaseEntityRepository<
	int, Indicator,
	CreateIndicatorDlDto,
	UpdateIndicatorDlDto>,
	IIndicatorRepository
{
	public IndicatorRepository(ICrudServices crudServices) : base(crudServices)
	{

	}
	protected override IQueryable<Indicator> InjectFilter(IQueryable<Indicator> query)
	{
		query = query.Where(a => a.StateId != StateIdConst.PASSIVE);
		return query.AsQueryable();
	}
	protected override IQueryable<Indicator> ByIdQuery()
			=> AllAsQueryable.Include(a => a.Translates).Include(x => x.Tables);
}
