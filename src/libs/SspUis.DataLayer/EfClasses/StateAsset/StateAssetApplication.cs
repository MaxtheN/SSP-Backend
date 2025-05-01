using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_state_asset_application", Schema = "state_asset")]
    public partial class StateAssetApplication : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("prtn_certificate_id")]
        public long PrtnCertificateId { get; set; }
        [Column("auction_doc_on")]
        public DateOnly? AuctionDocOn { get; set; }
        [Column("auction_doc_number")]
        [StringLength(50)]
        public string AuctionDocNumber { get; set; }
        [Column("state_asset_name")]
        [StringLength(1024)]
        public string StateAssetName { get; set; }
        [Column("state_asset_status_id")]
        public int? StateAssetStatusId { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(PrtnCertificateId))]
        public virtual PrtnCertificate PrtnCertificate { get; set; }
    }
}
