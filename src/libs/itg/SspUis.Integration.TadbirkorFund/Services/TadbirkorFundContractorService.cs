using Newtonsoft.Json;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.TadbirkorFund
{
    public class TadbirkorFundContractorService : StatusGenericHandler, IDisposable, ITadbirkorFundContractorService
    {
        private readonly TadbirkorFundConfig _config;

        private HttpClient _httpClient;
        public TadbirkorFundContractorService(TadbirkorFundConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            Initialize(config);
        }

        public void Initialize(TadbirkorFundConfig config)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicToken);
        }

        public async Task<TadbirkorFundContractorDto> GetByInn(string inn)
        {
            var url = $"{_config.Api}/ords/fond/ssp/request?tin_pinfl={inn}";
            var response = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(new {})));
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Маълумот топилмади! Информация не найдена {inn}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }
                AddError($"{response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            TadbirkorFundContractorDto contractor;
            try
            {
                contractor = JsonConvert.DeserializeObject<TadbirkorFundContractorDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (contractor == null  || contractor.Id==0)
            {
                AddError($"Маълумот топилмади! Информация не найдена {inn}");
                return null!;
            }
            return contractor;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null!;
        }
    }
}
