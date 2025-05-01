using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_payment_order_file", Schema = "public")]
    [Index(nameof(OwnerId), Name = "ix_doc_memship_payment_order_file_files__owner")]
    public partial class MemshipPaymentOrderFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MemshipPaymentOrder.Files))]
        public virtual MemshipPaymentOrder Owner { get; set; }
    }
}
