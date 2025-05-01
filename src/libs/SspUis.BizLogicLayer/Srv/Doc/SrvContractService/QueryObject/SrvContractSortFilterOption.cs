using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class SrvContractSortFilterOption : SortFilterPageOptions
    {
        public int? StatusId { get; set; }
        public int? OrganizationId { get; set; }
        public int? ContractorRegionId { get; set; }
        public int? ContractorDistrictId { get; set; }
        public string? ContractorInn { get; set; }
        public DateOnly? FromDocDate { get; set; }
        public DateOnly? ToDocDate { get; set; }
    }
}