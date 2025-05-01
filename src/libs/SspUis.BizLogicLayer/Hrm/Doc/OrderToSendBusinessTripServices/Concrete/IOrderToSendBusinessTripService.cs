using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public interface IOrderToSendBusinessTripService
    : IBaseEntityService<long, OrderToSendBusinessTrip, OrderToSendBusinessTripListDto, OrderToSendBusinessTripDto, CreateOrderToSendBusinessTripDlDto, UpdateOrderToSendBusinessTripDlDto,OrderToSendBusinessTripSortFilterOptions>
{
    PagedResult<OrderToSendBusinessTripListDto> GetList(OrderToSendBusinessTripSortFilterOptions options);
    PagedResult<OrderToSendBusinessTripListDto> GetListForSigner(OrderToSendBusinessTripSortFilterOptions dto);
    OrderToSendBusinessTripDto Get();
    OrderToSendBusinessTripTableDto GetByEmployeeId(long employeeId, DateOnly? startOn, DateOnly? endOn);
    SelectList<long> AsSelectList(int? employeeId = null);
    ValueTask<HaveId<long>> Create(CreateOrderToSendBusinessTripDlDto dto);
    ValueTask<string> PostToIMZOAndSentUrl(OrderToSendBusinessTrip contract);
    ValueTask<string?> SendUrl(long contractId);
    void Update(UpdateOrderToSendBusinessTripDlDto dto);
    void Accept(UpdateStatusOrderToSendBusinessTripDto dTo, bool isLoged = false);
    Task Sign(SignStatusOrderToSendBusinessTripDto dto);
    void Cancel(UpdateStatusOrderToSendBusinessTripDto dTo);
    void Delete(long id);
    Task<byte[]> DownloadPdf(Guid id2, string? lang);
    byte[]? GetWordTemplate();
    object UploadFiles(StorageFile[] dto);
}
