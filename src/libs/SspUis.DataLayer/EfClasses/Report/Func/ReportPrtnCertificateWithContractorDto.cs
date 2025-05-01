using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Report.Func
{
    [Keyless]
    public class ReportPrtnCertificateWithContractorDto
    {
        [Column("region_id")]
        public int? RegionId { get; set; }
        [Column("region_name")]
        public string Region { get; set; }
        
        [Column("district_id")]
        public int? DistrictId { get; set; }
        [Column("district_name")]
        public string District { get; set; }
        [Column("contract_id")]
        public long? ContractorId { get; set; }
        [Column("cotractor_name")]
        public string Contractor { get; set; }
        [Column("contractor_count")]
        public int? ContractorCount { get; set; }
        [Column("vacance_not_more100")]
        public int? VacanceNotMore100 { get; set; }
        [Column("vacance_not_more200")]
        public int? VacanceNotMore200 { get; set; }
        [Column("vacance_more200")]
        public int? VacanceMore200 { get; set; }
        [Column("job_count")]
        public int? JobCount { get; set; }

    }
    [Keyless]
    public class ReportPrtnExecutionApplicationContractorDto
    {
        [Column("region_id")]
        public int? RegionId { get; set; }
        [Column("region_name")]
        public string Region { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }
        [Column("district_name")]
        public string District { get; set; }
        [Column("contract_id")]
        public long? ContractorId { get; set; }
        [Column("cotractor_name")]
        public string Contractor { get; set; }
        [Column("contractor_count")]
        public int? ContractorCount { get; set; }
        [Column("average_salary")]
        public decimal? AverageSalary { get; set; }
        [Column("vacance_not_more100")]
        public int? VacanceNotMore100 { get; set; }
        [Column("vacance_not_more200")]
        public int? VacanceNotMore200 { get; set; }
        [Column("vacance_more200")]
        public int? VacanceMore200 { get; set; }
        [Column("job_count")]
        public int? JobCount { get; set; }
    }
	[Keyless]
	public class RegionOrDistrict
	{
		[Column("region_name")]
		public string? RegionName { get; set; }
		[Column("district_name")]
		public string? DistrictName { get; set; }
		[Column("region_id")]
		public int? RegionId { get; set; }
		[Column("district_id")]
		public int? DistrictId { get; set; }
	}

}
