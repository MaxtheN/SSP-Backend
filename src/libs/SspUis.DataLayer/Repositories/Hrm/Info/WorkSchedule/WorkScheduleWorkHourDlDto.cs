using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class WorkScheduleWorkHourDlDto : EntityDto<WorkScheduleWorkHourDlDto, WorkScheduleWorkHour>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        public DateOnly DateOn { get; set; }
        [LocalizedRequired]
        public int Days { get; set; }
        [LocalizedRequired]
        public decimal Hours { get; set; }
        public int OwnerId { get; set; }
    }
}
