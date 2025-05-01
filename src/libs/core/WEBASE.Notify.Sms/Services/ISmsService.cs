using System.Threading.Tasks;

namespace WEBASE.Notify.Sms
{
    public interface ISmsService
    {
        Task Send(string phonenumber, string message);
    }
}