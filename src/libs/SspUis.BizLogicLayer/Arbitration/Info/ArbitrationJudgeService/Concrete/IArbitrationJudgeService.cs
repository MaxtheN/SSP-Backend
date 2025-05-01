using System.Threading.Tasks;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IArbitrationJudgeService : IStatusGeneric
{
    PagedResult<ArbitrationJudgeListDto> GetList(ArbitrationJudgeSortFilterOptions dto);
    ArbitrationJudgeDto Get();
    ArbitrationJudgeDto Get(int id);
    SelectList<int> AsSelectList();
    Task<HaveId<int>> Create(CreateArbitrationJudgeDlDto dto);
    ValueTask Update(UpdateArbitrationJudgeDlDto dto);
    void Delete(int id);
}
