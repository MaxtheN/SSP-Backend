using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class SoliqContractorResponse
    {
        [JsonProperty("company")]
        public SoliqCompany Company { get; set; }

        [JsonProperty("companyBillingAddress")]
        public CompanyBillingAddress CompanyBillingAddress { get; set; }

        [JsonProperty("companyShippingAddresses")]
        public List<CompanyShippingAddress> CompanyShippingAddresses { get; set; }

        [JsonProperty("companyExtraInfo")]
        public object CompanyExtraInfo { get; set; }

        [JsonProperty("director")]
        public SoliqCompanyDirector Director { get; set; }

        [JsonProperty("directorAddress")]
        public SoliqCompanyDirectorAddress DirectorAddress { get; set; }

        [JsonProperty("directorContact")]
        public object DirectorContact { get; set; }

        [JsonProperty("accountant")]
        public object Accountant { get; set; }

        [JsonProperty("accountantAddress")]
        public object AccountantAddress { get; set; }

        [JsonProperty("accountantContact")]
        public object AccountantContact { get; set; }

        [JsonProperty("argos")]
        public object Argos { get; set; }

        [JsonProperty("companyBanks")]
        public List<object> CompanyBanks { get; set; }

        [JsonProperty("founders")]
        public List<SoliqCompanyFounder> Founders { get; set; }

        [JsonProperty("companyLink")]
        public object CompanyLink { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SoliqCompany
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("opf")]
        public int? Opf { get; set; }

        [JsonProperty("kfs")]
        public int? Kfs { get; set; }

        [JsonProperty("tin")]
        public string Tin { get; set; }
        [JsonProperty("pinfl")]
        public string Pinfl { get; set; }

        [JsonProperty("oked")]
        public string Oked { get; set; }

        [JsonProperty("soogu")]
        public string Soogu { get; set; }

        [JsonProperty("sooguRegistrator")]
        public string SooguRegistrator { get; set; }

        [JsonProperty("registrationDate")]
        public string RegistrationDate { get; set; }

        [JsonProperty("registrationNumber")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("reregistrationDate")]
        public string ReregistrationDate { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("statusUpdated")]
        public object StatusUpdated { get; set; }

        [JsonProperty("taxStatus")]
        public object TaxStatus { get; set; }

        [JsonProperty("liquidationDate")]
        public object LiquidationDate { get; set; }

        [JsonProperty("liquidationReason")]
        public object LiquidationReason { get; set; }

        [JsonProperty("suspensionDate")]
        public object SuspensionDate { get; set; }

        [JsonProperty("suspensionReason")]
        public object SuspensionReason { get; set; }

        [JsonProperty("taxMode")]
        public object TaxMode { get; set; }

        [JsonProperty("vatNumber")]
        public long? VatNumber { get; set; }

        [JsonProperty("vatRegistrationDate")]
        public object VatRegistrationDate { get; set; }

        [JsonProperty("taxpayerType")]
        public int? TaxpayerType { get; set; }

        [JsonProperty("businessType")]
        public int? BusinessType { get; set; }

        [JsonProperty("businessFund")]
        public long? BusinessFund { get; set; }

        [JsonProperty("businessStructure")]
        public object BusinessStructure { get; set; }

        [JsonProperty("businessFundCurrency")]
        public string BusinessFundCurrency { get; set; }

        [JsonProperty("createdSysDate")]
        public object CreatedSysDate { get; set; }

        [JsonProperty("updatedSysDate")]
        public object UpdatedSysDate { get; set; }
    }

    public class CompanyBillingAddress
    {
        [JsonProperty("countryCode")]
        public int? CountryCode { get; set; }

        [JsonProperty("sectorCode")]
        public int? SectorCode { get; set; }

        [JsonProperty("villageCode")]
        public string VillageCode { get; set; }

        [JsonProperty("streetName")]
        public string StreetName { get; set; }

        [JsonProperty("house")]
        public object House { get; set; }

        [JsonProperty("flat")]
        public object Flat { get; set; }

        [JsonProperty("soato")]
        public int? Soato { get; set; }

        [JsonProperty("cadastreNumber")]
        public object CadastreNumber { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }

    public class CompanyShippingAddress
    {
        [JsonProperty("countryCode")]
        public int? CountryCode { get; set; }

        [JsonProperty("sectorCode")]
        public object SectorCode { get; set; }

        [JsonProperty("villageCode")]
        public string VillageCode { get; set; }

        [JsonProperty("streetName")]
        public string StreetName { get; set; }

        [JsonProperty("house")]
        public object House { get; set; }

        [JsonProperty("flat")]
        public object Flat { get; set; }

        [JsonProperty("soato")]
        public int? Soato { get; set; }

        [JsonProperty("cadastreNumber")]
        public object CadastreNumber { get; set; }

        [JsonProperty("postcode")]
        public object Postcode { get; set; }
    }

    public class SoliqCompanyDirector
    {
        [JsonProperty("lastName")]
        public object LastName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public object MiddleName { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("nationality")]
        public object Nationality { get; set; }

        [JsonProperty("citizenship")]
        public object Citizenship { get; set; }

        [JsonProperty("passportSeries")]
        public object PassportSeries { get; set; }

        [JsonProperty("passportNumber")]
        public object PassportNumber { get; set; }

        [JsonProperty("pinfl")]
        public object Pinfl { get; set; }

        [JsonProperty("tin")]
        public object Tin { get; set; }

        [JsonProperty("birthDate")]
        public object BirthDate { get; set; }

        [JsonProperty("individualId")]
        public object IndividualId { get; set; }

        [JsonProperty("countryCode")]
        public object CountryCode { get; set; }
    }

    public class SoliqCompanyDirectorAddress
    {
        [JsonProperty("countryCode")]
        public object CountryCode { get; set; }

        [JsonProperty("sectorCode")]
        public object SectorCode { get; set; }

        [JsonProperty("villageCode")]
        public object VillageCode { get; set; }

        [JsonProperty("streetName")]
        public string StreetName { get; set; }

        [JsonProperty("house")]
        public object House { get; set; }

        [JsonProperty("flat")]
        public object Flat { get; set; }

        [JsonProperty("soato")]
        public object Soato { get; set; }

        [JsonProperty("cadastreNumber")]
        public object CadastreNumber { get; set; }

        [JsonProperty("postcode")]
        public object Postcode { get; set; }
    }

    public class SoliqCompanyFounder
    {
        [JsonProperty("founderIndividual")]
        public FounderIndividual FounderIndividual { get; set; }

        [JsonProperty("founderLegal")]
        public FounderLegal FounderLegal { get; set; }

        [JsonProperty("founderContact")]
        public object FounderContact { get; set; }

        [JsonProperty("founderAddress")]
        public FounderAddress FounderAddress { get; set; }
    }

    public class FounderAddress
    {
        [JsonProperty("countryCode")]
        public object CountryCode { get; set; }

        [JsonProperty("sectorCode")]
        public int? SectorCode { get; set; }

        [JsonProperty("villageCode")]
        public string VillageCode { get; set; }

        [JsonProperty("streetName")]
        public object StreetName { get; set; }

        [JsonProperty("house")]
        public object House { get; set; }

        [JsonProperty("flat")]
        public object Flat { get; set; }

        [JsonProperty("soato")]
        public object Soato { get; set; }

        [JsonProperty("cadastreNumber")]
        public object CadastreNumber { get; set; }

        [JsonProperty("postcode")]
        public object Postcode { get; set; }
    }

    public class FounderIndividual
    {
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("citizenship")]
        public string Citizenship { get; set; }

        [JsonProperty("passportSeries")]
        public string PassportSeries { get; set; }

        [JsonProperty("passportNumber")]
        public string PassportNumber { get; set; }

        [JsonProperty("pinfl")]
        public string Pinfl { get; set; }

        [JsonProperty("tin")]
        public string Tin { get; set; }

        [JsonProperty("birthDate")]
        public string BirthDate { get; set; }

        [JsonProperty("individualId")]
        public int? IndividualId { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("founderSharePercent")]
        public object FounderSharePercent { get; set; }

        [JsonProperty("founderShareSum")]
        public object FounderShareSum { get; set; }
    }

    public class FounderLegal
    {
        [JsonProperty("id")]
        public object Id { get; set; }

        [JsonProperty("tin")]
        public string Tin { get; set; }

        [JsonProperty("shortName")]
        public object ShortName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("registrationDate")]
        public object RegistrationDate { get; set; }

        [JsonProperty("registrationNumber")]
        public object RegistrationNumber { get; set; }

        [JsonProperty("reregistrationDate")]
        public object ReregistrationDate { get; set; }

        [JsonProperty("businessFund")]
        public object BusinessFund { get; set; }

        [JsonProperty("businessType")]
        public object BusinessType { get; set; }

        [JsonProperty("kfs")]
        public object Kfs { get; set; }

        [JsonProperty("oked")]
        public object Oked { get; set; }

        [JsonProperty("opf")]
        public object Opf { get; set; }

        [JsonProperty("soato")]
        public object Soato { get; set; }

        [JsonProperty("soogu")]
        public object Soogu { get; set; }

        [JsonProperty("sooguRegistrator")]
        public object SooguRegistrator { get; set; }

        [JsonProperty("status")]
        public object Status { get; set; }

        [JsonProperty("statusUpdated")]
        public object StatusUpdated { get; set; }

        [JsonProperty("taxMode")]
        public object TaxMode { get; set; }

        [JsonProperty("taxpayerType")]
        public object TaxpayerType { get; set; }

        [JsonProperty("vatNumber")]
        public object VatNumber { get; set; }

        [JsonProperty("liquidationDate")]
        public object LiquidationDate { get; set; }

        [JsonProperty("liquidationReason")]
        public object LiquidationReason { get; set; }

        [JsonProperty("businessStructure")]
        public object BusinessStructure { get; set; }

        [JsonProperty("created")]
        public object Created { get; set; }

        [JsonProperty("updated")]
        public object Updated { get; set; }

        [JsonProperty("regCountry")]
        public int? RegCountry { get; set; }

        [JsonProperty("founderSharePercent")]
        public double? FounderSharePercent { get; set; }

        [JsonProperty("founderShareSum")]
        public object FounderShareSum { get; set; }

        [JsonProperty("stateTin")]
        public object StateTin { get; set; }

        [JsonProperty("countTotalFounders")]
        public object CountTotalFounders { get; set; }

        [JsonProperty("director")]
        public object Director { get; set; }

        [JsonProperty("accountant")]
        public object Accountant { get; set; }

        [JsonProperty("businessStructureDetail")]
        public object BusinessStructureDetail { get; set; }

        [JsonProperty("statusDetail")]
        public object StatusDetail { get; set; }
    }
}
