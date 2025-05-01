using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_number")]
    public partial class Number : IHaveIdProp<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("document")]
        [StringLength(100)]
        public string Document { get; set; }
        [Column("current_number")]
        public int CurrentNumber { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("finance_year")]
        public int FinanceYear { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }


        public static Number Create(string document, int organizationId, int financeYear)
        {
            return new Number
            {
                Document = document,
                OrganizationId = organizationId,
                FinanceYear = financeYear
            };
        }
    }
}
