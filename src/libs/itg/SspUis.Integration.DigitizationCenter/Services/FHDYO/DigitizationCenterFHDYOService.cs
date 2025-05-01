using Newtonsoft.Json;
using SspUis.Integration.DigitizationCenter.Models.FHDYO;
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
    public class DigitizationCenterFHDYOService : StatusGenericHandler, IDigitizationCenterFHDYOService
    {
        private readonly DigitizationCenterConfig _config;
        private HttpClient _httpClient;
        private readonly IDigitizationCenterLoginService _digitizationCenterService;

        public DigitizationCenterFHDYOService(HttpClient httpClient, DigitizationCenterConfig config, IDigitizationCenterLoginService digitizationCenterService)
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
        public async Task<List<DeathInfoResponseDto>> GetDeathInfoByPinfl(DeathInfoRequestDto dto)
        {
            try
            {
                var url = $"{_config.Api}/justice/service/birth/v1";

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

                        var result = JsonConvert.DeserializeObject<DeathInfoResult>(responseContent);

                        if (result != null)
                        {
                            return result.Items;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                    AddError($"FHDYO. Response:  {response.StatusCode}");
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (FHDYO) {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
        public async Task<List<BirthInfoResponseDto>> GetBirthInfo(BirthInfoRequestDto dto)
        {
            try
            {
                var url = $"{_config.Api}/justice/service/birth/v1";

                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                await _digitizationCenterService.Login();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<BirthInfoResult>(responseContent);

                        if (result != null)
                        {
                            return result.Items;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                    AddError($"FHDYO. Response:  {response.StatusCode}");
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (FHDYO) {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
    }
}
