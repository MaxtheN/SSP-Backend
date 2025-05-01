using Newtonsoft.Json;
using StatusGeneric;
using System.Text;

namespace SspUis.Integration.OnlineMahalla
{
    public class OnlineMahallaService :
        StatusGenericHandler,
        IOnlineMahallaService
    {
        private readonly HttpClient _httpClient;
        private readonly OnlineMahallaConfig _config;

        public OnlineMahallaService(OnlineMahallaConfig config)
        {
            _httpClient = new HttpClient();
            _config = config;
            _httpClient.DefaultRequestHeaders.Add("token", _config.Token);
            //if (config.UseProxy)
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
            //}
        }

        public async Task<List<OnlineMahallaDataDto>> Get(long updateId)
        {
            var url = $"{_config.Api}/api/v1/integration/SSP/districts?update_id={updateId}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("token", _config.Token);

            var response = await _httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<OnlineMahallaResponseDto<List<OnlineMahallaDataDto>>>(content);

                    if (result != null)
                        return result.Data;
                }
                catch
                {
                }
            }
            AddError("Произошла ошибка при получении данных из (online-mahalla.uz)");
            return null;
        }

        public async Task<OnlineMahallaResponseDto<OnlineMahallaPostDataDto>> PostApplication(OnlineMahallaRequestDto dto)
        {
            try
            {
                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.Api}/api/v1/integration/SSP/post-application";

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

                httpRequest.Content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(httpRequest);

                try
                {

                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<OnlineMahallaResponseDto<OnlineMahallaPostDataDto>>(responseContent);
                    if (!response.IsSuccessStatusCode)
                    {
                        AddError($"Произошла ошибка при получении данных из (online-mahalla.uz). {result!.Message}");
                        return null!;
                    }
                    
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
                    AddError($"The object can not be deserialized or another error, status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (online-mahalla.uz) {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }

        public async Task<MahallaApplicationStatusUpdateResponseDto> UpdateApplicationStatus(MahallaApplicationStatusUpdateRequestDto dto)
        {
            try
            {
                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.Api}/api/v1/ssp_application_status/update";
                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<MahallaApplicationStatusUpdateResponseDto>(responseContent);

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
                    }
                }
                else
                    AddError($"Произошла ошибка при получении данных из (online-mahalla.uz). DtoJson: {dtoJson}   Response:  {JsonConvert.SerializeObject(response)}");
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (online-mahalla.uz) {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }

    }
}
