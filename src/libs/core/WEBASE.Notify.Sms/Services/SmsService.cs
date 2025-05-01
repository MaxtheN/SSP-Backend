using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBASE.Notify.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IProviderSmsService _providerSmsService;
        private readonly SmsProviderConfig _config;
        public SmsService(IProviderSmsService providerSmsService, SmsProviderConfig config)
        {
            _providerSmsService = providerSmsService;
            this._config = config;
        }

        [AutomaticRetry(Attempts = 3)]
        [Queue("01_sms_send")]
        public async Task Send(string phonenumber, string message)
        {
            if (string.IsNullOrWhiteSpace(phonenumber))
                throw new Exception("Хатолик коди: 8050 - Телефон рақам кўрсатилмаган // Код ошибки: 8050 - Номер телефона не указан");

            phonenumber = phonenumber.Replace("+", "").Replace("-", "");

            if (!phonenumber.StartsWith("998") && phonenumber.Length != 12)
                phonenumber = "998" + phonenumber;

            if (_config.HttpMethod.ToLower() == "get")
            {
                var response = await _providerSmsService.Client.GetAsync(String.Format(_config.ProviderUrl, phonenumber, message));
                if (!response.IsSuccessStatusCode)
                {
                    string error;
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    throw new Exception(error);
                }

            }
            if (_config.HttpMethod.ToLower() == "post")
            {
                var response = await _providerSmsService.Client.PostAsync(String.Format(_config.ProviderUrl, phonenumber, message), new System.Net.Http.StringContent(String.Format(_config.HttpBody, phonenumber, message), System.Text.Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    string error = "";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    throw new Exception(error);
                }
            }
        }
    }
}