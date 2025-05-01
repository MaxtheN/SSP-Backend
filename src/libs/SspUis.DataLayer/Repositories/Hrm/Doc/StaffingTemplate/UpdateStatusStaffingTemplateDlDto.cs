using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class UpdateStatusStaffingTemplateDlDto : EntityDto<UpdateStatusStaffingTemplateDlDto, StaffingTemplate>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        public string? Message { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StatusId { get; set; }
    }
}
