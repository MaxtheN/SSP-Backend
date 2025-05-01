using WEBASE.Models;
using WEBASE.EF;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class UpdateStatusStaffingDlDto : EntityDto<UpdateStatusStaffingDlDto, Staffing>, IHaveIdProp<long>
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
