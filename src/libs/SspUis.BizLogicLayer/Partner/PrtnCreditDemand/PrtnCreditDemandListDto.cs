using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using SspUis.Core;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.PrtnCreditDemandServices
{
    public class PrtnCreditDemandListDto : DocumentListDto<long>, ILinkToEntity<PrtnCreditDemand>, IHaveIdProp<long>
    {
        public string DocNumber { get; set; }
        public string Status { get; set; }
        public string ImplementedProjectName { get; set; }
        public double ProjectCost { get; set; }
        public double OwnInvestment { get; set; }
        public double ForeignInvestment { get; set; }
        public double PrivillageBankCredit { get; set; }
        public string DistrictName { get; set; }
        public string RegionName { get; set; }
        public long ContractorId { get; set; }
        public long CertificateId { get; set; }
        public long ApplicationId { get; set; }
        public int RegionId { get; set; }
        public int DistrictId { get; set; }
        public new int? BankId { get; set; }
        public string? Bank { get; set; }
        public string? BankCode { get; set; }
        public string ContractorPhoneNumber { get; set; }
        public string PrtnContractType { get; set; }
        public int? PrtnContractTypeId { get; set; }
        public int? NewVacanciesCount { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string PrtnCertificateDocNumber { get; set; }
        public DateOnly? PrtnCertificateDocOn { get; set; }
        public string BusinessmanUser { get; set; }

    }

}
