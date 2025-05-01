using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
	[Table("doc_memship_payment_order_table", Schema = "public")]
	[Index(nameof(OwnerId), Name = "ix_doc_memship_payment_order_table__owner")]
	public partial class MemshipPaymentOrderTable : IHaveIdProp<long>
	{
		[Column("id")]
        public long Id { get; set; }
        [Required]
		[Column("owner_id")]
        public long OwnerId { get; set; }
		[Column("amount")]
        public decimal Amount { get; set; }
		[Column("application_type_id")]
        public int ApplicationTypeId { get; set; }
		[Column("need_chamber_service_id")]
        public int NeedChamberServiceId { get; set; }
        [ForeignKey(nameof(ApplicationTypeId))]
		public virtual ApplicationType ApplicationType { get; set; }
		[ForeignKey(nameof(NeedChamberServiceId))]
		public virtual NeedChamberService NeedChamberService { get; set; }
		[ForeignKey(nameof(OwnerId))]
		[InverseProperty(nameof(MemshipPaymentOrder.Tables))]
		public virtual MemshipPaymentOrder Owner { get; set; }
	}
}
