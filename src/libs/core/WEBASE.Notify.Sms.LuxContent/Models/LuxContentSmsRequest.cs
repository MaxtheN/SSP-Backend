using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBASE.Notify.Sms.LuxContent
{
    public class LuxContentSmsRequest
    {
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("sign")]
        public string Sign { get; set; }
        [JsonProperty("params")]
        public LuxContentSmsParams Params { get; set; }
    }

    public class LuxContentSmsParams
    {
        [JsonProperty("msisdn")]
        public string Msisdn { get; set; }
        [JsonProperty("service")]
        public string Service { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class LuxContentSmsResponse
    {
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("data")]
        public LuxContentSmsResponseData Data { get; set; }
    }

    public class LuxContentSmsResponseData
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
