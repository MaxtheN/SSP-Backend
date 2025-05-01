using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata.Tax
{
    [Table("tax_employee_count", Schema ="exapidata")]
    public class EmployeeCount
    {
        [Key]
        [Required]
        [Column("id")]
        public long Id { get; set; }

        [Column("tin")]
        [Required]
        public string Tin { get; set; }
        [Column("year")]
        public int Year { get; set; }
        [Column("month")]
        public int Month { get; set; }
        [Column("monthly_number_employees")]
        public decimal MonthlyNumberEmployees { get; set; }
        [Column("payment_tax")]
        public decimal? PaymentTax{ get; set; }
    }
}
