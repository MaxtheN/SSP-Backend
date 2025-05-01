using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_business_activity_type")]
    public partial class BusinessActivityType : IHaveIdProp<long>
    {
        public BusinessActivityType()
        {
            Tables = new HashSet<BusinessActivityTypeTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("bank_id")]
        public int BankId { get; set; }
        [Column("help_amount")]
        [Precision(18, 2)]
        public decimal? HelpAmount { get; set; }
        [Column("real_employees_count")]
        [StringLength(255)]
        public string RealEmployeesCount { get; set; }
        [Column("business_ctor_id")]
        public int BusinessCtorId { get; set; }
        [Column("business_ctor_name")]
        [StringLength(255)]
        public string BusinessCtorName { get; set; }
        [Column("financial_help_id")]
        public int FinancialHelpId { get; set; }
        [Column("financial_help")]
        [StringLength(255)]
        public string FinancialHelp { get; set; }
        [Column("new_employees_count")]
        [StringLength(255)]
        public string NewEmployeesCount { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(BankId))]
        public virtual Bank Bank { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(BusinessCtorId))]
        public virtual BusinessSectorCategory BusinessCtor { get; set; }
        [InverseProperty(nameof(BusinessActivityTypeTable.Owner))]
        public virtual ICollection<BusinessActivityTypeTable> Tables { get; set; }
    }
}
