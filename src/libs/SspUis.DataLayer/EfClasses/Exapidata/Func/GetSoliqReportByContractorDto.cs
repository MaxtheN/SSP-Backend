using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace SspUis.DataLayer.EfClasses.Exapidata
{
    [Keyless]
    public class GetSoliqReportByContractorDto
    {
        [Column("contractor_count")]
        public long ContractorCount { get; set; }
        [Column("id")]
        public long? ContractorId { get; set; }
        [Column("inn")]
        public string ContractorInn { get; set; }
        [Column("short_name")]
        public string ContractorName { get; set; }

        [Column("region")]
        public string Region { get; set; }

        [Column("region_id")]
        public int? RegionId { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Column("district")]
        public string District { get; set; }


        [Column("month_1_number_employee")]
        public long NumberEmployee1 { get; set; }
        [Column("month_1_payment_tax")]
        public decimal PaymentTax1 { get; set; }

        [Column("month_2_number_employee")]
        public long NumberEmployee2 { get; set; }
        [Column("month_2_payment_tax")]
        public decimal PaymentTax2 { get; set; }

        [Column("month_3_number_employee")]
        public long NumberEmployee3 { get; set; }
        [Column("month_3_payment_tax")]
        public decimal PaymentTax3 { get; set; }

        [Column("month_4_number_employee")]
        public long NumberEmployee4 { get; set; }
        [Column("month_4_payment_tax")]
        public decimal PaymentTax4 { get; set; }

        [Column("month_5_number_employee")]
        public long NumberEmployee5 { get; set; }
        [Column("month_5_payment_tax")]
        public decimal PaymentTax5 { get; set; }

        [Column("month_6_number_employee")]
        public long NumberEmployee6 { get; set; }
        [Column("month_6_payment_tax")]
        public decimal PaymentTax6 { get; set; }

        [Column("month_7_number_employee")]
        public long NumberEmployee7 { get; set; }
        [Column("month_7_payment_tax")]
        public decimal PaymentTax7 { get; set; }

        [Column("month_8_number_employee")]
        public long NumberEmployee8 { get; set; }
        [Column("month_8_payment_tax")]
        public decimal PaymentTax8 { get; set; }

        [Column("month_9_number_employee")]
        public long NumberEmployee9 { get; set; }
        [Column("month_9_payment_tax")]
        public decimal PaymentTax9 { get; set; }

        [Column("month_10_number_employee")]
        public long NumberEmployee10 { get; set; }
        [Column("month_10_payment_tax")]
        public decimal PaymentTax10 { get; set; }

        [Column("month_11_number_employee")]
        public long NumberEmployee11 { get; set; }
        [Column("month_11_payment_tax")]
        public decimal PaymentTax11 { get; set; }

        [Column("month_12_number_employee")]
        public long NumberEmployee12 { get; set; }
        [Column("month_12_payment_tax")]
        public decimal PaymentTax12 { get; set; }

        [Column("total_number_employee")]
        public long TotalNumberEmployee { get; set; }
        [Column("total_payment_tax")]
        public decimal TotalPaymentTax { get; set; }


        [Column("period_1_net_income")]
        public decimal NetIncome1 { get; set; }
        [Column("period_2_net_income")]
        public decimal NetIncome2 { get; set; }
        [Column("period_3_net_income")]
        public decimal NetIncome3 { get; set; }
        [Column("period_4_net_income")]
        public decimal NetIncome4 { get; set; }
        [Column("total_net_income")]
        public decimal TotalNetIncome { get; set; }

        [Column("total_tax_debt")]
        public decimal TotalTaxDebt { get; set; }
    }

    [Keyless]
    public class GetTaxReportByContractorForPartnerDto
    {
        [Column("contractor_count")]
        public long ContractorCount { get; set; }
        [Column("id")]
        public long? ContractorId { get; set; }
        [Column("inn")]
        public string ContractorInn { get; set; }
        [Column("short_name")]
        public string ContractorName { get; set; }

        [Column("region")]
        public string Region { get; set; }

        [Column("region_id")]
        public int? RegionId { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Column("district")]
        public string District { get; set; }


        [Column("month_1_number_employee")]
        public long NumberEmployee1 { get; set; }
        [Column("month_1_payment_tax")]
        public decimal PaymentTax1 { get; set; }

        

        [Column("total_number_employee")]
        public long TotalNumberEmployee { get; set; }
        [Column("total_payment_tax")]
        public decimal TotalPaymentTax { get; set; }


        [Column("period_1_net_income")]
        public decimal NetIncome1 { get; set; }
        [Column("period_2_net_income")]
        public decimal NetIncome2 { get; set; }
        [Column("period_3_net_income")]
        public decimal NetIncome3 { get; set; }
        [Column("period_4_net_income")]
        public decimal NetIncome4 { get; set; }
        [Column("total_net_income")]
        public decimal TotalNetIncome { get; set; }

        [Column("total_tax_debt")]
        public decimal TotalTaxDebt { get; set; }
    }
}
