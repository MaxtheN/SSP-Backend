using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_sign_history")]
    public partial class SignHistory : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("document_id")]
        public long DocumentId { get; set; }
        [Column("data_for_sign")]
        public string DataForSign { get; set; }
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("doc_date", TypeName = "timestamp without time zone")]
        public DateTime DocDate { get; set; }
        [Column("user_info")]
        public string UserInfo { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("pkcs7info")]
        public byte[] Pkcs7info { get; set; }
        [Column("pc7key")]
        public byte[] Pc7key { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }
    }
}
