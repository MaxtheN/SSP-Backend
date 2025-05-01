using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_video_category")]
    [Index(nameof(Code), Name = "info_video_category_unique_index_code", IsUnique = true)]
    public partial class VideoCategory : IHaveStateId, IHaveIdProp<int>
    {
        public VideoCategory()
        {
            Translates = new HashSet<VideoCategoryTranslate>();
            VideoLessons = new HashSet<VideoLesson>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(500)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
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
        [InverseProperty(nameof(VideoLesson.Category))]
        public virtual ICollection<VideoLesson> VideoLessons { get; set; }
        [InverseProperty(nameof(VideoCategoryTranslate.Owner))]
        public virtual ICollection<VideoCategoryTranslate> Translates { get; set; }
    }
}
