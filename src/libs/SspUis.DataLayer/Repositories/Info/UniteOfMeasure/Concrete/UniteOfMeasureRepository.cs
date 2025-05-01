using GenericServices;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class UniteOfMeasureRepository :
	BaseEntityRepository<
	int, UniteOfMeasure,
	CreateUniteOfMeasureDlDto,
	UpdateUniteOfMeasureDlDto>,
	IUniteOfMeasureRepository
{
	public UniteOfMeasureRepository(ICrudServices crudServices) : base(crudServices)
	{

	}
	protected override IQueryable<UniteOfMeasure> InjectFilter(IQueryable<UniteOfMeasure> query)
	{
		query = query.Where(a => a.StateId != StateIdConst.PASSIVE);
		return query.AsQueryable();
	}
}

