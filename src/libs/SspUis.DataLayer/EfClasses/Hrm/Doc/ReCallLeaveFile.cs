using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;
[Table("doc_recall_leave_file", Schema = "hrm")]
public class ReCallLeaveFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(RecallLeave.Files))]
    public virtual RecallLeave Owner { get; set; }
    [Column("is_reject")]
    public bool? IsReject { get; set; }
}