using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_news_image", Schema = "my")]
    [Index(nameof(OwnerId), Name = "info_news_image_unique_owner_id", IsUnique = true)]
    public partial class NewsImage : FileEntity<int>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(News.Image))]
        public virtual News Owner { get; set; }
    }
}
