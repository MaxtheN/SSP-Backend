using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer
{ 
    public class CompletedServiceListDto : ILinkToEntity<CompletedService>
    {
        public long Id { get; set; }
        public string DocNumber { get; set; }
        public DateOnly DocOn { get; set; }
        public string Details { get; set; }
        public string OrganizationName { get; set; }
        public long ContractorId { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorRegion { get; set; }
        public string ContractorDistrict { get; set; }
        public long ServiceContractId { get; set; }
        public long ServiceContractTableId { get; set; }
        public long EmployeeManageId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}