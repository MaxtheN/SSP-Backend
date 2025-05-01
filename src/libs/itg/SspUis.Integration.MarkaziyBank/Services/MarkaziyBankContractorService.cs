using Newtonsoft.Json;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.MarkaziyBank
{
    public class MarkaziyBankContractorService : StatusGenericHandler, IDisposable, IMarkaziyBankContractorService
    {
        private readonly MarkaziyBankConfig _config;

        private HttpClient _httpClient;
        public MarkaziyBankContractorService(MarkaziyBankConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            Initialize(config);
        }

        public void Initialize(MarkaziyBankConfig config)
        {
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicToken}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicToken);
        }

        public async Task<List<MarkaziyBankContractorCreditHistoryDto>> GetCreditHistoryByInn(string inn, string fromDate, string toDate)
        {
            var url = $"{_config.Api}/api/v1/credit/history?tin={inn}&fromDate={fromDate}&toDate={toDate}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"Банк кредит тизимидан маълумот олишда хатолик рўй берди! {inn}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }
                AddError($"Банк кредит тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            MarkaziyBankContractorCreditHistoryResponseDto contractor;
            try
            {
                contractor = JsonConvert.DeserializeObject<MarkaziyBankContractorCreditHistoryResponseDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (contractor.Data == null)
            {
                AddError($"Банк кредит тизимидан маълумот топилмади! Информация не найдена {inn} ({contractor.Message})");
                return null!;
            }
            return contractor.Data;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null!;
        }
    }
}
