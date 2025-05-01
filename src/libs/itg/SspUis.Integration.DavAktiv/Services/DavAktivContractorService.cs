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
    public class DavAktivContractorService : StatusGenericHandler, IDisposable, IDavAktivContractorService
    {
        private readonly DavAktivConfig _config;

        private HttpClient _httpClient;
        public DavAktivContractorService(DavAktivConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            Initialize(config);
        }

        public void Initialize(DavAktivConfig config)
        {
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicToken}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicToken);
        }

        public async Task<DavAktivContractorDto> GetByInn(string inn)
        {
            var url = $"{_config.Api}/reestr/v4";
            var response = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(new { tin = inn }), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Маълумот топилмади DavAktiv! Информация не найдена {inn}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }
                AddError($"{response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            DavAktivContractorResponseDto contractor;
            try
            {
                contractor = JsonConvert.DeserializeObject<DavAktivContractorResponseDto>(responceJson)!;
                if (!contractor.Success)
                    contractor.Data = null!;    
            }
            catch
            {
                throw;
            }

            if (contractor.Data == null || contractor.Data.Count() == 0)
            {
                AddError($"Маълумот топилмади DavAktiv! Информация не найдена {inn} ({contractor.Message})");
                return null!;
            }
            return contractor.Data.FirstOrDefault();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null!;
        }
    }
}
