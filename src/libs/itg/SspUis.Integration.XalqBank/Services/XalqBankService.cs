

using Newtonsoft.Json;
using SspUis.Integration.XalqBank.Configs;
using SspUis.Integration.XalqBank.Models;
using StatusGeneric;
using System.Net.Http;

namespace SspUis.Integration.XalqBank.Services;

public class XalqBankService : StatusGenericHandler, IXalqBankService
{
    private readonly XalqBankConfig _config;

    private readonly HttpClient _httpClient;
    public XalqBankService(XalqBankConfig config)
    {
        _httpClient = new HttpClient();
        _config = config;

    }

    public async Task<ChekAmountResponse> ChekAmountXalqBank(string loanId)
    {
        try
        {
            string url =  $"{_config.Api}/loanInfo/api/client/loan?loanId={loanId}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ChekAmountResponse>(responseContent);

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
            AddError($"Произошла ошибка при получении данных из (Xalq Bank) {ex.Message}: {ex.InnerException}");
        }

        return null!;
    }
}
