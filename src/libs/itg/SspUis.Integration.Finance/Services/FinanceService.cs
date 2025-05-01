using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SspUis.Integration.Finance.Configs;
using SspUis.Integration.Finance.Models;
using StatusGeneric;
using System.Net.Http.Headers;
using System.Text;


namespace SspUis.Integration.Finance.Services;

public class FinanceService : StatusGenericHandler , IFinanceService
{
    private readonly FinanceConfig _config;
    private HttpClient _httpClient;


    public FinanceService(FinanceConfig config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
        Initialize(config);
    }

    public void Initialize(FinanceConfig config)
    {
        Login().Wait();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
    }

    public async Task<List<GetPayDocsDto>> GetPayDocsAsync(string code,string date)
    {
        try
        {
            

             //var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

             var url = $"{_config.Api}/test_be/paydoc/getPayDocsByAccEqualsAndBankDateBetween/{code}/{date}/{date}";

            //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

            //var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"Молия тизимидан маълумот олишда хатолик рўй берди! ";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Молия тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            List<GetPayDocsDto> res;

            try
            {
                res = JsonConvert.DeserializeObject<List<GetPayDocsDto>>(responceJson)!;
                if (res == null)
                {
                    AddError($"Молия тизимидан маълумот топилмади! ");
                    return null;
                }
                
            }
            catch
            {
                throw;
            }


            return res;
        }
        catch (Exception ex)
        {
            AddError($"Молия тизимидан маълумот олишда хатолик рўй берди! {ex.Message}: {ex.InnerException}");
        }

        return null!;
    }

    public async Task Login()
    {
        try
        {
            var dto = new FinanceLoginRequestDto()
            {
                Login = _config.UserName,
                Password = _config.Password
            };
            var dtoJson = JsonConvert.SerializeObject(dto);

            var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

           var url = $"{_config.LoginApi}";

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                string error = "";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Moliyadan avtorizatsiyadan o'tishda xatolik!: {response.ReasonPhrase} error: {error}");
                return;
            }
            try
            {
                var token = response.Headers.Where(s => s.Key == "Authorization").FirstOrDefault().Value.FirstOrDefault().ToString();
                if (token == null)
                {
                    AddError("Moliyadan token olishda xatolik");
                    return;
                }
                _config.AccessToken = token;
              
                

            }
            catch
            {
                throw;
            }
            return;
        }
        catch (Exception ex)
        {
            AddError($"Moliyadan avtorizatsiyadan o'tishda xatolik! {ex.Message}: {ex.InnerException}");
        }
    }
}
