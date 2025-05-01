using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationSortFilterOptions : DocumentSortFilterOptions
    {
        public bool? IsFree { get; set; }
        public string ContractorInn { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? StatusId { get; set; }
    }
}
