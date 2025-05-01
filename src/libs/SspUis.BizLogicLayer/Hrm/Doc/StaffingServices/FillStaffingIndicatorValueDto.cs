using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class FillStaffingIndicatorValueDto : StaffingIndicatorValueDto
    {
        public bool IsTotal { get; set; }
        public bool IsCalculationKindTotal { get; set; }
        public List<int> Tables { get; set; }
        public decimal Percentage { get; set; }
        public int CalcOrderCode { get; set; }
        public int DisplayOrderCode { get; set; }
        public bool IsCalCulated { get; set; }
    }
}
