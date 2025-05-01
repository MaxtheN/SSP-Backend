using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class SoliqIndividualEntrepreneur
    {
        [JsonProperty("data")]
        public SoliqIndividualEntrepreneurData Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class SoliqIndividualEntrepreneurCompany
    {
        [JsonProperty("opf")]
        public object Opf { get; set; }

        [JsonProperty("pinfl")]
        public string Pinfl { get; set; }

        [JsonProperty("tin")]
        public string Tin { get; set; }

        [JsonProperty("sooguRegistrator")]
        public string SooguRegistrator { get; set; }

        [JsonProperty("registrationDate")]
        public string RegistrationDate { get; set; }

        [JsonProperty("reestrId")]
        public int? ReestrId { get; set; }

        [JsonProperty("reRegistrationDate")]
        public object ReRegistrationDate { get; set; }

        [JsonProperty("statusId")]
        public int? StatusId { get; set; }

        [JsonProperty("statusUpdated")]
        public object StatusUpdated { get; set; }

        [JsonProperty("liquidationDate")]
        public object LiquidationDate { get; set; }

        [JsonProperty("liquidationReason")]
        public object LiquidationReason { get; set; }

        [JsonProperty("suspensionDate")]
        public object SuspensionDate { get; set; }

        [JsonProperty("suspensionReason")]
        public object SuspensionReason { get; set; }

        [JsonProperty("taxMode")]
        public int? TaxMode { get; set; }

        [JsonProperty("vatNumber")]
        public object VatNumber { get; set; }

        [JsonProperty("taxpayerType")]
        public object TaxpayerType { get; set; }

        [JsonProperty("activityCode")]
        public int? ActivityCode { get; set; }

        [JsonProperty("individualEntrepreneurType")]
        public int? IndividualEntrepreneurType { get; set; }

        [JsonProperty("licenseBeginDate")]
        public string LicenseBeginDate { get; set; }

        [JsonProperty("licenseEndDate")]
        public string LicenseEndDate { get; set; }

        [JsonProperty("applicationId")]
        public object ApplicationId { get; set; }

        [JsonProperty("lastApplicationId")]
        public object LastApplicationId { get; set; }

        [JsonProperty("certificateDocNumber")]
        public object CertificateDocNumber { get; set; }
    }

    public class CompanyBank
    {
        [JsonProperty("mfo")]
        public string Mfo { get; set; }

        [JsonProperty("paymentAccount")]
        public string PaymentAccount { get; set; }

        [JsonProperty("name")]
        public object Name { get; set; }
    }

    public class SoliqIndividualEntrepreneurCompanyBillingAddress
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
        public string CadastreNumber { get; set; }

        [JsonProperty("postcode")]
        public object Postcode { get; set; }
    }

    public class SoliqIndividualEntrepreneurCompanyShippingAddress
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

    public class SoliqIndividualEntrepreneurData
    {
        [JsonProperty("company")]
        public SoliqIndividualEntrepreneurCompany Company { get; set; }

        [JsonProperty("companyBillingAddress")]
        public CompanyBillingAddress CompanyBillingAddress { get; set; }

        [JsonProperty("companyShippingAddress")]
        public CompanyShippingAddress CompanyShippingAddress { get; set; }

        [JsonProperty("companyBanks")]
        public List<CompanyBank> CompanyBanks { get; set; }

        [JsonProperty("extraActivityTypes")]
        public object ExtraActivityTypes { get; set; }

        [JsonProperty("director")]
        public SoliqIndividualEntrepreneurDirector Director { get; set; }

        [JsonProperty("familyMembers")]
        public List<object> FamilyMembers { get; set; }
    }

    public class SoliqIndividualEntrepreneurDirector
    {
        [JsonProperty("pinfl")]
        public string Pinfl { get; set; }

        [JsonProperty("tin")]
        public string Tin { get; set; }

        [JsonProperty("phone")]
        public object Phone { get; set; }

        [JsonProperty("lastNameLatin")]
        public string LastNameLatin { get; set; }

        [JsonProperty("firstNameLatin")]
        public string FirstNameLatin { get; set; }

        [JsonProperty("middleNameLatin")]
        public string MiddleNameLatin { get; set; }
    }
}
