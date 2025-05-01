using SspUis.DataLayer.Repositories.Hrm;
using StatusGeneric;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public interface IStaffingTemplateService :
        IStatusGeneric
    {
        PagedResult<StaffingTemplateListDto> GetList(StaffingTemplateListSortFilterDto dto);
        StaffingTemplateDto Get();
        StaffingTemplateDto Get(long id); 
        IEnumerable<StaffingPostitionDto> GetPositionsAsSelectList(
            string? organizationSettlementAccountCode = null,
            int? staffingTemplateId = null);
        /*IEnumerable<StaffingTemplateTableDto> GetTableAsSelectList(
            string? organizationSettlementAccountCode = null,
            int? staffingTemplateId = null);*/
        SelectList<long> AsSelectList();
        HaveId<long> Create(CreateStaffingTemplateDlDto dto);
        void Accept(UpdateStatusStaffingTemplateDlDto statusDto);
        void Cancel(UpdateStatusStaffingTemplateDlDto statusDto);
        void Update(UpdateStaffingTemplateDlDto dto);
        void Delete(long id);
    }
}