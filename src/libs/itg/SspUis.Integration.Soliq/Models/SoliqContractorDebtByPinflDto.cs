using Newtonsoft.Json;

namespace SspUis.Integration.Soliq.Models;
public class SoliqContractorByPinflResponseDto
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("data")]
    public SoliqContractorByPinflResponseChildDto Data { get; set; } = new();
}
public class SoliqContractorByPinflResponseChildDto
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("error")]
    public string Error { get; set; } = string.Empty;

    [JsonProperty("object")]
    public object Object { get; set; }

    [JsonProperty("data")]
    public SoliqContractorDebtByPinflDto Data { get; set; } = new();
}
public class SoliqContractorDebtByPinflDto
{
    [JsonProperty("sendDate")]
    public string SendDate { get; set; } = string.Empty;

    [JsonProperty("fullName")]
    public FullName FullName { get; set; } = new();

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("regionId")]
    public int? RegionId { get; set; }

    [JsonProperty("regionName")]
    public string RegionName { get; set; } = string.Empty;

    [JsonProperty("districtId")]
    public int? DistrictId { get; set; }

    [JsonProperty("districtName")]
    public string DistrictName { get; set; } = string.Empty;

    [JsonProperty("soatoCode")]
    public int? SoatoCode { get; set; }

    [JsonProperty("villageCode")]
    public string VillageCode { get; set; } = string.Empty;

    [JsonProperty("villageName")]
    public string VillageName { get; set; } = string.Empty;

    [JsonProperty("sectorCode")]
    public string SectorCode { get; set; } = string.Empty;

    [JsonProperty("sectorName")]
    public SectorName SectorName { get; set; } = new();
    
    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;

    [JsonProperty("tin")]
    public int? Tin { get; set; }
    
    [JsonProperty("pinfl")]
    public long? Pinfl { get; set; }

    [JsonProperty("contact")]
    public object? Contact { get; set; }

    [JsonProperty("objectCode")]
    public object? ObjectCode { get; set; }

    [JsonProperty("middleName")]
    public MiddleName MiddleName { get; set; } = new();

    [JsonProperty("firstName")]
    public FirstName FirstName { get; set; } = new();

    [JsonProperty("lastName")]
    public LastName LastName { get; set; } = new();

    [JsonProperty("isResident")]
    public int? IsResident { get; set; }

    [JsonProperty("passportSerie")]
    public string? PassportSerie { get; set; } = string.Empty;

    [JsonProperty("passportNumber")]
    public string? PassportNumber { get; set; } = string.Empty;

    [JsonProperty("passportGivenBy")]
    public string? PassportGivenBy { get; set; } = string.Empty;

    [JsonProperty("givenDate")]
    public string? GivenDate { get; set; } = string.Empty;

    [JsonProperty("citizenshipCode")]
    public string? CitizenshipCode { get; set; } = string.Empty;

    [JsonProperty("citizenshipName")]
    public CitizenshipName CitizenshipName { get; set; } = new();

    [JsonProperty("isEntrepreneur")]
    public int? IsEntrepreneur { get; set; }

    [JsonProperty("isPrivateNotary")]
    public int? IsPrivateNotary { get; set; }

    [JsonProperty("isSelfEmployed")]
    public int? IsSelfEmployed { get; set; }

    [JsonProperty("isActive")]
    public int? IsActive { get; set; }

    [JsonProperty("birthDate")]
    public string? BirthDate { get; set; } = string.Empty;

    [JsonProperty("genderCode")]
    public int? GenderCode { get; set; }

    [JsonProperty("gender")]
    public string? Gender { get; set; } = string.Empty;

    [JsonProperty("entrepreneur")]
    public Entrepreneur? Entrepreneur { get; set; } = new();
}
public class Entrepreneur
{
    [JsonProperty("id")]
    public int? Id { get; set; }

    [JsonProperty("formId")]
    public int? FormId { get; set; }

    [JsonProperty("tin")]
    public int? Tin { get; set; }

    [JsonProperty("pinfl")]
    public long? Pinfl { get; set; }

    [JsonProperty("phoneNumber")]
    public long? PhoneNumber { get; set; }

    [JsonProperty("formName")]
    public FormName FormName { get; set; } = new();

