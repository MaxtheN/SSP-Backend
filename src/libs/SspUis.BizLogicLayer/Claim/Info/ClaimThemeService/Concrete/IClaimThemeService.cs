using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim.ClaimThemeServices
{
    public interface IClaimThemeService : IStatusGeneric
    {
        PagedResult<ClaimThemeListDto> GetList(SortFilterPageOptions dto);
        ClaimThemeDto Get();
        ClaimThemeDto Get(int id);
        SelectList<int> AsSelectList(int? langId);
        HaveId<int> Create(CreateClaimThemeDlDto dto);
        void Update(UpdateClaimThemeDlDto dto);
        void Delete(int id);
    }
}
