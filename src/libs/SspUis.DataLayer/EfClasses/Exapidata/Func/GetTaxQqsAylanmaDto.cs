using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata;
[Keyless]
public class GetTaxQqsAylanmaDto
{
    
    [Column("contractor_id")]
    public long? ContractorId { get; set; }
    [Column("contractor_inn")]
    public string ContractorInn { get; set; }
    [Column("contractor_full_name")]
    public string ContractorFullName { get; set; }

    [Column("contractor_count")]
    public int ContractorCount { get; set; }

    [Column("region")]
    public string Region { get; set; }

    [Column("region_id")]
    public int? RegionId { get; set; }

    [Column("district_id")]
    public int? DistrictId { get; set; }

    [Column("district")]
    public string District { get; set; }


    [Column("net_income_without_vat_1")]
    public decimal NetIncomeWithOutVat1 { get; set; }
    [Column("vat_sum_1")]
    public decimal VatSum1 { get; set; }

    [Column("net_income_without_vat_2")]
    public decimal NetIncomeWithOutVat2 { get; set; }
    [Column("vat_sum_2")]
    public decimal VatSum2 { get; set; }

    [Column("net_income_without_vat_3")]
    public decimal NetIncomeWithOutVat3 { get; set; }
    [Column("vat_sum_3")]
    public decimal VatSum3 { get; set; }
    [Column("p1_net_income_without_vat")]
    public decimal PNetIncomeWithoutvat1 { get; set; }
    [Column("p1_vat_sum")]
    public decimal PVatSum1 { get; set; }




    [Column("net_income_without_vat_4")]
    public decimal NetIncomeWithOutVat4 { get; set; }
    [Column("vat_sum_4")]
    public decimal VatSum4 { get; set; }

    [Column("net_income_without_vat_5")]
    public decimal NetIncomeWithOutVat5 { get; set; }
    [Column("vat_sum_5")]
    public decimal VatSum5 { get; set; }

    [Column("net_income_without_vat_6")]
    public decimal NetIncomeWithOutVat6 { get; set; }
    [Column("vat_sum_6")]
    public decimal VatSum6 { get; set; }
    [Column("p2_net_income_without_vat")]
    public decimal PNetIncomeWithoutvat2 { get; set; }
    [Column("p2_vat_sum")]
    public decimal PVatSum2 { get; set; }



    [Column("net_income_without_vat_7")]
    public decimal NetIncomeWithOutVat7 { get; set; }
    [Column("vat_sum_7")]
    public decimal VatSum7 { get; set; }
    [Column("net_income_without_vat_8")]
    public decimal NetIncomeWithOutVat8 { get; set; }
    [Column("vat_sum_8")]
    public decimal VatSum8 { get; set; }

    [Column("net_income_without_vat_9")]
    public decimal NetIncomeWithOutVat9 { get; set; }
    [Column("vat_sum_9")]
    public decimal VatSum9 { get; set; }
    [Column("p3_net_income_without_vat")]
    public decimal PNetIncomeWithoutvat3 { get; set; }
    [Column("p3_vat_sum")]
    public decimal PVatSum3 { get; set; }



    [Column("net_income_without_vat_10")]
    public decimal NetIncomeWithOutVat10 { get; set; }
    [Column("vat_sum_10")]
    public decimal VatSum10 { get; set; }
    [Column("net_income_without_vat_11")]
    public decimal NetIncomeWithOutVat11 { get; set; }
    [Column("vat_sum_11")]
    public decimal VatSum11 { get; set; }
    [Column("net_income_without_vat_12")]
    public decimal NetIncomeWithOutVat12 { get; set; }
    [Column("vat_sum_12")]
    public decimal VatSum12 { get; set; }

    [Column("p4_net_income_without_vat")]
    public decimal P4NetIncomeWithoutvat { get; set; }
    [Column("p4_vat_sum")]
    public decimal P4VatSum { get; set; }

    [Column("total_net_income_without_vat")]
    public decimal TotalNetIncomeWithoutvat { get; set; }
    [Column("total_vat_sum")]
    public decimal TotalVatSum { get; set; }
}


