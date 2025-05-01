using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationTypeServices
{
    public interface IClaimOrganizationTypeService : IStatusGeneric
    {
        PagedResult<ClaimOrganizationTypeListDto> GetList(SortFilterPageOptions dto);
        ClaimOrganizationTypeDto Get();
        ClaimOrganizationTypeDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateClaimOrganizationTypeDlDto dto);
        void Update(UpdateClaimOrganizationTypeDlDto dto);
        void Delete(int id);
    }
}
