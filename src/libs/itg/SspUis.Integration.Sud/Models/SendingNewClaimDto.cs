using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models;
public class SendingNewClaimDto
{
    [JsonProperty("case")]
    public CaseDto Case { get; set; }

    [JsonProperty("claim_categories")]
    public List<ClaimCategory> ClaimCategories { get; set; }

    [JsonProperty("case_documents")]
    public List<CaseDocument> CaseDocuments { get; set; }

    [JsonProperty("case_participants")]
    public List<CaseParticipant> CaseParticipants { get; set; }

    [JsonProperty("claim_amounts_with_parts")]
    public List<ClaimAmountsWithPart> ClaimAmountsWithParts { get; set; }

    [JsonProperty("claim")]
    public ClaimDto Claim { get; set; }

    [JsonProperty("request_id")]
    public Guid RequestId { get; set; }

    [JsonProperty("signature")]
    public Signature Signature { get; set; }
}
public class CaseDto
{
    [JsonProperty("doc_date")]
    public string DocDate { get; set; }

    [JsonProperty("court_id")]
    public Guid CourtId { get; set; }

    [JsonProperty("doc_number")]
    public string DocNumber { get; set; }

    //[JsonProperty("post_reason_id")]
    //public Guid PostReasonId { get; set; }

    //[JsonProperty("duty_reason_id")]
    //public Guid DutyReasonId { get; set; }

    [JsonProperty("is_electron_formed")]
    public bool IsElectronFormed { get; set; }
}

public class ClaimCategory
{
    [JsonProperty("category_id")]
    public Guid CategoryId { get; set; }
}

public class CaseDocument
{
    [JsonProperty("file_id")]
    public Guid FileId { get; set; }

    [JsonProperty("type_id")]
    public Guid TypeId { get; set; }

    [JsonProperty("file_hash")]
    public string FileHash { get; set; }
}

public class Entity
{
    [JsonProperty("tin")]
    public long Tin { get; set; }

    [JsonProperty("pinfl")]
    public long? Pinfl { get; set; }

    [JsonProperty("not_citizen")]
    public bool NotCitizen { get; set; }
}

public class Participant
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("is_main")]
    public bool IsMain { get; set; }
}

public class Founder
{
    [JsonProperty("founder_name")]
    public string FounderName { get; set; }
}

public class EntityDetails
{
    [JsonProperty("entity_type")]
    public string EntityType { get; set; }

    [JsonProperty("is_current")]
    public bool IsCurrent { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("region_id")]
    public Guid RegionId { get; set; }

    [JsonProperty("district_id")]
    public Guid DistrictId { get; set; }

    [JsonProperty("country_id")]
    public Guid CountryId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("director")]
    public string Director { get; set; }

    [JsonProperty("org_type")]
    public string OrgType { get; set; }

    [JsonProperty("founders")]
    public List<Founder> Founders { get; set; }

    //[JsonProperty("director_pinfl")]
    //public long DirectorPinfl { get; set; }

    //[JsonProperty("accountant_name")]
    //public string AccountantName { get; set; }

    //[JsonProperty("accountant_pinfl")]
    //public string AccountantPinfl { get; set; }
}

public class CaseParticipant
{
    [JsonProperty("entity")]
    public Entity Entity { get; set; }

    [JsonProperty("participant")]
    public Participant Participant { get; set; }

    [JsonProperty("entity_details")]
    public EntityDetails EntityDetails { get; set; }
}

public class ClaimAmountPartsDetails
{
    [JsonProperty("payment_account")]
    public long PaymentAccount { get; set; }
}

public class ClaimAmountPart
{
    [JsonProperty("amount")]
    public string Amount { get; set; }

    [JsonProperty("amount_type")]
    public string AmountType { get; set; }

    [JsonProperty("amount_category_id")]
    public Guid AmountCategoryId { get; set; }

    [JsonProperty("details")]
    public ClaimAmountPartsDetails Details { get; set; }
}

public class ClaimAmount
{
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("forfeit")]
    public decimal Forfeit { get; set; }

    [JsonProperty("currency_id")]
    public string CurrencyId { get; set; }
}

public class ClaimAmountsWithPart
{
    [JsonProperty("claim_amount")]
    public ClaimAmount ClaimAmount { get; set; }

    [JsonProperty("claim_amount_parts")]
    public List<ClaimAmountPart> ClaimAmountParts { get; set; }
}

public class ClaimDto
{
    [JsonProperty("claim_kind")]
    public string ClaimKind { get; set; }
}

public class Signature
{
    [JsonProperty("public_certificate")]
    public string PublicCertificate { get; set; }

    [JsonProperty("sign")]
    public string Sign { get; set; }

    [JsonProperty("signed_hash")]
    public string SignedHash { get; set; }
}