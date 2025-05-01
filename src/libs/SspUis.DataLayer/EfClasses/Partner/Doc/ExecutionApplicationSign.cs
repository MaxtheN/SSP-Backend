using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_execution_application_sign", Schema = "partner")]
    public partial class ExecutionApplicationSign : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
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

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ExecutionApplication.Signs))]
        public virtual ExecutionApplication Owner { get; set; }
    }
}
