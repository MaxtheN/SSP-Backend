using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using StatusGeneric;
using System.Text;
using SspUis.Integration.IntegrationCertificate;

namespace SspUis.Integration.Certificate;

public class MoliyaIntegrationCertificateService : StatusGenericHandler, IMoliyaIntegrationCertificateService
{
    private readonly HttpClient _httpClient;
    private readonly IntegrationCertificateConfig _config;

    public MoliyaIntegrationCertificateService(IntegrationCertificateConfig config)
    {
        _httpClient = new HttpClient();
        _config = config;
    }
    public async Task Login()
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { username = _config.UserNameMoliya, password = _config.PasswordMoliya });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{_config.Api}" + "/v1/atm/auth/login";

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                string error = "";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Moliya tizimidan avtorizatsiyadan o'tishda xatolik!: {response.ReasonPhrase} error: {error}");
                return;
            }
            try
            {
                var responceJson = await response.Content.ReadAsStringAsync();
                MoliyaLoginResponseDto res;
                res = JsonConvert.DeserializeObject<MoliyaLoginResponseDto>(responceJson, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                })!;
                if (res.Token == null)
                {
                    AddError("Moliya tizimidan token olishda xatolik");
                }
                _config.TokenMoliya = res.Token;

            }
            catch
            {
                throw;
            }

            return;
        }
        catch (Exception ex)
        {
            AddError($"Moliya tizimidan avtorizatsiyadan o'tishda xatolik! {ex.Message}: {ex.InnerException}");
        }
    }
}
