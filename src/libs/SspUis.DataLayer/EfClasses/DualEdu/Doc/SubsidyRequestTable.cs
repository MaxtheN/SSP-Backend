using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("doc_subsidy_request_table", Schema = "dual_edu")]
[Index(nameof(OwnerId), Name = "ix_doc_subsidy_request_table__owner")]
public partial class SubsidyRequestTable : IHaveIdProp<long>
{
    public SubsidyRequestTable()
    {
        Files = new();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("person_id")]
    public int PersonId { get; set; }
    [Column("salary")]
    [Precision(18, 2)]
    public decimal Salary { get; set; }
    [Column("subsidy")]
    [Precision(18, 2)]
    public decimal Subsidy { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(SubsidyRequest.Tables))]
    public virtual SubsidyRequest Owner { get; set; }
    [ForeignKey(nameof(PersonId))]
    public virtual Person Person { get; set; }
    [InverseProperty(nameof(SubsidyRequestFile.SubsidyRequestTable))]
    public virtual List<SubsidyRequestFile> Files { get; set; }
}
