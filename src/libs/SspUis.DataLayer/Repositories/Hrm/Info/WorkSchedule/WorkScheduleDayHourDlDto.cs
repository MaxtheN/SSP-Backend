using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class WorkScheduleDayHourDlDto : EntityDto<WorkScheduleDayHourDlDto, WorkScheduleDayHour>,IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        public int DayNumber { get; set; }
        [LocalizedRequired]
        public bool IsDayOff { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(10)]
        public string BeginAt { get; set; } = null!;
        [LocalizedRequired]
        [LocalizedStringLength(10)]
        public string EndAt { get; set; } = null!;
    }
}
