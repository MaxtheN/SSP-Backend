using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IUniteOfMeasureService : IStatusGeneric

{
	PagedResult<UniteOfMeasureListDto> GetList(SortFilterPageOptions option);
	UniteOfMeasureDto Get();
	UniteOfMeasureDto GetById(int id);
	SelectList<int> AsSelectList();
	HaveId<int> Create(CreateUniteOfMeasureDlDto dto);
	void Update(UpdateUniteOfMeasureDlDto dto);
	public void Delete(int id);
}

