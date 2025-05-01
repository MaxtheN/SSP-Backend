using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Report.Func
{
    [Keyless]
    public class BojxonaImtiyozReportByContractorDto
    {
        [Column("certificate_count")]
        public long CertificateCount { get; set; }
        
        [Column("new_vacancies_count")]
        public long NewVacanciesCount { get; set; }
        
        [Column("total_contractor_count")]
        public long TotalContractorCount { get; set; }

        [Column("total_count")]
        public long TotalCount { get; set; }

        [Column("contractor_full_name")]
        public string ContractorFullName { get; set; }
        
        [Column("contractor_id")]
        public long? ContractorId { get; set; }
        
        [Column("contractor_inn")]
        public string ContractorInn { get; set; }
        
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

        [Column("app_count")]
        public int AppCount { get; set; }

        [Column("app_contractor_count")]
        public int AppContractorCount { get; set; }

        [Column("rej_count")]
        public int RejCount { get; set; }

        [Column("rej_contractor_count")]
        public int RejContractorCount { get; set; }

        [Column("sum")]
        public decimal Sum { get; set; }

        [Column("gr_chan_count")]
        public int GrChanCount { get; set; }

        [Column("gr_chan_contractor_count")]
        public int GrChanContractorCount { get; set; }


        [Column("dev_count")]
        public int DevCount { get; set; }

        [Column("dev_contractor_count")]
        public int DevContractorCount { get; set; }

        [Column("gr_chan_new_vacancies_count")]
        public int GrChanNewVacanciesCount { get; set; }
    }
}
