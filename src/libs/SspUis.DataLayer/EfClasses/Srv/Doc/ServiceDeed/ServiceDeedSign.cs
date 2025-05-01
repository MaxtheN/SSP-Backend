using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;
using System.Collections.Generic;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_deed_sign", Schema = "srv")]
    [Index(nameof(OwnerId), Name = "ix_doc_service_deed_sign__owner")]
    public class ServiceDeedSign : IHaveIdProp<long>
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

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual ServiceDeed Owner { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
    }
}
