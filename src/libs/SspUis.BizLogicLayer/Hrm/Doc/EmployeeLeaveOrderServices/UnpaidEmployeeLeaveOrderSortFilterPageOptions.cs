using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UnpaidEmployeeLeaveOrderSortFilterPageOptions : SortFilterPageOptions
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
