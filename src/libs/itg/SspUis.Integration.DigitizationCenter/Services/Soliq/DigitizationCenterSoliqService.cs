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
using SspUis.Integration.DigitizationCenter.Soliq;

namespace SspUis.Integration.DigitizationCenter
{
    public class DigitizationCenterSoliqService : StatusGenericHandler, IDigitizationCenterSoliqService
    {
        private readonly DigitizationCenterConfig _config;
        private HttpClient _httpClient;
        private readonly IDigitizationCenterLoginService _digitizationCenterService;

        public DigitizationCenterSoliqService(HttpClient httpClient, DigitizationCenterConfig config, IDigitizationCenterLoginService digitizationCenterService)
        {
            _config = config;
            _httpClient = httpClient;
            _digitizationCenterService = digitizationCenterService;
            Initialize(config);
        }
        public async Task Initialize(DigitizationCenterConfig config)
        {
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicToken);
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Bearer {config.AccessToken}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.Token);
        }
        public async Task<List<LegalentityDebtResponseDto>> GetLegalentityDebt(LegalentityDebtRequestDto dto)
        {
            try
            {

                var url = $"{_config.Api}/gnk/service/legalentity/debt/v1";

                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                await _digitizationCenterService.Login();

                var response = await _httpClient.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);


                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<LegalentityDebtResponseResultDto>(responseContent);

                        if (result != null)
                        {
                            return result.data;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                    AddError($"Soliq. Response:  {responseObject}");
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (Soliq) {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
    }
}
