using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_prtn_contract_type", Schema = "partner")]
    public partial class PrtnContractType : IHaveIdProp<int>, IHaveStateId
    {
        public PrtnContractType()
        {
            Translates = new HashSet<PrtnContractTypeTranslate>();
            Tables = new HashSet<PrtnContractTypeTable>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(250)]
        public string FullName { get; set; }
        [Column("employee_range_from")]
        public int EmployeeRangeFrom { get; set; }
        [Column("employee_range_to")]
        public int? EmployeeRangeTo { get; set; }
        [Column("certificate_period_in_years")]
        public int CertificatePeriodInYears { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(PrtnContractTypeTable.Owner))]
        public virtual ICollection<PrtnContractTypeTable> Tables { get; set; }
        [InverseProperty(nameof(PrtnContractTypeTranslate.Owner))]
        public virtual ICollection<PrtnContractTypeTranslate> Translates { get; set; }
    }
}
