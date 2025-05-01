using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Report.Func
{
    [Keyless]
    public class BankCreditApplicationReportByRegionAndDistrictDto
    {
        [Column("region")]
        public string Region { get; set; }
        [Column("region_id")]
        public int? RegionId { get; set; }

        [Column("district")]
        public string District { get; set; }
        [Column("district_id")]
        public int? DistrictId { get; set; }
        [Column("contract_type_id")]
        public int? ContractTypeId { get; set; }
        [Column("contract_type")]
        public string? ContractType { get; set; }

        [Column("certificate_count")]
        public long CertificateCount { get; set; }
        [Column("new_vacancies_count")]
        public long NewVacanciesCount { get; set; }

        [Column("contractor_count")]
        public long ContractorCount { get; set; }
        
        [Column("contractor_full_name")]
        public string ContractorFullName { get; set; }

        [Column("contractor_id")]
        public long? ContractorId { get; set; }
        [Column("contractor_inn")]
        public string ContractorInn { get; set; }


        [Column("submitted_new_vacancies_count")]
        public long SubmittedNewVacanciesCount { get; set; }
        [Column("submitted_count")]
        public long SubmittedCount { get; set; }
        [Column("submitted_contractor_count")]
        public long SubmittedContractorCount { get; set; }
        [Column("submitted_sum")]
        public decimal SubmittedSum{ get; set; }


        [Column("approved_new_vacancies_count")]
        public long ApprovedNewVacanciesCount { get; set; }
        [Column("approved_count")]
        public long ApprovedCount { get; set; }
        [Column("approved_contractor_count")]
        public long ApprovedContractorCount { get; set; }
        [Column("approved_sum")]
        public decimal ApprovedSum { get; set; }


        [Column("issuance_new_vacancies_count")]
        public long IssuanceNewVacanciesCount { get; set; }
        [Column("issuance_count")]
        public long IssuanceCount { get; set; }
        [Column("issuance_contractor_count")]
        public long IssuanceContractorCount { get; set; }
        [Column("issuance_sum")]
        public decimal IssuanceSum { get; set; }


        [Column("canceled_new_vacancies_count")]
        public long CanceledNewVacanciesCount { get; set; }
        [Column("canceled_count")]
        public long CanceledCount { get; set; }
        [Column("canceled_contractor_count")]
        public long CanceledContractorCount { get; set; }
        [Column("canceled_sum")]
        public decimal CanceledSum { get; set; }


        [Column("rejected_new_vacancies_count")]
        public long RejectedNewVacanciesCount { get; set; }
        [Column("rejected_count")]
        public long RejectedCount { get; set; }
        [Column("rejected_contractor_count")]
        public long RejectedContractorCount { get; set; }
        [Column("rejected_sum")]
        public decimal RejectedSum { get; set; }

    }
}
