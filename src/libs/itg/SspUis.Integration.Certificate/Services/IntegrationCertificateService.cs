using Newtonsoft.Json;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using StatusGeneric;
using System.Net.Http.Headers;
using System.Text;

namespace SspUis.Integration.IntegrationCertificate
{
    public class IntegrationCertificateService :
        StatusGenericHandler,
        IIntegrationCertificateService
    {
        private readonly HttpClient _httpClient;
        private readonly IntegrationCertificateConfig _config;

        public IntegrationCertificateService(IntegrationCertificateConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicTokenSoliq}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicTokenSoliq);
        }

        public async Task<IntegrationCertificateResponseDto> PostCertificate(IntegrationCertificateRequestDto dto)
        {
            try
            {
                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.Api}/trade-api/api/save/job-certificate-info";
                
               //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var result = new IntegrationCertificateResponseDto();
                result.Path = url;
                result.Status = response.StatusCode != null ? (int)response.StatusCode : 0;

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<IntegrationCertificateResponseDto>(responseContent);

                        if (result != null)
                        {
                            result.Status = response.StatusCode != null ? (int)response.StatusCode : 0;
                            result.Path = url;
                            result.Message = responseContent;

                            return result;
                        }
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                        return result;
                    }
                }
                else
                    AddError("Произошла ошибка при получении данных из (mspd-api.soliq.uz)");
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (mspd-api.soliq.uz) {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
    }
}
