using StatusGeneric;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.MfyServices
{
    public interface IMfyService : IStatusGeneric
    {
        PagedResult<MfyListDto> GetList(SortFilterPageOptions dto);
        MfyDto Get();
        MfyDto Get(long id);
        SelectList<long> AsSelectList(int? regionId,int? districtId);
        Task SyncMfy();


    }
}
