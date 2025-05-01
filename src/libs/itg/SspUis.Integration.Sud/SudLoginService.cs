using Newtonsoft.Json;
using SspUis.Integration.Sud.Configs;
using SspUis.Integration.Sud.Models.AuthModels;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud
{
    public class SudLoginService : StatusGenericHandler, ISudLoginService
    {
        private readonly SudConfig _sudConfig;
        private readonly HttpClient _httpClient;

        public SudLoginService(SudConfig sudConfig)
        {
             _sudConfig = sudConfig;
            _httpClient = new HttpClient();
        }

        public async Task<string> SudAuthLoginCreate()
        {
            try
            {
                var url = $"{_sudConfig.api}/e_s/authorize/create";
                var values = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>( "client_id", _sudConfig.ClientId),
                    new KeyValuePair<string, string>("client_secret", _sudConfig.ClientSecret)
                };
                var data = new FormUrlEncodedContent(values);
                data.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await _httpClient.PostAsync(url, data);

                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        AddError($"responseda hatolik SudAuthLoginCreate()", "Message");
                        return null;
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<SudAuthCreateResponseDto>(responseContent);
                        if (result != null)
                        {
                            return result.session_id.ToString();
                        }
                    }

                }
                catch (Exception)
                {
                    AddError($"The object can not be deserialized or another error, status: {response.StatusCode}", "Message");
                }
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (sud) {ex.Message}: {ex.InnerException}", "Message");
            }

            return null!;
        }
    }
}
