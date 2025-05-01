using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer
{
    public class SrvContractForDeedDto
    {
        public string Details { get; set; }
        public long ApplicationId { get; set; }
        public long ContractorId { get; set; }
        public long SrvContractorId { get; set; }
        public Guid Id2 { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorBankAccount { get; set; }
        public string ContractorBankName { get; set; }
        public string ContractorBankMFO { get; set; }
        public string ContractorDirector { get; set; }
        public string ContractorPhoneNumber { get; set; }
        public string ContractorRegion { get; set; }
        public string Organization { get; set; }
        public string ContractorDistrict { get; set; }
        public DateOnly SrvContractDocOn { get; set; }
        public string SrvContractDocNumber { get; set; }
		public new List<SrvDeedGroupDto> Groups { get; set; } = new();


	}
}