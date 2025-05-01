using Newtonsoft.Json;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.IntegrationCertificate;
using StatusGeneric;
using System.Net.Http.Headers;
using System.Text;

namespace SspUis.Integration.IntegrationCertificateMB
{
    public class IntegrationCertificateBojxonaService :
        StatusGenericHandler,
        IIntegrationCertificateBojxonaService
    {
        private readonly HttpClient _httpClient;
        private readonly IntegrationCertificateConfig _config;

        public IntegrationCertificateBojxonaService(IntegrationCertificateConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicTokenBojxona}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicTokenBojxona);
        }

        public async Task<IntegrationCertificateResponseDto> PostCertificateBojxona(IntegrationCertificateRequestDto dto)
        {
            try
            {
                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.ApiBojxona}/SSP_receive/rest/Service/Sertificate";

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
                            result.Message = responseContent;
                            result.Path = url;
                            return result;
                        }
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                {
                    AddError("Произошла ошибка при получении данных из Bojxona");
                    return result;
                }
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из Bojxona {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
    }
}
