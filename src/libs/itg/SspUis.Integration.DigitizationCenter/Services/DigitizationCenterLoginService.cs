using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace SspUis.Integration.DigitizationCenter
{
    public class DigitizationCenterLoginService : StatusGenericHandler, IDigitizationCenterLoginService
    {
        private readonly HttpClient _httpClient;
        private readonly DigitizationCenterConfig _config;

        public DigitizationCenterLoginService(DigitizationCenterConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;

            //_httpClient.DefaultRequestHeaders.Add("Content-Type1", "application/json");
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicToken);
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.Token}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.Token);
        }
        public async Task Login()
        {
            try
            {
                var content = new StringContent("{\"key\":\"value\"}", Encoding.UTF8, "application/json");
                var url = $"{_config.LoginApi}" + "/oauth2/token?grant_type=password" + $"&username={_config.UserName}" + $"&password={_config.Password}";

                var response = await _httpClient.PostAsync(url, content);
                if(!response.IsSuccessStatusCode)
                {
                    string error = "";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch { }

                    AddError($"Raqamlashtirish markazidan avtorizatsiyadan o'tishda xatolik!: {response.ReasonPhrase} error: {error}");
                    return;
                }
                try
                {
                    var responceJson = await response.Content.ReadAsStringAsync();
                    DigitizationCenterLoginResponseDto res;

                    res = JsonConvert.DeserializeObject<DigitizationCenterLoginResponseDto>(responceJson, new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    })!;

                    if(res.AccessToken == null) 
                    {
                        AddError("Raqamlashtirish markazidan token olishda xatolik");
                        return;
                    }
                    _config.AccessToken = res.AccessToken;
                  
                }
                catch
                {
                    throw;
                }
                return;
            }
            catch (Exception ex)
            {
                AddError($"Raqamlashtirish markazidan avtorizatsiyadan o'tishda xatolik! {ex.Message}: {ex.InnerException}");
            }
        }
    }
}
