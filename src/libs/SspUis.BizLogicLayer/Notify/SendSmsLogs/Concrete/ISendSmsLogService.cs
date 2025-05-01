using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Notify
{
    public interface ISendSmsLogService : IStatusGeneric
    {
        List<SendSmsLogListDto> GetList();
        HaveId<long> Create(CreateSendSmsLogDlDto dto);
    }
}
