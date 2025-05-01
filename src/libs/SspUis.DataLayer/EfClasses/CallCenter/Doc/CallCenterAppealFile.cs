using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_call_center_appeal_files")]
    [Index(nameof(OwnerId), Name = "ix_doc_call_center_appeal_files__owner")]
    public partial class CallCenterAppealFile : FileEntity<long>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(CallCenterAppeal.Files))]
        public virtual CallCenterAppeal Owner { get; set; }
    }
}
