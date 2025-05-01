using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE;
using WEBASE.Models;
using WEBASE.Utility;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_news", Schema = "my")]
    public partial class News : IHaveIdProp<int>, IHaveStateId
    {
        public News()
        {
            Translates = new HashSet<NewsTranslate>();
            Tags = new HashSet<NewsTag>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("date", TypeName = "timestamp without time zone")]
        public DateTime Date { get; set; }
        [Column("view_count")]
        public int ViewCount { get; set; }
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
        [Column("short_content")]
        public string ShortContent { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        public virtual NewsImage Image { get; set; }
        [InverseProperty(nameof(NewsTag.News))]
        public virtual ICollection<NewsTag> Tags { get; set; }
        [InverseProperty(nameof(NewsTranslate.Owner))]
        public virtual ICollection<NewsTranslate> Translates { get; set; }
    }
}
