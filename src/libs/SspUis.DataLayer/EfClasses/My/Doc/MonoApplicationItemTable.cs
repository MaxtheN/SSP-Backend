using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_mono_application_item_table", Schema = "my")]
    [Index(nameof(OwnerId), Name = "ix_doc_mono_application_item_table__owner")]
    public partial class MonoApplicationItemTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("education_item_currency_id")]
        public int EducationItemCurrencyId { get; set; }
        [Column("education_item_id")]
        public int EducationItemId { get; set; }
        [Column("education_item_count")]
        [Precision(18, 2)]
        public decimal EducationItemCount { get; set; }
        [Column("education_item_price")]
        [Precision(18, 2)]
        public decimal EducationItemPrice { get; set; }
        [Column("education_item_amount")]
        [Precision(18, 2)]
        public decimal EducationItemAmount { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(EducationItemId))]
        public virtual EducationItem EducationItem { get; set; }
        [ForeignKey(nameof(EducationItemCurrencyId))]
        public virtual Currency EducationItemCurrency { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual MonoApplication Owner { get; set; }
    }
}
