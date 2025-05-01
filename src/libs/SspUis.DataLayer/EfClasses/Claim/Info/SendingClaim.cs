using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using Ssp.DataLayer.EFClasses.Edoc;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Claim.Info
{
    [Table("info_sending_new_claims", Schema = "claim")]
    public class SendingNewClaimDto : IHaveIdProp<int>
    {
    
        public SendingNewClaimDto()
        {
            ClaimCategories = new HashSet<ClaimCategory>();
            CaseDocuments = new HashSet<CaseDocument>();
            CaseParticipantes = new HashSet<CaseParticipant>();
            ClaimAmountsWithPartes = new HashSet<ClaimAmountsWithPart>();
        }

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("request_id")]
        public Guid RequestId { get; set; }

        public virtual Case Case { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual Signature Signature { get; set; }

        [InverseProperty(nameof(ClaimCategory.Owner))]
        public virtual ICollection<ClaimCategory> ClaimCategories { get; set; }

        [InverseProperty(nameof(CaseDocument.Owner))]
        public virtual ICollection<CaseDocument> CaseDocuments { get; set; }

        [InverseProperty(nameof(CaseParticipant.Owner))]
        public virtual ICollection<CaseParticipant> CaseParticipantes { get; set; }

        [InverseProperty(nameof(ClaimAmountsWithPart.Owner))]
        public virtual ICollection<ClaimAmountsWithPart> ClaimAmountsWithPartes { get; set; }


    }

    [Table("info_cases", Schema = "claim")]

    public class Case : IHaveIdProp<int>
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("doc_date")]
        public string DocDate { get; set; }

        [Column("court_id")]
        public Guid CourtId { get; set; }

        [Column("doc_number")]
        public string DocNumber { get; set; }


        [Column("is_electron_formed")]
        public bool IsElectronFormed { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual SendingNewClaimDto SendingNewClaimDto { get; set; }


    }


    [Table("info_claim_categories", Schema = "claim")]
    public class ClaimCategory : IHaveIdProp<int>
    {

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual SendingNewClaimDto Owner { get; set; }
    }

    [Table("info_case_documents", Schema = "claim")]
    public class CaseDocument
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("file_id")]
        public Guid FileId { get; set; }

        [Column("type_id")]
        public Guid TypeId { get; set; }

        [Column("file_hash")]
        public string FileHash { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual SendingNewClaimDto Owner { get; set; }
    }


    [Table("info_founders", Schema = "claim")]
    public class Founder
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("founder_name")]
        public string FounderName { get; set; }
        [Column("entity_details_id")]
        public int EntityDetailsId { get; set; }

        [ForeignKey(nameof(EntityDetailsId))]
        public virtual EntityDetails EntityDetails { get; set; }
    }

    [Table("info_entity_details", Schema = "claim")]
    public class EntityDetails
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("entity_type")]
        public string EntityType { get; set; }

        [Column("case_participants_id")]
        public int CaseParticipantsId { get; set; }

        [Column("is_current")]
        public bool IsCurrent { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("region_id")]
        public Guid RegionId { get; set; }

        [Column("district_id")]
        public Guid DistrictId { get; set; }

        [Column("country_id")]
        public Guid CountryId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("director")]
        public string Director { get; set; }

        [Column("org_type")]
        public string OrgType { get; set; }

        [InverseProperty(nameof(Founder.EntityDetails))]
        public virtual ICollection<Founder> Founders { get; set; }

        [ForeignKey(nameof(CaseParticipantsId))]
        public virtual CaseParticipant CaseParticipant { get; set; }



        //[Column("director_pinfl")]
        //public long DirectorPinfl { get; set; }

        //[Column("accountant_name")]
        //public string AccountantName { get; set; }

        //[Column("accountant_pinfl")]
        //public string AccountantPinfl { get; set; }
    }

    [Table("info_participants", Schema = "claim")]
    public class Participant
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("is_main")]
        public bool IsMain { get; set; }

        [Column("case_participants_id")]
        public int CaseParticipantsId { get; set; }

        [ForeignKey(nameof(CaseParticipantsId))]
        public virtual CaseParticipant CaseParticipant { get; set; }
    }

    [Table("info_entities", Schema = "claim")]
    public class Entity
    {

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("tin")]
        public long Tin { get; set; }

        [Column("pinfl")]
        public long? Pinfl { get; set; }

        [Column("not_citizen")]
        public bool NotCitizen { get; set; }

        [Column("case_participants_id")]
        public int CaseParticipantsId { get; set; }

        [ForeignKey(nameof(CaseParticipantsId))]
        public virtual CaseParticipant CaseParticipant { get; set; }

    }

    [Table("info_case_participants", Schema = "claim")]
    public class CaseParticipant
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual SendingNewClaimDto Owner { get; set; }
        public virtual Entity Entity { get; set; }

        public virtual Participant Participant { get; set; }
        public virtual EntityDetails EntityDetails { get; set; }
    }

    [Table("info_claim_amount_parts_details", Schema = "claim")]
    public class ClaimAmountPartsDetails
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("payment_account")]
        public long PaymentAccount { get; set; }

        [Column("claim_amounts_parts_id")]
        public int ClaimAmountPartsId { get; set; }

        [ForeignKey(nameof(ClaimAmountPartsId))]
        public virtual ClaimAmountPart ClaimAmountPart { get; set; }    
    }

    [Table("info_claim_amount_parts_details", Schema = "claim")]
    public class ClaimAmountPart
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("amount")]
        public string Amount { get; set; }

        [Column("info_claim_amounts_with_parts_id")]
        public int InfoClaimAmountWithPartsId { get; set; }

        [Column("amount_type")]
        public string AmountType { get; set; }

        [Column("amount_category_id")]
        public Guid AmountCategoryId { get; set; }

        [ForeignKey(nameof(InfoClaimAmountWithPartsId))]
        public virtual ClaimAmountsWithPart ClaimAmountsWithPart { get; set; }
        public virtual ClaimAmountPartsDetails ClaimAmountPartsDetails { get; set; }
    }


    [Table("info_claim_amounts", Schema = "claim")]
    public class ClaimAmount
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("info_claim_amounts_with_parts_id")]
        public int InfoClaimAmountWithPartsId { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("forfeit")]
        public decimal Forfeit { get; set; }

        [Column("currency_id")]
        public string CurrencyId { get; set; }

        [ForeignKey(nameof(InfoClaimAmountWithPartsId))]
        public virtual ClaimAmountsWithPart ClaimAmountsWithPart { get; set; }
    }

    [Table("info_claim_amounts_with_parts", Schema = "claim")]
    public class ClaimAmountsWithPart
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual SendingNewClaimDto Owner { get; set; }

        public virtual ClaimAmount ClaimAmount { get; set; }

        [InverseProperty(nameof(ClaimAmountPart.ClaimAmountsWithPart))]
        public virtual ICollection<ClaimAmountPart> ClaimAmountParts { get; set; }
    }


    [Table("info_claims", Schema = "claim")]
    public class Claim
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual SendingNewClaimDto Owner { get; set; }

        [Column("claim_kind")]
        public string ClaimKind { get; set; }
    }

    [Table("info_signatures", Schema = "claim")]
    public class Signature
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual SendingNewClaimDto Owner { get; set; }

        [Column("public_certificate")]
        public string PublicCertificate { get; set; }

        [Column("sign")]
        public string Sign { get; set; }

        [Column("signed_hash")]
        public string SignedHash { get; set; }
    }
}
