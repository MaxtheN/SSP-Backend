using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface IRestrictionSendingAppService : IStatusGeneric
    {
        PagedResult<RestrictionSendingAppListDto> GetList(RestrictionSendingAppDtoSortFilter dto);
        RestrictionSendingAppDto Get();
        RestrictionSendingAppDto Get(long id);
        HaveId<long> Create(CreateRestrictionSendingAppDlDto dto);
        void Update(UpdateRestrictionSendingAppDlDto dto);
        void Delete(long id);
    }
}