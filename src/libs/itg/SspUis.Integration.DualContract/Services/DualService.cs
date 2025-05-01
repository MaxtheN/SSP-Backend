using Newtonsoft.Json;
using SspUis.Integration.Dual.Configs;
using SspUis.Integration.Dual.Models;
using StatusGeneric;
using System.Net.Http.Headers;

namespace SspUis.Integration.Dual.Services;

public class DualService : StatusGenericHandler, IDualService
{
    private readonly DualConfig _config;

    private HttpClient _httpClient;
    public DualService(DualConfig config)
    {
        _httpClient = new HttpClient();
        _config = config;
        Initialize(config);
    }

    public void Initialize(DualConfig config)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicTokenClient);
    }
   
    public async Task<string> RejectDualContract(DualContractRejectDto dto)
    {
        try
        {
            var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

            var url = $"{_config.api}/DualContract/Reject";

            //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

            var content = new StringContent(dtoJson, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"Ma'lumot saqlashda xatolik!!!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch
                {
                    AddError($"Ma'lumot saqlashda xatolik!!!: {response.ReasonPhrase} error: {error}");
                }
            }

            return "Success created";
        }
        catch (Exception ex)
        {
            AddError($"Ma'lumot saqlashda xatolik!!! {ex.Message}: {ex.InnerException}");
            return "Error creating!!!";
        }
    }   
}