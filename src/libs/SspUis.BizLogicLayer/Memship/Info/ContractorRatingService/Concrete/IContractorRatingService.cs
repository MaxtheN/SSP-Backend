using SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices;
using SspUis.DataLayer.Repositories;
using StatusGeneric;

using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public interface IContractorRatingService : IStatusGeneric
{
    PagedResult<ContractorRatingListDto> GetList(SortFilterPageOptions dto);
    ContractorRatingDto Get();
    ContractorRatingDto GetById(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateContractorRatingDlDto dto);
    void Update(UpdateContractorRatingDlDto dto);
    void Delete(int id);
}
