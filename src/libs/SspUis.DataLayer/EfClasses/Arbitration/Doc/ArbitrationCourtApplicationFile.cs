using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_arbitration_court_application_files", Schema = "arbitration")]
[Index(nameof(OwnerId), Name = "ix_doc_service_application_table_files__owner")]
public partial class ArbitrationCourtApplicationFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [Column("column_name")]
    [StringLength(100)]
    public string ColumnName { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ArbitrationCourtApplication.Files))]
    public virtual ArbitrationCourtApplication Owner { get; set; }
    [Column("can_sign")]
    public bool CanSign { get; set; } = false;
    [Column("step_id")]
    public int StepId { get; set; }
    [Column("is_created_erp")]
    public bool IsCreatedErp { get; set; }
}
