using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UnpaidChastisementSortFilterPageOptions : SortFilterPageOptions
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
