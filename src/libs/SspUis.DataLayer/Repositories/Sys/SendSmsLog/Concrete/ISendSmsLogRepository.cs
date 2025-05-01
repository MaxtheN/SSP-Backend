using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface ISendSmsLogRepository 
        : IBaseEntityRepository<long, SendSmsLog, CreateSendSmsLogDlDto, UpdateSendSmsLogDlDto>
    {

    }
}
