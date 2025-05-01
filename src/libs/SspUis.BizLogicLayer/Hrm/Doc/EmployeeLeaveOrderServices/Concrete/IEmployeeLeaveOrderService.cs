using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public interface IEmployeeLeaveOrderService
    : IBaseEntityService<long, EmployeeLeaveOrder, EmployeeLeaveOrderListDto, EmployeeLeaveOrderDto, CreateEmployeeLeaveOrderDlDto, UpdateEmployeeLeaveOrderDlDto, EmployeeLeaveOrderSortFilterOptions>
{
    //PagedResult<EmployeeLeaveOrderListDto> GetListAll(EmployeeLeaveOrderSortFilter dto);
    PagedResult<EmployeeLeaveOrderListDto> GetList(EmployeeLeaveOrderSortFilterOptions dto);
    PagedResult<EmployeeLeaveOrderListDto> GetListForSigner(EmployeeLeaveOrderSortFilterOptions dto);
    //PagedResult<UnpaidEmployeeLeaveOrderListDto> GetUnPaidList(UnpaidEmployeeLeaveOrderSortFilterPageOptions dto);
    EmployeeLeaveOrderDto Get();
    EmployeeLeaveOrderDto Get(long id);
    EmployeeLeaveOrderTableDto GetByEmployeeId(int employeeId, DateOnly? startOn, DateOnly? endOn);
    SelectList<long> AsSelectList(int? employeeId);
    SelectList<long> GetTableAsSelectList(long ownerId, long? employeeId);
    HaveId<long> Create(CreateEmployeeLeaveOrderDlDto dto);
    void Accept(UpdateStatusEmployeeLeaveOrderDto dTo, bool isLoged = false);
    void Cancel(UpdateStatusEmployeeLeaveOrderDto dTo);
    void Update(UpdateEmployeeLeaveOrderDlDto dto);
    void Delete(long id);
    Task Sign(SignStatusEmployeeLeaveOrdeDto dto);
    int GetCalculatedDays(int employeeManageId,
                          DateTime startDate,
                          DateTime endDate);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    Task<byte[]> DownloadPdf(Guid id2, string? lang);
    byte[]? GetWordTemplate();
    object UploadFiles(StorageFile[] dto);
    ValueTask<string> PostToIMZOAndSentUrl(EmployeeLeaveOrder contract);
    ValueTask<string?> SendUrl(long contractId);
}