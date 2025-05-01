using Newtonsoft.Json;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.Integration.DigitizationCenter.Soliq;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter.Services.GSP
{
    public class DigitizationCenterGspService : StatusGenericHandler, IDigitizationCenterGspService
    {
        private readonly DigitizationCenterConfig _config;
        private HttpClient _httpClient;
        private readonly IDigitizationCenterLoginService _digitizationCenterService;

        public DigitizationCenterGspService(HttpClient httpClient, DigitizationCenterConfig config, IDigitizationCenterLoginService digitizationCenterService)
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
        public async Task<List<GSPNewApiData>> GetFromGSP(GSPNewApiRequestDto dto)
        {
            try
            {

                _digitizationCenterService.Login().Wait();
                
                var url = $"{_config.Api}/gcp/docrest/v1";

                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json"); 

                var response = await _httpClient.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);


                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<GSPNewApiResponseDto>(responseContent);

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
                    AddError($"GSP. Response:  {responseObject}");
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (GSP) {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
    }
}
