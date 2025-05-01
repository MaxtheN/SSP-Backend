using SspUis.DataLayer.EfClasses.Appeal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_external_document_from_edoc", Schema = "appeal")]
public partial class ExternalDocumentFromEdoc : IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("term_execution", TypeName = "timestamp without time zone")]
    public DateTime? TermExecution { get; set; }
    [Column("assignment")]
    [StringLength(250)]
    public string Assignment { get; set; }
    [Column("organization_id")]
    public int? OrganizationId { get; set; }
    [Column("proces_id")]
    public int? ProcessId { get; set; }
    [Column("appeal_application_id")]
    public long? AppealAplicationtId { get; set; }
    [Column("call_center_appeal_id")]
    public long? CallCenterAppealId { get; set; }
    [Column("reg_number")]
    [StringLength(50)]
    public string RegNumber { get; set; }
    [Column("reg_date", TypeName = "timestamp without time zone")]
    public DateTime? RegDate { get; set; }
    [Column("outgoing_doc_create_data", TypeName = "timestamp without time zone")]
    public DateTime? OutgoingDocCreatedData { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }
    [ForeignKey(nameof(AppealAplicationtId))]
    public virtual AppealApplication AppealApplication { get; set; }
    [ForeignKey(nameof(CallCenterAppealId))]
    public virtual CallCenterAppeal CallCenterAppeal { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
}


