using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationServices
{
    public interface IClaimOrganizationService : IStatusGeneric
    {
        PagedResult<ClaimOrganizationListDto> GetList(SortFilterPageOptions dto);
        ClaimOrganizationDto Get();
        ClaimOrganizationDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateClaimOrganizationDlDto dto);
        void Update(UpdateClaimOrganizationDlDto dto);
        void Delete(int id);
    }
}
