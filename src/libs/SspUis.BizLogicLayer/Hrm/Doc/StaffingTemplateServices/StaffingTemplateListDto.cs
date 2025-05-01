using GenericServices;
using SspUis.BizLogicLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.Models;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public class StaffingTemplateListDto : DocumentListDto<long>, ILinkToEntity<StaffingTemplate>, IHaveIdProp<long>, IHaveStatusId
    {
        public string DocNumber { get; set; } = null!;
        public string? Details { get; set; }
        public string Template { get; set; } = null!;

        public string Status { get; set; }
        public int OrganizationId { get; set; }
    }
}
