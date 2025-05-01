using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_item_of_expense", Schema = "hrm")]
    //[Index(nameof(Code1), nameof(Code2), nameof(Code3), Name = "uc_info_item_of_expense_another_code", IsUnique = true)]
    //[Index(nameof(Code), nameof(NumberOfGroup), Name = "uc_info_item_of_expense_code", IsUnique = true)]
    public partial class ItemOfExpense : IHaveIdProp<int>, IHaveStateId
    {
        public ItemOfExpense()
        {
            Translates = new HashSet<ItemOfExpenseTranslate>();
            Children = new HashSet<ItemOfExpense>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("number_of_group")]
        public int NumberOfGroup { get; set; }
        [Required]
        [Column("code")]
        [StringLength(7)]
        public string Code { get; set; }
        [Required]
        [Column("code1")]
        [StringLength(2)]
        public string Code1 { get; set; }
        [Required]
        [Column("code2")]
        [StringLength(3)]
        public string Code2 { get; set; }
        [Required]
        [Column("code3")]
        [StringLength(3)]
        public string Code3 { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("is_group")]
        public bool IsGroup { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("parent_id")]
        public int? ParentId { get; set; }
        [Column("ageing_allowed")]
        public bool AgeingAllowed { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public virtual ItemOfExpense Parent { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [InverseProperty(nameof(ItemOfExpenseTranslate.Owner))]
        public virtual ICollection<ItemOfExpenseTranslate> Translates { get; set; }
        [InverseProperty(nameof(Parent))]
        public virtual ICollection<ItemOfExpense> Children { get; set; }
    }
}
