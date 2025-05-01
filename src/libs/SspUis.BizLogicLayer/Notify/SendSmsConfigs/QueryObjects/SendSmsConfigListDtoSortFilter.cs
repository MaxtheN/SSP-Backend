using SspUis.BizLogicLayer.Hrm.EmployeeManageServices;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Notify
{
    public static class SendSmsConfigListDtoSortFilter
    {
        public static IQueryable<SendSmsConfigListDto> SortFilter(
            this IQueryable<SendSmsConfigListDto> query,
            SendSmsConfigListDtoSortFilterPageOption options)
        {
            query = query.Where(a => a.StateId != StateIdConst.PASSIVE);

            if (options.TableId.HasValue)
                query = query.Where(a => a.TableId == options.TableId.Value);

            if (options.FromStatusId.HasValue)
                query = query.Where(a => a.FromStatusId == options.FromStatusId.Value);

            if (options.ToStatusId.HasValue)
                query = query.Where(a => a.ToStatusId == options.ToStatusId.Value);

            if (options.HasSearch())
                query = query.Where(a => a.Title.ToLower().Contains(options.Search.ToLower())
                                      || a.SmsText.ToLower().Contains(options.Search.ToLower()));

            return query;
        }
    }
}
