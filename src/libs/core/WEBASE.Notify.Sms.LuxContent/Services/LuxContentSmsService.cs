using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;
using StatusGeneric;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text.Unicode;

namespace WEBASE.Notify.Sms.LuxContent
{
    public class LuxContentSmsService : StatusGenericHandler, ILuxContentSmsService
    {
        private readonly HttpClient _httpClient;
        private readonly LuxContentSmsProviderConfig _config;

        public LuxContentSmsService(LuxContentSmsProviderConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
        }


        public async Task Send(string phoneNumber, string message)
        {
            var request = new LuxContentSmsRequest()
            {
                User = _config.User,
                Method = _config.Method,
                Sign = Sha256($"{_config.User}{_config.Method}{phoneNumber}{_config.Key}"),
                Params = new LuxContentSmsParams
                {
                    Service = _config.Service,
                    Message = string.Format(_config.MessageTemplate, message),
                    Msisdn = phoneNumber
                }
            };

            var response = await _httpClient.PostAsJsonAsync(new Uri(_config.ProviderUrl), request);
            if (!response.IsSuccessStatusCode)
            {
                string error = "Маълумот топилмади! Информация не найдена";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }
                AddError($"{response.ReasonPhrase} error: {error}");
                return;
            }
            var responceString = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<LuxContentSmsResponse>(responceString);
            if (res == null || res.Result != "ok")
            {
                AddError(res.Data?.Message);
            }
        }

        static string Sha256(string randomString)
        {
            var bytes = Encoding.UTF8.GetBytes(randomString);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += string.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}
