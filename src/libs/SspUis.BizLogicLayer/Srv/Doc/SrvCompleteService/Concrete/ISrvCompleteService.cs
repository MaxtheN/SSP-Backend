using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public interface ISrvCompleteService
        : IBaseEntityService<long, CompletedService, CompletedServiceListDto, CompletedServiceDto, CreateCompletedServiceDlDto, UpdateCompletedServiceDlDto, SrvCompleteSortFilterOption>
    {
        void Accept(UpdateStatusCompleteService.AcceptStatusCompletedSrvDto dto);
        void Cancel(UpdateStatusCompleteService.CancelStatusCompletedSrvDto dto);
    }
}