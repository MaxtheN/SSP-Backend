using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class PrtnContractListDto : DocumentListDto<long>, ILinkToEntity<PrtnContract>, IHaveIdProp<long>
    {
        public Guid Id2 { get; set; }
        public int TableId { get { return TableIdConst.DOC_PRTN_CONTRACT; } }
        public string DocNumber { get; set; }
        public string Status { get; set; }
        public long ContractorId { get; set; }
        public string ContractorInn { get; set; }
        public string Contractor { get; set; }
        public DateOnly? ContractorRegestrationDate { get; set; }
        public int ContractorRegionId { get; set; }
        public string ContractorRegion { get; set; }
        public int ContractorDistrictId { get; set; }
        public string ContractorDistrict { get; set; }
        public long? MfyId { get; set; }
        public string Mfy { get; set; }
        public int PrtnContractTypeId { get; set; }
        public string PrtnContractType { get; set; }
        public int NewVacanciesCount { get; set; }
        public long ApplicationId { get; set; }
        public string ApplicationDocOn { get; set; }
        public string ApplicationDocNumber { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public string OrganizationInn { get; set; }
        public int? PrtnCertificateStatusId { get; set; }
        public string PrtnCertificateStatus { get; set; }
        public long? PrtnCertificateId { get; set; }
        public string OkedCode { get; set; }
        public string Oked { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<PrtnContractListSignedDto> Signed { get; set; }
        public IEnumerable<PrtnContractListSignedDto> NotSigned { get; set; }
        public string NextSigner { get; set; }
        public string LastSigner { get; set; }
        public bool ContractorHasGovShare { get; set; }
        public decimal? ContractorGovShare { get; set; }
        public bool IsRead { get; set; }
    }

    public class PrtnContractListSignedDto
    {
        public int OrderNumber { get; set; }
        public string FullName { get; set; }
        public string Fio { get; set; }
        public DateTime? SignedAt { get; set; }
    }

}
