using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Globalization;

namespace SspUis.Integration.DavAktiv
{
    public class DavAktivApplicationDto
    {
        [JsonProperty("id")]
        public Guid Id2 { get; set; }
        [JsonProperty("send_date")]
        [JsonConverter(typeof(DateFormatConverter), new object[] { "dd.MM.yyyy HH:mm:ss" })]
        public DateTime SendAt { get; set; } = DateTime.Now;
        [JsonProperty("doc_on")]
        [JsonConverter(typeof(DateOnlyConverter), new object[] { "dd.MM.yyyy" })]
        public DateOnly DocOn { get; set; }
        [JsonProperty("doc_number")]
        public string DocNumber { get; set; } = string.Empty;
        [JsonProperty("inn")]
        public int Inn { get; set; }
        [JsonProperty("contractor")]
        public string Contractor { get; set; } = string.Empty;
        [JsonProperty("region_soato")]
        public int RegionSoato { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; } = string.Empty;
        [JsonProperty("district_soato")]
        public int DistrictSoato { get; set; }
        [JsonProperty("district")]
        public string District { get; set; } = string.Empty;
        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;
        [JsonProperty("certificate_link")]
        public string PrtnCertificateLink { get; set; } = string.Empty;
        [JsonProperty("auction_doc_on")]
        [JsonConverter(typeof(DateOnlyConverter), new object[] { "dd.MM.yyyy" })]
        public DateOnly? AuctionDocOn { get; set; }
        [JsonProperty("auction_doc_number")]
        public string AuctionDocNumber { get; set; } = string.Empty;
        [JsonProperty("state_asset_name")]
        public string StateAssetName { get; set; } = string.Empty;
        [JsonProperty("contract_type_id")]
        public int PrtnContractTypeId { get; set; }
        [JsonProperty("contract_type")]
        public string PrtnContractType { get; set; } = string.Empty;
        [JsonProperty("status")]
        public int StateAssetStatus { get; set; }
        [JsonProperty("title")]
        public string Title => "Davlat aktivlari uchun ariza";

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class DavAktivApplicationResponseDto
    {
        [JsonProperty("success")]
        public bool Succes { get; set; }
        [JsonProperty("error_msg")]
        public string ErrorMsg { get; set; }
        [JsonProperty("data")]
        public DavAktivApplicationResponseDataDto Data { get; set; }
    }
    public class DavAktivApplicationResponseDataDto
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }


    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string _format;

        public DateOnlyConverter(string format)
        {
            _format = format;
        }

        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DateOnly.ParseExact((string)reader.Value!, _format, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(_format, CultureInfo.InvariantCulture));
        }
    }
}
