using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("doc_subsidy_request_file", Schema = "dual_edu")]
[Index(nameof(OwnerId), Name = "ix_doc_subsidy_request_file__owner")]
public partial class SubsidyRequestFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [Column("subsidy_request_table_id")]
    public long? SubsidyRequestTableId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(SubsidyRequest.Files))]
    public virtual SubsidyRequest Owner { get; set; }
    [ForeignKey(nameof(SubsidyRequestTableId))]
    public virtual SubsidyRequestTable SubsidyRequestTable { get; set; }
}
