using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_video_lesson")]
    [Index(nameof(Number), Name = "doc_video_lesson_unique_index_code", IsUnique = true)]
    public partial class VideoLesson : IHaveIdProp<long>, IHaveStateId
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("number")]
        [StringLength(50)]
        public string Number { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Column("сategory_id")]
        public int CategoryId { get; set; }
        [Required]
        [Column("theme")]
        public string Theme { get; set; }
        [Column("tag")]
        [StringLength(500)]
        public string Tag { get; set; }
        [Required]
        [Column("uri")]
        public string Uri { get; set; }
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
        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(VideoCategory.VideoLessons))]
        public VideoCategory Category { get; set; }
    }
}
