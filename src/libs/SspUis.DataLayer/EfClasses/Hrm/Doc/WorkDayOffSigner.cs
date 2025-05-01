using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_work_day_off_signer", Schema = "hrm")]
    [Index(nameof(OwnerId), nameof(SignOrder), Name = "ux_doc_work_day_off_signer", IsUnique = true)]
    public partial class WorkDayOffSigner : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("sign_order")]
        public int SignOrder { get; set; }
        [Column("department_id")]
        public int DepartmentId { get; set; }
        [Column("position_id")]
        public int PositionId { get; set; }
        [Column("employee_manage_id")]
        public long EmployeeManageId { get; set; }
        [Column("is_hr")]
        public bool IsHr { get; set; }
        [Column("is_director")]
        public bool IsDirector { get; set; }
        [Column("sign_file")]
        public Guid? SignFile { get; set; }
        [Column("data_file")]
        public Guid? DataFile { get; set; }
        [Column("signed_user_info")]
        public string SignedUserInfo { get; set; }
        [Column("signed_at", TypeName = "timestamp without time zone")]
        public DateTime? SignedAt { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual WorkDayOff Owner { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual Position Position { get; set; }
    }
}
