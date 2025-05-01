using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_application_group_table", Schema = "srv")]
    [Index(nameof(OwnerId), nameof(NeedChamberServiceId), Name = "ux_doc_service_application_table__ownerid_need_chamber_service_id", IsUnique = true)]
    public class ServiceApplicationTable : IHaveIdProp<long>
    {
        public ServiceApplicationTable()
        {
            Files = new HashSet<ServiceApplicationTableFile>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Column("is_completed")]
        public bool? IsCompleted { get; set; }

        [Required]
        [Column("need_chamber_service_id")]
        public int NeedChamberServiceId { get; set; }

        [Column("service_price_id")]
        public long? ServicePriceId { get; set; }

        [Column("service_price_table_id")]
        public long? ServicePriceTableId { get; set; }

        [StringLength(500)]
        [Column("offer_service_text")]
        public string OfferServiceText { get; set; }
        [StringLength(500)]
        [Column("for_boshqa_message")]
        public string ForBoshqaMessage { get; set; }

        [ForeignKey(nameof(NeedChamberServiceId))]
        public virtual NeedChamberService NeedChamberService { get; set; }

        [ForeignKey(nameof(ServicePriceId))]
        public virtual ServicePrice ServicePrice { get; set; }

        [ForeignKey(nameof(ServicePriceTableId))]
        public virtual ServicePriceTable ServicePriceTable { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ServiceApplicationGroup.Tables))]
        public virtual ServiceApplicationGroup Owner { get; set; }

        [InverseProperty(nameof(ServiceApplicationTableFile.Owner))]
        public virtual ICollection<ServiceApplicationTableFile> Files { get; set; }
    }
}