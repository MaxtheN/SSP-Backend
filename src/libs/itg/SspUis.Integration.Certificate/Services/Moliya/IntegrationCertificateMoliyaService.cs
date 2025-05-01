using Newtonsoft.Json;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.IntegrationCertificate;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Certificate
{
    public class IntegrationCertificateMoliyaService : StatusGenericHandler, IIntegrationCertificateMoliyaService
    {
        private readonly HttpClient _httpClient;
        private readonly IntegrationCertificateConfig _config;

        public IntegrationCertificateMoliyaService(IntegrationCertificateConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            //if (config.UseProxy)
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
            //    _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicTokenMoliya}");
            //}
            //else
               _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Token);
        }
        public async Task<IntegrationCertificateResponseDto> PostCertificateMoliya(IntegrationCertificateRequestDto dto)
        {
            try
            {
                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.ApiMoliya}/v1/atm/ssp";

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var result = new IntegrationCertificateResponseDto();
              

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<IntegrationCertificateResponseDto>(responseContent);

                        if (result != null)
                        {
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
