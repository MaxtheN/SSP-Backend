using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("sys_document_template_file")]
public class DocumentTempleteFile : IFileEntity, IHaveIdProp<Guid>
{
    [Key]
    [Column("id")]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column("file_name")]
    [StringLength(100)]
    public string FileName { get; set; }

    [Required]
    [Column("file_extension")]
    [StringLength(50)]
    public string FileExtension { get; set; }

    [Column("table_id")]
    public int TableId { get; set; }
    [Column("language_id")]
    public int? LanguageId { get; set; }

    [Column("status_id")]
    public int? StatusId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }

    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }

    [ForeignKey(nameof(TableId))]
    public virtual Table Table { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
}