    [JsonProperty("registrationId")]
    public string RegistrationId { get; set; } = string.Empty;

    [JsonProperty("registrationDate")]
    public string RegistrationDate { get; set; } = string.Empty;

    [JsonProperty("beginDate")]
    public string BeginDate { get; set; } = string.Empty;

    [JsonProperty("endDate")]
    public string EndDate { get; set; } = string.Empty;

    [JsonProperty("liquidationDate")]
    public object LiquidationDate { get; set; } = new();

    [JsonProperty("suspensionDate")]
    public object SuspensionDate { get; set; } = new();

    [JsonProperty("activityTypeId")]
    public int? ActivityTypeId { get; set; }

    [JsonProperty("activityTypeName")]
    public ActivityTypeName ActivityTypeName { get; set; } = new();

    [JsonProperty("entrepreneurExtraActivityTypes")]
    public object EntrepreneurExtraActivityTypes { get; set; } = new();

    [JsonProperty("entrepreneurshipAddress")]
    public EntrepreneurshipAddress EntrepreneurshipAddress { get; set; } = new();

    [JsonProperty("entrepreneurshipDirector")]
    public EntrepreneurshipDirector EntrepreneurshipDirector { get; set; } = new();

    [JsonProperty("entrepreneurshipContact")]
    public EntrepreneurshipContact EntrepreneurshipContact { get; set; } = new();

    [JsonProperty("status")]
    public Status Status { get; set; } = new();

    [JsonProperty("certificateDocNumber")]
    public string CertificateDocNumber { get; set; } = string.Empty;

    [JsonProperty("isVatTaxpayer")]
    public int? IsVatTaxpayer { get; set; }

    [JsonProperty("vatNumber")]
    public string VatNumber { get; set; } = string.Empty;

    [JsonProperty("vatStatusId")]
    public int? VatStatusId { get; set; }
    [JsonProperty("vatStatusName")]
    public string vatStatusName { get; set; } = string.Empty;

    [JsonProperty("vatRegDate")]
    public string VatRegDate { get; set; } = string.Empty;

    [JsonProperty("taxMode")]
    public int? TaxMode { get; set; }

    [JsonProperty("taxModeName")]
    public TaxModeName TaxModeName { get; set; } = new();

    [JsonProperty("taxpayerType")]
    public int? TaxpayerType { get; set; }

    [JsonProperty("taxpayerTypeName")]
    public TaxpayerTypeName TaxpayerTypeName { get; set; } = new();
}
public class EntrepreneurshipAddress
{
    [JsonProperty("soatoCode")]
    public int SoatoCode { get; set; }

    [JsonProperty("regionId")]
    public int? RegionId { get; set; }

    [JsonProperty("districtId")]
    public int? DistrictId { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;
}
public class EntrepreneurshipDirector
{
    [JsonProperty("middleName")]
    public string MiddleName { get; set; } = string.Empty;

    [JsonProperty("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = string.Empty;
}
public class Status
{
    [JsonProperty("code")]
    public int? Code { get; set; }

    [JsonProperty("name")]
    public StatusName Name { get; set; } = new();
}
public class EntrepreneurshipContact
{
    [JsonProperty("mobilePhoneNumber")]
    public string? MobilePhoneNumber { get; set; } = string.Empty;

    [JsonProperty("cellPhoneNumber")]
    public string? CellPhoneNumber { get; set; } = string.Empty;
}
public class FullName : MultiLanguageDatas { }
public class MiddleName : MultiLanguageDatas { }
public class FirstName : MultiLanguageDatas { }
public class SectorName : MultiLanguageDatas { }
public class LastName : MultiLanguageDatas { }
public class CitizenshipName : MultiLanguageDatas { }
public class FormName : MultiLanguageDatas { }
public class ActivityTypeName : MultiLanguageDatas { }
public class StatusName : MultiLanguageDatas { }
public class TaxModeName : MultiLanguageDatas { }
public class TaxpayerTypeName : MultiLanguageDatas { }
public abstract class MultiLanguageDatas
{
    [JsonProperty("ru")]
    public string? Rus { get; set; } = string.Empty;

    [JsonProperty("uz")]
    public string? Uzb { get; set; } = string.Empty;

    [JsonProperty("en")]
    public string? Eng { get; set; } = string.Empty;
}