using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Memship;

[Keyless]
public class MemshipReportFuncDto
{
    [Column("region_id")]
    public int? RegionId { get; set; }
    [Column("region")]
    public string Region { get; set; }
    [Column("region_order_code")]
    public string RegionOrderCode { get; set; }
    [Column("district_id")]
    public int? DistrictId { get; set; }
    [Column("district")]
    public string District { get; set; }
    [Column("district_order_code")]
    public string DistrictOrderCode { get; set; }

    [Column("memship_general_plan")]
    public int MemshipGeneralPlan { get; set; }
    [Column("memship_fact_by_month")]
    public int MemshipFactByMonth { get; set; }
    [Column("memship_fact_by_month_percentage")]
    public int MemshipFactByMonthPercentage { get; set; }

    [Column("memship_application_legal")]
    public int MemshipApplicationLegal { get; set; }
    [Column("memship_application_ytt")]
    public int MemshipApplicationYtt { get; set; }
    [Column("memship_certificate_legal")]
    public int MemshipCertificateLegal { get; set; }
    [Column("memship_certificate_ytt")]
    public int MemshipCertificateYtt { get; set; }

    [Column("memship_general_plan_by_year")]
    public int MemshipGeneralPlanByYear { get; set; }
    [Column("memship_fact_by_year")]
    public int MemshipFactByYear { get; set; }
    [Column("memship_fact_by_year_percentage")]
    public int MemshipFactByYearPercentage { get; set; }

    [Column("memship_application_count_by_year")]
    public int MemshipApplicationCountByYear { get; set; }
    [Column("memship_contract_count_by_year")]
    public int MemshipContractCountByYear { get; set; }

    [Column("memship_certificate_accepted_count_by_year")]
    public int MemshipCertificateAcceptedCountByear { get; set; }
    [Column("memship_certificate_progress_count_by_year")]
    public int MemshipCertificateProgressCountByear { get; set; }
    [Column("memship_certificate_not_included_count_by_year")]
    public long MemshipCertificateNotIncludedCountByear { get; set; }

    [Column("memship_general_indebtedness_by_year")]
    public int MemshipGeneralIndebtednessByYear { get; set; }
    [Column("memship_general_indebtedness_percentage_by_year")]
    public int MemshipGeneralIndebtednessPercentageByYear { get; set; }
    [Column("memship_general_indebtedness_coefficient_by_year")]
    public float MemshipGeneralIndebtednessCoefficientByYear { get; set; }

}
