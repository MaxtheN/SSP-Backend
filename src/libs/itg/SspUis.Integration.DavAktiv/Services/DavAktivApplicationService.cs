using Newtonsoft.Json;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DavAktiv
{
    public class DavAktivApplicationService : StatusGenericHandler, IDisposable, IDavAktivApplicationService
    {
        private readonly DavAktivConfig _config;

        private HttpClient _httpClient;
        public DavAktivApplicationService(DavAktivConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            Initialize(config);
        }

        private void Initialize(DavAktivConfig config)
        {
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.ApplicationBasicToken}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.ApplicationBasicToken);
        }

        public async Task<(DavAktivApplicationResponseDto result, HttpResponseMessage response, string responseText, string url)> PostApplication(DavAktivApplicationDto dto)
        {
            var url = $"{_config.ApplicationApi}/savdosanoat/v1";
            var json = JsonConvert.SerializeObject(dto);
            var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Dav aktivga ariza yuborishda xatolik yuz berdi";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }
                AddError($"Dav aktivga ariza yuborishda xatolik yuz berdi: {response.ReasonPhrase} error: {error}");
                return (null!, response, error, url);
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DavAktivApplicationResponseDto>(responseJson)!;
            if (!result.Succes)
                AddError($"Dav aktivga ariza yuborishda xatolik yuz berdi: {result.ErrorMsg}");

            return (result, response, responseJson, url);
        }

        void IDisposable.Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null!;
        }

    }
}
