using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;
using WEBASE.EF;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_news_tag", Schema = "my")]
    public partial class NewsTag : IHaveIdProp<int>, IHaveSingleUniqueForeignKey<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("news_id")]
        public int NewsId { get; set; }
        [Column("tag_id")]
        public int TagId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(NewsId))]
        [InverseProperty(nameof(EfClasses.News.Tags))]
        public virtual News News { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty(nameof(EfClasses.Tag.NewsTags))]
        public virtual Tag Tag { get; set; }

        public object GetUniqueForeignKey() => TagId;

        public void SetUniqueForeignKey(int foreignKey) => TagId = foreignKey;
    }
}
