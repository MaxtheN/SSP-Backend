using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_custom_job_action")]
    public partial class CustomJobAction : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Column("start_at", TypeName = "timestamp without time zone")]
        public DateTime StartAt { get; set; }

        [Column("end_at", TypeName = "timestamp without time zone")]
        public DateTime? EndAt { get; set; }

        [Column("input_data")]
        public string InputData { get; set; }

        [Column("return_data")]
        public string ReturnData { get; set; }

        [Column("is_success")]
        public bool IsSuccess { get; set; }

        [Column("from_cache")]
        public bool FromCache { get; set; }

        [Column("has_exception")]
        public bool HasException { get; set; }

        [Column("error")]
        public string Error { get; set; }

        [Column("user_message")]
        [StringLength(1000)]
        public string UserMessage{ get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }


        [ForeignKey(nameof(OwnerId))]
        public virtual CustomJob Owner { get; set; }
    }
}
