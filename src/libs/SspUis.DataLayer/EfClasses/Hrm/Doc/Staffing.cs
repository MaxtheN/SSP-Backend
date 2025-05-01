using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_staffing", Schema = "hrm")]
    [Index(nameof(OrganizationId), nameof(StatusId), nameof(DocOn), Name = "ix_doc_staffing__org")]
    public partial class Staffing : IHaveIdProp<long>, IHaveStatusId
    {
        public Staffing()
        {
            IndicatorValues = new HashSet<StaffingIndicatorValue>();
            Positions = new HashSet<StaffingPosition>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(250)]
        public string DocNumber { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("doc_sum")]
        [Precision(18, 2)]
        public decimal DocSum { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("finance_year")]
        public int FinanceYear { get; set; }
        [Column("for_months")]
        public int ForMonths { get; set; }
        [Column("staffing_type_id")]
        public int StaffingTypeId { get; set; }
        [Column("org_settlement_account_id")]
        public long? OrgSettlementAccountId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("staffing_template_id")]
        public long? StaffingTemplateId { get; set; }
        [Column("settlement_account_source_id")]
        public int? SettlementAccountSourceId { get; set; }
        [Column("level_code_id")]
        public int? LevelCodeId { get; set; }
        [Column("source_code_id")]
        public int? SourceCodeId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(LevelCodeId))]
        public virtual LevelCode LevelCode { get; set; }
        [ForeignKey(nameof(OrgSettlementAccountId))]
        public virtual OrganizationSettlementAccount OrgSettlementAccount { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(SettlementAccountSourceId))]
        public virtual SettlementAccountSource SettlementAccountSource { get; set; }
        [ForeignKey(nameof(SourceCodeId))]
        public virtual SourceCode SourceCode { get; set; }
        [ForeignKey(nameof(StaffingTemplateId))]
        public virtual StaffingTemplate StaffingTemplate { get; set; }
        [ForeignKey(nameof(StaffingTypeId))]
        public virtual StaffingType StaffingType { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(StaffingIndicatorValue.Owner))]
        public virtual ICollection<StaffingIndicatorValue> IndicatorValues { get; set; }
        [InverseProperty(nameof(StaffingPosition.Owner))]
        public virtual ICollection<StaffingPosition> Positions { get; set; }
    }
}
