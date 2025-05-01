using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Report
{
    [Keyless]
    public class TaxCreditreportDto
    {
        [Column("certificate_count")]
        public long CertificateCount { get; set; }
        [Column("new_vacancies_count")]
        public long NewVacanciesCount { get; set; }
        [Column("total_application_count")]
        public long TotalApplicationCount { get; set; }
        [Column("contractor_full_name")]
        public string ContractorFullName { get; set; }

        [Column("contractor_id")]
        public long? ContractorId { get; set; }
        [Column("contractor_inn")]
        public string ContractorInn { get; set; }



        [Column("income_new_vacancies_count")]
        public long IncomeNewVacanciesCount { get; set; }

        [Column("income_tax_sum")]
        public long IncomeTaxSum { get; set; }
        
        [Column("income_tax_count")]
        public long IncomeTaxCount { get; set; }

        [Column("contractor_income_tax_count")]
        public long ContractorIncomeTaxCount { get; set; }


        [Column("land_new_vacancies_count")]
        public long LandNewVacanciesCount { get; set; }

        [Column("land_tax_sum")]
        public long LandTaxSum { get; set; }

        [Column("land_tax_count")]
        public long LandTaxCount { get; set; }

        [Column("contractor_land_tax_count")]
        public long ContractorLandTaxCount { get; set; }


        [Column("property_new_vacancies_count")]
        public long PropertyNewVacanciesCount { get; set; }

        [Column("property_tax_sum")]
        public long PropertyTaxSum { get; set; }
        [Column("property_tax_count")]
        public long PropertyTaxCount { get; set; }

        [Column("contractor_property_tax_count")]
        public long ContractorPropertyTaxCount { get; set; }


        [Column("social_new_vacancies_count")]
        public long SocialNewVacanciesCount { get; set; }

        [Column("social_tax_sum")]
        public long SocialTaxSum { get; set; }

        [Column("social_tax_count")]
        public long SocialTaxCount { get; set; }

        [Column("contractor_social_tax_count")]
        public long ContractorSocialTaxCount { get; set; }


        [Column("privilege_new_vacancies_count")]
        public long PrivilegeNewVacanciesCount { get; set; }

        [Column("privilege_tax_sum")]
        public long PrivilegeTaxSum { get; set; }

        [Column("privilege_tax_count")]
        public long PrivilegeTaxCount { get; set; }

        [Column("contractor_privilege_tax_count")]
        public long ContractorPrivilegeTaxCount { get; set; }


        [Column("total_contractor_application_count")]
        public long TotalContractApplicationCount { get; set; }


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
    }
}