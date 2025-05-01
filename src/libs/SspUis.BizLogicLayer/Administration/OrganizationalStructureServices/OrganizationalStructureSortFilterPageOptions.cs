using WEBASE;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureSortFilterPageOptions : TableSortFilterPageOptions
    {
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? SignOrganizationTypeId { get; set; }
    }
}
