using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bojxona.Models
{
    public class GetGTDByInnResponseDto
    {
        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("organizationtin")]
        public string Organizationtin { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dataDeclaration")]
        public List<GetGTDByInnDataDto> Data { get; set; } = new();

        [JsonProperty("status")]
        public int Status { get; set; }
    }
    public class GetGTDByInnDataDto
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("organization2adress")]
        public string Organization2adress { get; set; }

        [JsonProperty("organization1adress")]
        public string Organization1adress { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("organizationtin")]
        public string Organizationtin { get; set; }

        [JsonProperty("organization2name")]
        public string Organization2name { get; set; }

        [JsonProperty("ekimcountry")]
        public string EkimcountryCode { get; set; }

        public string EkimcountryFullName { get; set; }

        [JsonProperty("typeincoterms")]
        public string Typeincoterms { get; set; }

        [JsonProperty("goods")]
        public List<Good> Goods { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("organization1name")]
        public string Organization1name { get; set; }

        [JsonProperty("typetransport")]
        public string Typetransport { get; set; }
    }

    public class Good
    {
        [JsonProperty("netmassgoods")]
        public decimal NetMassGoods { get; set; }

        [JsonProperty("numbergoods")]
        public string NumberGoods { get; set; }

        [JsonProperty("codetiftngoods")]
        public string CodeTiftnGoods { get; set; }

        [JsonProperty("additionalunitgoods")]
        public decimal AdditionalUnitgoods { get; set; }

        [JsonProperty("numcontract")]
        public string NumContract { get; set; }

        [JsonProperty("unitgoods")]
        public string UnitGoods { get; set; }

        [JsonProperty("valuegoods")]
        public decimal ValueGoods { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }
    }

}
