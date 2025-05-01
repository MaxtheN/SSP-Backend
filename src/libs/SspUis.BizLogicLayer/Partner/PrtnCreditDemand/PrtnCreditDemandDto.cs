using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.PrtnCreditDemandServices
{
    public class PrtnCreditDemandDto : UpdatePrtnCreditDemandDlDto, ILinkToEntity<PrtnCreditDemand>, IDocument
    {
        public string Status { get; set; }
        public long ContractorId { get; set; }
        public long? CertificateId { get; set; }
        public long ApplicationId { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string PrtnCertificateDocNumber { get; set; }
        public DateOnly? PrtnCertificateDocOn { get; set; }
        public new int? BankId { get; set; }
        public string? Bank { get; set; }
        public string BusinessmanUser { get; set; }
        public string PrtnContractType { get; set; }
        public int? PrtnContractTypeId { get; set; }
        public string Address { get; set; }
        public int? NewVacanciesCount { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
    }
}
