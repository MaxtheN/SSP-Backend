using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Report
{
    [Keyless]
    public class PrtnCreditDemandInfoByBankDto
    {
        [Column("contractor_count")]
        public long ContractorCount { get; set; }

        [Column("contractor_id")]
        public long? ContractorId { get; set; }
        [Column("contractor_inn")]
        public string ContractorInn { get; set; }
        [Column("contractor_full_name")]
        public string Contractor { get; set;}

        [Column("region")]
        public string Region { get; set;}
        [Column("region_id")]
        public int? RegionId { get; set; }

        [Column("district")]
        public string District { get; set; }
        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Column("bank")]
        public string Bank{ get; set; }
        [Column("bank_id")]
        public int? BankId { get; set;}

        [Column("main_bank")]
        public string MainBank { get; set; }
        [Column("main_bank_id")]
        public int? MainBankId { get; set; }

        [Column("contract_type_id")]
        public int? ContractorTypeId { get; set; }
        [Column("contract_type_name")]
        public string ContractorType { get; set; }

        [Column("new_vacancies_count")]
        public long NewVacanciesCount { get; set; }

        [Column("project_cost")]
        [Precision(18, 2)]
        public decimal ProjectCost { get; set; }
        [Column("own_investment")]
        [Precision(18, 2)]
        public decimal OwnInvestment { get; set; }
        [Column("foreign_investment")]
        [Precision(18, 2)]
        public decimal ForeignInvestment { get; set; }
        [Column("privilege_bank_credit")]
        [Precision(18, 2)]
        public decimal PrivilegeBankCredit { get; set; }
    }
}
