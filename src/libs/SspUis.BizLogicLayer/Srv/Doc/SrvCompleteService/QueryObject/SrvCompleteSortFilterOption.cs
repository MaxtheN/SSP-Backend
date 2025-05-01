using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class SrvCompleteSortFilterOption : SortFilterPageOptions
    {
        public int? StatusId { get; set; }
        public long? ContractorId { get; set; }
        public long? ServiceContractId { get; set; }
        public long? ServiceContractTableId { get; set; }
        public long? EmployeeManageId { get; set; }
    }
}
