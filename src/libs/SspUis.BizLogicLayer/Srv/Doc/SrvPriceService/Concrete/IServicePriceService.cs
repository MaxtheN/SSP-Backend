using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface IServicePriceService
        : IBaseEntityService<long, ServicePrice, ServicePriceListDto, ServicePriceDto, CreateServicePriceDlDto, UpdateServicePriceDlDto, ServicePriceSortFilterOption>
    {
        List<ServicePriceGroupListDto> GroupingByServicePrice(GroupingByServicesPriceDtoFilter dto);
        ServicePriceDto CloneServicePrice(long id);
        HaveId<long> Cancel(CancelStatusSrvPriceDto dto);
        HaveId<long> Accept(AcceptStatusSrvPriceDto dto);
        SelectList<long> AsSelectList(ServicePriceSortFilterOption options);
    }
}
