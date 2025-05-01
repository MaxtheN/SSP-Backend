using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata.Investment
{
    [Table("investment_by_inn", Schema = "exapidata")]
    public class InvestmentByInn
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }


        [Column("tin")]
        [Required]
        public string Tin { get; set; }
        [Column("idn")]
        public string Idn { get; set; }


        [Column("doc_no")]
        public string DocNo { get; set; }
        [Column("doc_date")]
        public string DocDate { get; set; }


        [Column("contractor_uz_name")]
        public string ContractorUzName { get; set; }
        [Column("contractor_for_name")]
        public string ContractorForName { get; set; }
        [Column("contractor_for_country_code")]
        public string ContractorForCountryCode { get; set; }
        [Column("contractor_country")]
        public string ContractorCountry { get; set; }


        [Column("bank_id")]
        public string BankId { get; set; }
        [Column("bank_name")]
        public string BankName { get; set; }


        [Column("contract_status")]
        public int ContractStatus { get; set; }
        [Column("contract_status_name")]
        public string ContractStatusName { get; set; }


        [Column("contract_type")]
        public string ContractType { get; set; }
        [Column("contract_type_name")]
        public string ContractTypeName { get; set; }


        [Column("contract_subject")]
        public int ContractSubject { get; set; }
        [Column("contract_subject_name")]
        public string ContractSubjectName { get; set; }


        [Column("amount_1")]
        public decimal Amount1 { get; set; }
        [Column("amount_2")]
        public decimal Amount2 { get; set; }
    }
}
