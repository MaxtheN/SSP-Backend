using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IIndicatorService : IStatusGeneric

{
	PagedResult<IndicatorListDto> GetList(SortFilterPageOptions option);
	IndicatorDto Get();
	IndicatorDto GetById(int id);
	SelectList<int> AsSelectList();
	HaveId<int> Create(CreateIndicatorDlDto dto);
	void Update(UpdateIndicatorDlDto dto);
	public void Delete(int id);
}

