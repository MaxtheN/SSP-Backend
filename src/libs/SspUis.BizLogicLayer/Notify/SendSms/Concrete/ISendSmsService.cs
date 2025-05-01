using StatusGeneric;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Notify
{
    public interface ISendSmsService : IStatusGeneric
    {
        Task SendSms(int tableId, int? fromStatusId, int? toStatusId, string phoneNumber); 
    }
}
