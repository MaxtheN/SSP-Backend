using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UnpaidEmployeeSendTrainSortFilterPageOptions : SortFilterPageOptions
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
