using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Models;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer
{
    public class SrvDeedListDto 
        : DocumentListDto<long>, ILinkToEntity<ServiceDeed>, IHaveIdProp<long>, IHaveStatusId
    {
        [IgnoreWordProperty]
        public Guid Id2 { get; set; }
        public string DocNumber { get; set; }
        public string Details { get; set; }
        public long ContractorId { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorRegion { get; set; }
        public decimal Price { get; set; }
        public string ContractorDistrict { get; set; }
        public int? ContractorRegionId { get; set; }
        public string OrderCode {  get; set; }
        public int? ContractorDistrictId { get; set; }
        public long ApplicationId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationRegion { get; set; }
        public string OrganizationDistrict { get; set; }
        public string Organization { get; set; }
        public string Status { get; set; }
        public int CompletedWorksCount { get; set; }
        public bool CanCreatePaymentOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int SrvContractStatusId { get; set; }
        public int? SrvApplicationOrganizationId { get; set; }
        public new List<SrvDeedGroupDto> Groups { get; set; } = new();
    }
}