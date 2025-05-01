using Newtonsoft.Json;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.OneId;

public class OneIdService : StatusGenericHandler, IOneIdService
{
    private HttpClient _httpClient;
    private readonly OneIdConfig _config;

    public OneIdService(OneIdConfig config)
    {
        _httpClient = new HttpClient();
        _config = config;
        Initialize(config);
    }
    public void Initialize(OneIdConfig config)
    {
        if (config.UseProxy)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJveHk6VWJ2YzY1I2NidiMkXmhqR3NkZkhKc2RmR2RzZnRkYXNkZnN5dF4mc2ZkJF4qKCgpKigmODd5ZnNmWVRF");
    }

    public async Task<AccessTokenResponseDto?> GetAccessToken(string code, string redirectUrl)
    {
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("grant_type", "one_authorization_code");
        queryString.Add("client_id", _config.ClientId);
        queryString.Add("client_secret", _config.Secret);
        queryString.Add("code", code);
        queryString.Add("redirect_uri", redirectUrl);

        var res = await _httpClient.PostAsync($"{_config.BaseUri}?{queryString}", default);
        var resContentString = await res.Content.ReadAsStringAsync();
        if (res.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<AccessTokenResponseDto>(resContentString);
        }

        var error = JsonConvert.DeserializeObject<ErrorResponseDto>(resContentString);
        AddError(error?.Message);
        return null;
    }

    public async Task Logout(string accessToken)
    {
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("grant_type", "one_log_out");
        queryString.Add("client_id", _config.ClientId);
        queryString.Add("client_secret", _config.Secret);
        queryString.Add("access_token", accessToken);
        queryString.Add("scope", _config.Scope);

        var res = await _httpClient.PostAsync($"{_config.BaseUri}?{queryString}", default);
        var resContentString = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            var error = JsonConvert.DeserializeObject<ErrorResponseDto>(resContentString);
            AddError(error?.Message);
        }
    }

    public async Task<OneIdUserDataDto?> GetUserData(string accessToken)
    {
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("grant_type", "one_access_token_identify");
        queryString.Add("client_id", _config.ClientId);
        queryString.Add("client_secret", _config.Secret);
        queryString.Add("access_token", accessToken);
        queryString.Add("scope", _config.Scope);

        var res = await _httpClient.PostAsync($"{_config.BaseUri}?{queryString}", default);
        var resContentString = await res.Content.ReadAsStringAsync();
        if (res.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<OneIdUserDataDto>(resContentString);
        }

        var error = JsonConvert.DeserializeObject<ErrorResponseDto>(resContentString);
        AddError(error?.Message);
        return null;
    }
}
