using System.Net.Http;

namespace WEBASE.Notify.Sms
{
    public interface IProviderSmsService
    {
        HttpClient Client { get; }
    }
}