using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WEBASE.Models;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

namespace SspUis.DataLayer.EfClasses.Claim
{
    [Table("info_court_integration", Schema = "claim")]
    [Index(nameof(ApplicationForCourtId), Name = "uix__info_court_integration")]
    public class Courtntegration : IHaveIdProp<int>
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("application_for_court_id")]
        public long ApplicationForCourtId { get; set; }

        [Column("court_id")]
        public Guid CourtId { get; set; }
        [Column("participant_type")]
        public string ParticipantType { get; set; }
        [Column("doc_number")]
        public string DocNumber { get; set; }
        [Column("category_id")]
        public Guid CategoryId { get; set; }
        [Column("currency_id")]
        public string CurrencyId { get; set; }
        [Column("entity_type")]
        public string EntityType { get; set; }
        [Column("claim_kind")]
        public string ClaimKind { get; set; }
        [Column("file_id")]
        public Guid FileId { get; set; }
        [Column("type_id")]
        public Guid TypeId { get; set; }
        [Column("amount_category_id")]
        public Guid AmountCategoryId { get; set; }
        //public Guid PostReasonId { get; set; }
        //public Guid DutyReasonId { get; set; }
        [Column("region_id")]
        public Guid RegionId { get; set; }
        [Column("district_id")]
        public Guid DistrictId { get; set; }
        [Column("country_id")]
        public Guid CountryId { get; set; }
        [Column("payment_account")]
        public long PaymentAccount { get; set; }

        [ForeignKey(nameof(ApplicationForCourtId))]
        public virtual ApplicationForCourt ApplicationForCourt { get; set; }
    }
}