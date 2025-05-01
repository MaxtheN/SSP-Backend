using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class SaveAsExcelAllIntegrationReportByContractorDto
    {
        public string Contractor { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string? Mfy { get; set; }
        public string Inn { get; set; }
        public string ContractType { get; set; }

        public decimal? ProjectCost { get; set; }
        public decimal? OwnInvestment { get; set; }
        public decimal? PrivilegeBankCredit { get; set; }
        public decimal? ForeignInvestment { get; set; }
        public string? Bank { get; set; }

        public int ? NewVacanciesCount { get; set; }
        public string? ApplicationStatus { get; set; }
        public string? ContractStatus { get; set; }
        public string? CertificateStatus { get; set; }

        public string Oked { get; set; }

        public double? ApprovedSum { get; set; }    
        public double? RejectedSum { get; set; }
        public double? OtherSum { get; set; }

        public decimal KafillikAmount { get; set; }

        public long? SoliqSum { get; set; }
        public long? LandPropertySum { get; set;}
        public long? IncomeSum { get; set; }
        public long? SocialSum { get; set; }

        public int? GrChanContractorCount { get; set; }
    }
}
