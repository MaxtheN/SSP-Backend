using StatusGeneric;
using System.Threading.Tasks;

namespace WEBASE.Notify.Sms.LuxContent
{
    public interface ILuxContentSmsService : IStatusGeneric
    {
        Task Send(string phonenumber, string message);
    }
}