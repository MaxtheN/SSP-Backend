using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Info.OrganizationalStructure
{
    public class OrganizationalStructureStaffingIndicatorTableDlDto : EntityDto<OrganizationalStructureStaffingIndicatorTableDlDto, OrganizationalStructureStaffingIndicatorTable>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int OwnerId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StaffingIndicatorId { get; set; }
    }
}
