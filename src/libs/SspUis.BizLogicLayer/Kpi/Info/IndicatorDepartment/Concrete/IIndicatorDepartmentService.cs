using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IIndicatorDepartmentService : IStatusGeneric

{
	PagedResult<IndicatorDepartmentListDto> GetList(SortFilterPageOptions option);
	IndicatorDepartmentDto Get();
	IndicatorDepartmentDto GetById(int id);
	SelectList<int> AsSelectList();
	HaveId<int> Create(CreateIndicatorDepartmentDlDto dto);
	void Update(UpdateIndicatorDepartmentDlDto dto);
	public void Delete(int id);
}

