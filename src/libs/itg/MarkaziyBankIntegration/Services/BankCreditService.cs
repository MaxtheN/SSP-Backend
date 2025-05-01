using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SspUis.Integration.BankCredit.Models;
using StatusGeneric;
using System.Net.Http.Headers;
using System.Text;

namespace SspUis.Integration.BankCredit
{
    public class BankCreditService :
        StatusGenericHandler,
        IBankCreditService
    {
        private readonly HttpClient _httpClient;
        private readonly BankCreditConfig _config;

        public BankCreditService(BankCreditConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicToken}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicToken);
        }

        public async Task<List<BankCreditReportResponse>> GetReport()
        {
            var url = $"{_config.Api}/api/v1/ssp/credit_contracts/report";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Банк Кредит тизимидан маълумот олишда хатолик рўй берди!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Банк Кредит тизимидан маълумот олишда хатолик рўй берди!:\n{response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            BankCreditReportResponseDto responseData;
            try
            {
                responseData = JsonConvert.DeserializeObject<BankCreditReportResponseDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (responseData.Status!=200)
            {
                AddError($"Банк Кредит тизимидан маълумот топилмади!");
                return null!;
            }
            return responseData.Data;
        }

        public async Task<BankCreditApplicationsResponse> GetApplications(string tin, int offerSigned= 1)
        {
            var url = $"{_config.Api}/api/v1/ssp/contractor/applications?contractor_tin={tin}&offer_signed={offerSigned}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Банк Кредит тизимидан маълумот олишда хатолик рўй берди!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            BankCreditApplicationsResponseDto responseData;
            try
            {
                responseData = JsonConvert.DeserializeObject<BankCreditApplicationsResponseDto>(responceJson, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                })!;
            }
            catch
            {
                throw;
            }

            if (responseData.Status != 200)
            {
                AddError($"Банк Кредит тизимидан маълумот топилмади!");
                return null!;
            }
            return responseData.Data;
        }

        public async Task<List<BankCreditContractStatusResponse>> GetContractStatus()
        {
            var url = $"{_config.Api}/api/v1/ssp/credit_contracts/status";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Банк Кредит тизимидан маълумот олишда хатолик рўй берди!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Банк Кредит тизимидан маълумот олишда хатолик рўй берди!:\n{response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            BankCreditContractStatusDto responseData;
            try
            {
                responseData = JsonConvert.DeserializeObject<BankCreditContractStatusDto>(responceJson, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                })!
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                ;
            }
            catch
            {
                throw;
            }

            if (responseData.Status != 200)
            {
                AddError($"Банк Кредит тизимидан маълумот топилмади!");
                return null!;
            }
            return responseData.Data;
        }
    }
}
