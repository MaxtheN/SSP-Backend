using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBASE.Notify.Sms
{
    public class ProviderSmsService : IProviderSmsService
    {
        public System.Net.Http.HttpClient Client { get; }

        public ProviderSmsService(System.Net.Http.HttpClient httpClient, SmsProviderConfig smsProviderInfo)
        {
            //httpClient.BaseAddress = new Uri(roamingIntegrationInfo.mysoliquz.api);
            if (!string.IsNullOrWhiteSpace(smsProviderInfo.Authorization))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", smsProviderInfo.Authorization);
            }
            Client = httpClient;
        }
    }
}
