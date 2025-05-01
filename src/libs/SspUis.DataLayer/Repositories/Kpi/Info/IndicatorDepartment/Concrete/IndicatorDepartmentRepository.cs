using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class IndicatorDepartmentRepository :
	BaseEntityRepository<
	int, IndicatorDepartment,
	CreateIndicatorDepartmentDlDto,
	UpdateIndicatorDepartmentDlDto>,
	IIndicatorDepartmentRepository
{
	public IndicatorDepartmentRepository(ICrudServices crudServices) : base(crudServices)
	{

	}
	protected override IQueryable<IndicatorDepartment> InjectFilter(IQueryable<IndicatorDepartment> query)
	{
		query = query.Where(a => a.StateId != StateIdConst.PASSIVE);
		return query.AsQueryable();
	}
	protected override IQueryable<IndicatorDepartment> ByIdQuery()
			=> AllAsQueryable.Include(a => a.Translates);
}
