using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_prtn_application_graph", Schema = "partner")]
    [Index(nameof(OwnerId), nameof(YearIn), nameof(MonthIn), Name = "ux_doc_prtn_application_graph__owner", IsUnique = true)]
    public partial class PrtnApplicationGraph : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("year_in")]
        public int YearIn { get; set; }
        [Column("month_in")]
        public int MonthIn { get; set; }
        [Column("new_vacancies_count")]
        public int NewVacanciesCount { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual PrtnApplication Owner { get; set; }
    }
}
