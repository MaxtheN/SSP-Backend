
using SspUis.BizLogicLayer;
using WEBASE.Models;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public class StaffingTemplateListSortFilterDto : DocumentSortFilterOptions
    {
        public string OrganizationSettlementAccountCode { get; set; }
    }
}
