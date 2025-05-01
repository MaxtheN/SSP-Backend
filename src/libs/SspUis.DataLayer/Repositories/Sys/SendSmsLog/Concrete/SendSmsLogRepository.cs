using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SendSmsLogRepository
        : BaseEntityRepository<long, SendSmsLog, CreateSendSmsLogDlDto, UpdateSendSmsLogDlDto>, ISendSmsLogRepository
    {
        public SendSmsLogRepository(ICrudServices crudServices) 
            : base(crudServices)
        { }


    }
}
