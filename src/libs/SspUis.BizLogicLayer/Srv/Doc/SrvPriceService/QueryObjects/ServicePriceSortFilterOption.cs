using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceSortFilterOption : SortFilterPageOptions
    {

    }

    public class GroupingByServicesPriceDtoFilter
    {
        public int? NeedChamberServiceGroupId { get; set; }
        public bool? IsPaid { get; set; }
        public DateOnly? DocOn { get; set; }
    }
}
