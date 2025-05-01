using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface ISendSmsConfigRepository 
        : IBaseEntityRepository<int, SendSmsConfig, CreateSendSmsConfigDlDto, UpdateSendSmsConfigDlDto>
    {

    }
}
