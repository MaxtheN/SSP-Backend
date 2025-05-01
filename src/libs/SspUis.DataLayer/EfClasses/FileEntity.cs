using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.EF;

namespace SspUis.DataLayer.EfClasses
{
    public abstract class FileEntity<TOwnerId> : IFileEntity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        [Required]
        [Column("file_name")]
        [StringLength(100)]
        public string FileName { get; set; }
        [Required]
        [Column("file_extension")]
        [StringLength(50)]
        public string FileExtension { get; set; }
        [Column("owner_id")]
        public TOwnerId OwnerId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
    }
}
