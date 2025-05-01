using SspUis.Core;
using System;

namespace SspUis.BizLogicLayer.Hrm
{
    public class ExcludePeriodDto
    {
        public int EmployeeId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public ExcludePerionType ExcludePeriodType { get; set; }
    }
}
