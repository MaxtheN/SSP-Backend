using SspUis.DataLayer;
using System;

namespace SspUis.BizLogicLayer.Memship
{
    public class MemshipDashFilterOption
    {
        public int? ContractorCategoryId { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public bool HasWeekly { get; set; } = false;
        public bool HasMonthly { get; set; } = false;
        public bool HasYearly { get; set; } = false;
    }

    public class MemshipDashRateFilterOption : MemshipDashFilterOption
    {
        public int? TableId { get; set; } = TableIdConst.MEMSHIP__DOC_MEMSHIP_APPLICATION;
        public bool IsDept { get; set; } = false;
    }
    public static class FilterTheDateTime
    {
        private static DateTime now { get; set; } = DateTime.Now;
        public static DateOnly[] Weekly()
        {
            var nowDayOfWeek = (int)now.DayOfWeek;

            return new DateOnly[]
            {
                DateOnly.FromDateTime(now.AddDays((int)DayOfWeek.Monday - nowDayOfWeek)),
                DateOnly.FromDateTime(now.AddDays(7 - nowDayOfWeek))
            };
        }
        public static DateOnly[] Monthly()
        {
            var begin = new DateTime(now.Year, now.Month, 1);
            return new DateOnly[]
            {
                DateOnly.FromDateTime(begin),
                DateOnly.FromDateTime(begin.AddMonths(1).AddDays(-1))
            };
        }
        public static DateOnly[] Yearly()
        {
            return new DateOnly[]
            {
                DateOnly.FromDateTime(new DateTime(now.Year, 1, 1)),
                DateOnly.FromDateTime(new DateTime(now.Year, 12, 31))
            };
        }
    }
}