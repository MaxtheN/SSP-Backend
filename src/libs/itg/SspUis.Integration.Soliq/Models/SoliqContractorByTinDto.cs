using Newtonsoft.Json;
using SspUis.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class SoliqContractorByTinResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public SoliqContractorByTinDto Data { get; set; } = new();
    }

    public class SoliqContractorByTinDto
    {
        [JsonProperty("company")]
        public SoliqContractorByTinCompany Company { get; set; } = new();

        [JsonProperty("companyBillingAddress")]
        public SoliqContractorByTinCompanyBillingAddress CompanyBillingAddress { get; set; } = new();

        [JsonProperty("companyShippingAddress")]
        public List<SoliqContractorByTinCompanyShippingAddress> CompanyShippingAddress { get; set; } = new();

        [JsonProperty("companyExtraInfo")]
        public SoliqContractorByTinCompanyExtraInfo CompanyExtraInfo { get; set; } = new();

        [JsonProperty("director")]
        public SoliqContractorByTinDirector Director { get; set; } = new();

        [JsonProperty("directorAddress")]
        public SoliqContractorByTinDirectorAddress DirectorAddress { get; set; } = new();

        [JsonProperty("directorContact")]
        public SoliqContractorByTinDirectorContact DirectorContact { get; set; } = new();

        [JsonProperty("accountant")]
        public SoliqContractorByTinAccountant Accountant { get; set; } = new();

        [JsonProperty("accountantAddress")]
        public SoliqContractorByTinAccountantAddress AccountantAddress { get; set; } = new();

        [JsonProperty("accountantContact")]
        public SoliqContractorByTinAccountantContact AccountantContact { get; set; } = new();

        [JsonProperty("companyContact")]
        public object CompanyContact { get; set; } = new();

        [JsonProperty("companyBanks")]
        public List<SoliqContractorByTinCompanyBank> CompanyBanks { get; set; } = new();

        [JsonProperty("founders")]
        public object Founders { get; set; } = new();

        [JsonProperty("founderBeneficiaries")]
        public object FounderBeneficiaries { get; set; } = new();

        [JsonProperty("argos")]
        public SoliqContractorByTinArgos Argos { get; set; } = new();

        [JsonProperty("names")]
        public SoliqContractorByTinNames Names { get; set; } = new();
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SoliqContractorByTinAccountant
    {
        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("middleName")]
        public string MiddleName { get; set; } = string.Empty;

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; } = string.Empty;

        [JsonProperty("citizenship")]
        public string Citizenship { get; set; } = string.Empty;

        [JsonProperty("passportSeries")]
        public string PassportSeries { get; set; } = string.Empty;

        [JsonProperty("passportNumber")]
        public string PassportNumber { get; set; } = string.Empty;

        [JsonProperty("pinfl")]
        public string Pinfl { get; set; } = string.Empty;

        [JsonProperty("tin")]
        public string Tin { get; set; } = string.Empty;

        [JsonProperty("birthDate")]
        public string BirthDate { get; set; } = string.Empty;

        [JsonProperty("individualId")]
        public int? IndividualId { get; set; }

        [JsonProperty("countryCode")]
        public int? CountryCode { get; set; }
    }

    public class SoliqContractorByTinAccountantAddress
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("countryCode")]
        public int? CountryCode { get; set; }

        [JsonProperty("region")]
        public SoliqContractorByTinRegion Region { get; set; } = new();

        [JsonProperty("district")]
        public SoliqContractorByTinDistrict District { get; set; } = new();

        [JsonProperty("sectorCode")]
        public int? SectorCode { get; set; }

        [JsonProperty("village")]
        public string Village { get; set; } = string.Empty;

        [JsonProperty("streetName")]
        public string StreetName { get; set; } = string.Empty;

        [JsonProperty("house")]
        public string House { get; set; } = string.Empty;

        [JsonProperty("flat")]
        public string Flat { get; set; } = string.Empty;

        [JsonProperty("soatoCode")]
        public int SoatoCode { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinAccountantContact
    {
        [JsonProperty("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinActivityType
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;

        [JsonProperty("level_id")]
        public int? LevelId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinArgos
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("type")]
        public Type Type { get; set; } = new();

        [JsonProperty("category")]
        public SoliqContractorByTinCategory Category { get; set; } = new();

        [JsonProperty("complex")]
        public SoliqContractorByTinComplex Complex { get; set; } = new();

        [JsonProperty("ggs")]
        public SoliqContractorByTinGgs Ggs { get; set; } = new();

        [JsonProperty("subordination")]
        public Subordination Subordination { get; set; } = new();

        [JsonProperty("territorialLevel")]
        public TerritorialLevel TerritorialLevel { get; set; } = new();

        [JsonProperty("companyTin")]
        public string CompanyTin { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinBusinessStructureDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinBusinessTypeDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinCategory
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinCompany
    {
        [JsonProperty("tin")]
        public string Tin { get; set; } = string.Empty;

        [JsonProperty("created")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("updated")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Updated { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("shortName")]
        public string ShortName { get; set; } = string.Empty;

        [JsonProperty("opf")]
        public int? Opf { get; set; }

        [JsonProperty("kfs")]
        public int? Kfs { get; set; }

        [JsonProperty("oked")]
        public string Oked { get; set; } = string.Empty;

        [JsonProperty("soato")]
        public int Soato { get; set; }

        [JsonProperty("soogu")]
        public string Soogu { get; set; } = string.Empty;

        [JsonProperty("sooguRegistrator")]
        public string SooguRegistrator { get; set; } = string.Empty;

        [JsonProperty("registrationDate")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty("registrationNumber")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [JsonProperty("reregistrationDate")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime? ReregistrationDate { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("statusUpdated")]
        public string StatusUpdated { get; set; } = string.Empty;

        [JsonProperty("liquidationDate")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime? LiquidationDate { get; set; }

        [JsonProperty("liquidationReason")]
        public string LiquidationReason { get; set; } = string.Empty;

        [JsonProperty("taxMode")]
        public int? TaxMode { get; set; }

        [JsonProperty("vatNumber")]
        public long? VatNumber { get; set; }

        [JsonProperty("taxpayerType")]
        public int? TaxpayerType { get; set; }

        [JsonProperty("businessType")]
        public int? BusinessType { get; set; }

        [JsonProperty("businessFund")]
        public long? BusinessFund { get; set; }

        [JsonProperty("businessStructure")]
        public int? BusinessStructure { get; set; }

        [JsonProperty("opfDetail")]
        public SoliqContractorByTinOpfDetail OpfDetail { get; set; } = new();

        [JsonProperty("sooguDetail")]
        public SooguDetail SooguDetail { get; set; } = new();

        [JsonProperty("statusDetail")]
        public StatusDetail StatusDetail { get; set; } = new();

        [JsonProperty("region")]
        public object Region { get; set; } = null!;

        [JsonProperty("district")]
        public object District { get; set; } = null!;

        [JsonProperty("businessStructureDetail")]
        public SoliqContractorByTinBusinessStructureDetail BusinessStructureDetail { get; set; } = new();

        [JsonProperty("okedDetail")]
        public SoliqContractorByTinOkedDetail OkedDetail { get; set; } = new();

        [JsonProperty("villageCode")]
        public int? VillageCode { get; set; }

        [JsonProperty("villageName")]
        public string VillageName { get; set; } = string.Empty;

        [JsonProperty("streetName")]
        public string StreetName { get; set; } = string.Empty;

        [JsonProperty("flat")]
        public string Flat { get; set; } = string.Empty;

        [JsonProperty("house")]
        public string House { get; set; } = string.Empty;

        [JsonProperty("addressId")]
        public string AddressId { get; set; } = string.Empty;

        [JsonProperty("statusType")]
        public string StatusType { get; set; } = string.Empty;

        [JsonProperty("flagReason")]
        public string FlagReason { get; set; } = string.Empty;

        [JsonProperty("vatReason")]
        public string VatReason { get; set; } = string.Empty;

        [JsonProperty("vatBeginDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? VatBeginDate { get; set; }

        [JsonProperty("vatFromDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? VatFromDate { get; set; }

        [JsonProperty("vatStatus")]
        public string VatStatus { get; set; } = string.Empty;

        [JsonProperty("businessTypeDetail")]
        public SoliqContractorByTinBusinessTypeDetail BusinessTypeDetail { get; set; } = new();

        [JsonProperty("taxRate")]
        public decimal? TaxRate { get; set; }

        [JsonProperty("argosDetails")]
        public string ArgosDetails { get; set; } = string.Empty;

        [JsonProperty("activityTypes")]
        public List<SoliqContractorByTinActivityType> ActivityTypes { get; set; } = new();

        [JsonProperty("cottonCluster")]
        public bool CottonCluster { get; set; }
    }

    public class SoliqContractorByTinCompanyBank
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("mfo")]
        public string Mfo { get; set; } = string.Empty;

        [JsonProperty("paymentAccount")]
        public string PaymentAccount { get; set; } = string.Empty;

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("statusName")]
        public string StatusName { get; set; } = string.Empty;

        [JsonProperty("openDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? OpenDate { get; set; }
        [JsonProperty("closeDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CloseDate { get; set; }

        [JsonProperty("attribute")]
        public int? Attribute { get; set; }

        [JsonProperty("reasonCode")]
        public int? ReasonCode { get; set; }

        [JsonProperty("companyTin")]
        public string CompanyTin { get; set; } = string.Empty;

        [JsonProperty("bankName")]
        public string BankName { get; set; } = string.Empty;

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; } = string.Empty;

        [JsonProperty("balance")]
        public decimal? Balance { get; set; }
    }

    public class SoliqContractorByTinCompanyBillingAddress
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("countryCode")]
        public int? CountryCode { get; set; }

        [JsonProperty("region")]
        public SoliqContractorByTinRegion Region { get; set; } = new();

        [JsonProperty("district")]
        public SoliqContractorByTinDistrict District { get; set; } = new();

        [JsonProperty("sectorCode")]
        public int? SectorCode { get; set; }

        [JsonProperty("village")]
        public string Village { get; set; } = string.Empty;

        [JsonProperty("streetName")]
        public string StreetName { get; set; } = string.Empty;

        [JsonProperty("house")]
        public string House { get; set; } = string.Empty;

        [JsonProperty("flat")]
        public string Flat { get; set; } = string.Empty;

        [JsonProperty("soatoCode")]
        public int SoatoCode { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinCompanyExtraInfo
    {
        [JsonProperty("companyExtraInfoId")]
        public int? CompanyExtraInfoId { get; set; }

        [JsonProperty("avgNumberEmployees")]
        public int? AvgNumberEmployees { get; set; }

        [JsonProperty("monthlyNumberEmployees")]
        public int? MonthlyNumberEmployees { get; set; }
    }

    public class SoliqContractorByTinCompanyShippingAddress
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("countryCode")]
        public int? CountryCode { get; set; }

        [JsonProperty("region")]
        public SoliqContractorByTinRegion Region { get; set; } = new();

        [JsonProperty("district")]
        public SoliqContractorByTinDistrict District { get; set; } = new();

        [JsonProperty("sectorCode")]
        public int? SectorCode { get; set; }

        [JsonProperty("village")]
        public string Village { get; set; } = string.Empty;

        [JsonProperty("streetName")]
        public string StreetName { get; set; } = string.Empty;

        [JsonProperty("house")]
        public string House { get; set; } = string.Empty;

        [JsonProperty("flat")]
        public string Flat { get; set; } = string.Empty;

        [JsonProperty("soatoCode")]
        public int? SoatoCode { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinComplex
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; } = string.Empty;

        [JsonProperty("abbreviation_ru")]
        public string AbbreviationRu { get; set; } = string.Empty;

        [JsonProperty("abbreviation_uz_cyrl")]
        public string AbbreviationUzCyrl { get; set; } = string.Empty;

        [JsonProperty("abbreviation_uz_latn")]
        public string AbbreviationUzLatn { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinDirector
    {
        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("middleName")]
        public string MiddleName { get; set; } = string.Empty;

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; } = string.Empty;

        [JsonProperty("citizenship")]
        public string Citizenship { get; set; } = string.Empty;

        [JsonProperty("passportSeries")]
        public string PassportSeries { get; set; } = string.Empty;

        [JsonProperty("passportNumber")]
        public string PassportNumber { get; set; } = string.Empty;

        [JsonProperty("pinfl")]
        public string Pinfl { get; set; } = string.Empty;

        [JsonProperty("tin")]
        public string Tin { get; set; } = string.Empty;

        [JsonProperty("birthDate")]
        public string BirthDate { get; set; } = string.Empty;

        [JsonProperty("individualId")]
        public int? IndividualId { get; set; }

        [JsonProperty("countryCode")]
        public int? CountryCode { get; set; }
    }

    public class SoliqContractorByTinDirectorAddress
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("countryCode")]
        public int? CountryCode { get; set; }

        [JsonProperty("region")]
        public SoliqContractorByTinRegion Region { get; set; } = new();

        [JsonProperty("district")]
        public SoliqContractorByTinDistrict District { get; set; } = new();

        [JsonProperty("sectorCode")]
        public int? SectorCode { get; set; }

        [JsonProperty("village")]
        public string Village { get; set; } = string.Empty;

        [JsonProperty("streetName")]
        public string StreetName { get; set; } = string.Empty;

        [JsonProperty("house")]
        public string House { get; set; } = string.Empty;

        [JsonProperty("flat")]
        public string Flat { get; set; } = string.Empty;

        [JsonProperty("soatoCode")]
        public int SoatoCode { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinDirectorContact
    {
        [JsonProperty("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinDistrict
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;

        [JsonProperty("administrativeCenterCode")]
        public string AdministrativeCenterCode { get; set; } = string.Empty;

        [JsonProperty("regionCode")]
        public int RegionCode { get; set; }

        [JsonProperty("region_id")]
        public int RegionsId { get; set; }

        [JsonProperty("regionId")]
        public int RegionId { get; set; }

        [JsonProperty("districtId")]
        public int DistrictId { get; set; }

        [JsonProperty("districts_id")]
        public int DistrictsId { get; set; }

        [JsonProperty("active")]
        public string Active { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinGgs
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinNames
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("statName")]
        public string StatName { get; set; } = string.Empty;

        [JsonProperty("argosName")]
        public string ArgosName { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinOkedDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;

        [JsonProperty("section")]
        public string Section { get; set; } = string.Empty;

        [JsonProperty("name_short_ru")]
        public string NameShortRu { get; set; } = string.Empty;

        [JsonProperty("name_short_uz_cyrl")]
        public string NameShortUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_short_uz_latn")]
        public string NameShortUzLatn { get; set; } = string.Empty;

        [JsonProperty("pkm275")]
        public string Pkm275 { get; set; } = string.Empty;

        [JsonProperty("employee_limit_mf")]
        public int? EmployeeLimitMf { get; set; }

        [JsonProperty("employee_limit_lf")]
        public int? EmployeeLimitLf { get; set; }
    }

    public class SoliqContractorByTinOpfDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;
    }

    public class SoliqContractorByTinRegion
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;

        [JsonProperty("administrativeCenterCode")]
        public string AdministrativeCenterCode { get; set; } = string.Empty;
    }


    public class SooguDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;

        [JsonProperty("okpo")]
        public string Okpo { get; set; } = string.Empty;
    }

    public class StatusDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;

        [JsonProperty("group")]
        public string Group { get; set; } = string.Empty;
    }

    public class Subordination
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;
    }

    public class TerritorialLevel
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;
    }

    public class Type
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("name_ru")]
        public string NameRu { get; set; } = string.Empty;

        [JsonProperty("name_uz_cyrl")]
        public string NameUzCyrl { get; set; } = string.Empty;

        [JsonProperty("name_uz_latn")]
        public string NameUzLatn { get; set; } = string.Empty;
    }


}
