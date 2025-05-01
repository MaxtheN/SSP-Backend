using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SspUis.Integration.AgroBank.Configs;
using SspUis.Integration.AgroBank.Models;
using StatusGeneric;
using System.Net.Http.Headers;
using System.Text;


namespace SspUis.Integration.AgroBank.Services;

public class AgroBankService : StatusGenericHandler, IAgroBankService
{
    private readonly HttpClient _httpClient;
    private readonly AgroBankConfig _config;


    public AgroBankService(AgroBankConfig config)
    {
        _httpClient = new HttpClient();

        Initialize(config);
        _config = config;
    }
    public async Task Initialize(AgroBankConfig config)
    {
        if (config.UseProxy)
        {
            var url = config.Api;
            config.Api = "https://proxy.chamber.uz/" + url;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicToken);

            _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.AccessToken}");
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.AccessToken);
        }
    }

    public async Task<GetLoanActualDataResponse2> GetLoanActualData(string loanId)
    {
        try
        {
           
            string url = _config.Api + $"/crm-delta/agrotpp?crmLoanId={loanId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
             
                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<GetLoanActualDataResponse2>(responseContent);

                    if (result != null)
                    {
                        return result;
                    }
                    return null;
                }
                catch (Exception)
                {
                    AddError("The object can not be deserialized");
                }
            }
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (Agro Bank) {ex.Message}: {ex.InnerException}");
        }

        return null!;


    }
}


 
