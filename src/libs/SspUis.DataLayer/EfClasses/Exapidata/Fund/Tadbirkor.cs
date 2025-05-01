using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Exapidata.Fund
{
    [Table("fund_tadbirkor", Schema = "exapidata")]
    public class FundTadbirkor
    {
        public FundTadbirkor()
        {
            Credits = new HashSet<FundTadbirkorCredit>();
        }

        [Column("id")]
        [Key]
        [Required]
        public long Id { get; set; }

        //[Required]
        [Column("external_id")]
        public int ExternalId { get; set; }

        //[Required]
        [Column("date_on")]
        public DateOnly DateOn { get; set; }

        [Required]
        [Column("tin_pinfl")]
        public string TinPinfl { get; set; } 

        //[Required]
        [Column("name")]
        public string Name { get; set; }

        //[Required]
        [Column("business_sector_id")]
        public int BusinessSectorId { get; set; }

        [Column("business_sector_name")]
        public string BusinessSectorName { get; set; } 

        [Column("business_sector_type_id")]
        public int? BusinessSectortypeid { get; set; }

        [Column("business_sector_type_name")]
        public string? BusinessSectorTypeName { get; set; }

        [Column("financial_asistance_id")]
        public int FinancialAssistanceId { get; set; }

        [Column("financial_asistance_name")]
        public string FinancialAsistanceName { get; set; } 

        [Column("bank_code")]
        public string BankCode { get; set; } 

        [Column("bank_name")]
        public string BankName { get; set; } 

        [Column("region_soato")]
        public string RegionSoato { get; set; }

        [Column("region_name")]
        public string RegionName { get; set; } 

        [Column("district_soato")]
        public string DistrictSoato { get; set; }

        [Column("district_name")]
        public string DistrictName { get; set; }

        [Column("aid_amount")]
        //[Required]
        public decimal AidAmount { get; set; }

        [Column("new_job_position")]
        public string NewJobPosition { get; set; } 

        [Column("real_job_position")]
        public string RealJobPosition { get; set; }

        [InverseProperty(nameof(FundTadbirkorCredit.Owner))]
        public virtual ICollection<FundTadbirkorCredit> Credits { get; set; }
    }
}
