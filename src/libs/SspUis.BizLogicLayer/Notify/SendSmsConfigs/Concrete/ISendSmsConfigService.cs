using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Notify
{
    public interface ISendSmsConfigService : IStatusGeneric
    {
        PagedResult<SendSmsConfigListDto> GetList(SendSmsConfigListDtoSortFilterPageOption dto);
        SendSmsConfigDto Get();
        SendSmsConfigDto Get(int id);
        HaveId<int> Create(CreateSendSmsConfigDlDto dto);
        void Update(UpdateSendSmsConfigDlDto dto);
        void Delete(int id);
    }
}
