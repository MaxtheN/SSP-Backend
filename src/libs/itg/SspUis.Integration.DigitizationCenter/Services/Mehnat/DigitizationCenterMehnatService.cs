using Newtonsoft.Json;
using SspUis.Integration.DigitizationCenter.Models.Mehnat;
using SspUis.Integration.DigitizationCenter.Soliq;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter
{
    public class DigitizationCenterMehnatService : StatusGenericHandler, IDigitizationCenterMehnatService
    {
        private readonly DigitizationCenterConfig _config;
        private HttpClient _httpClient;
        private readonly IDigitizationCenterLoginService _digitizationCenterService;

        public DigitizationCenterMehnatService(HttpClient httpClient, DigitizationCenterConfig config, IDigitizationCenterLoginService digitizationCenterService)
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
        public async Task<Data> GetMehnatHistory(WorkPositionHistoryRequestDto dto)
        {
            try
            {
                var url = $"{_config.Api}/labour/service/citizenhistory/v1";

                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                await _digitizationCenterService.Login();

                var response = await _httpClient.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                //var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);


                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<WorkPositionHistoryResponseDto>(responseContent);

                        if (result != null)
                        {
                            return result.Result.Data;
                        }
                        else
                        {
                            AddError("Маълумот топилмади");
                        }
                    }
                    
                    catch (Exception)
                    {
                        AddError("Маълумот топилмади");
                    }
                }
                else
                    AddError($"Mehnat. Response:  {response}");
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (Mehnat) {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
        public async Task<NumberOfWorkersData> GetWorkersCount(NumberOfWorkersRequestDto dto)
        {
            try
            {
                var url = $"{_config.Api}/labour/service/staffcount/v1";

                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                await _digitizationCenterService.Login();

                var response = await _httpClient.PostAsync(url, content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<NumberOfWorkersResponseDto>(responseContent);

                        if (result != null)
                        {
                            return result.result.data;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                    AddError($"Mehnat. Response:  {response}");
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (Mehnat) {ex.Message}: {ex.InnerException}");
            }

            return null!;

        }
    }
}
