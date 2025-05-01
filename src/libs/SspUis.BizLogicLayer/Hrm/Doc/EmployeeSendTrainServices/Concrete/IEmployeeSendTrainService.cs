using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public interface IEmployeeSendTrainService
    : IBaseEntityService<long, EmployeeSendTrain, EmployeeSendTrainListDto, EmployeeSendTrainDto, CreateEmployeeSendTrainDlDto, UpdateEmployeeSendTrainDlDto,EmployeeSendTrainSortFilterOptions>
{
    PagedResult<EmployeeSendTrainListDto> GetList(EmployeeSendTrainSortFilterOptions dto);
    PagedResult<EmployeeSendTrainListDto> GetListForSigner(EmployeeSendTrainSortFilterOptions dto);
    //PagedResult<UnpaidEmployeeSendTrainListDto> GetUnPaidList(UnpaidEmployeeSendTrainSortFilterPageOptions dto);
    EmployeeSendTrainDto Get();
    EmployeeSendTrainDto Get(long id);
    SelectList<long> AsSelectList(int? employeeId = null);
    ValueTask<HaveId<long>> Create(CreateEmployeeSendTrainDlDto dto);
    ValueTask<string> PostToIMZOAndSentUrl(EmployeeSendTrain contract);
    ValueTask<string?> SendUrl(long contractId);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    void Accept(UpdateStatusEmployeeSendTrainDto dTo, bool isLoged = false);
    Task Sign(SignStatusEmployeeSendTrainDto dto);
    void Cancel(UpdateStatusEmployeeSendTrainDto dTo);
    void Update(UpdateEmployeeSendTrainDlDto dto);
    Guid SaveFile(long docId, string data, string fileName);
    void Delete(long id);
    Task<byte[]> DownloadPdf(Guid id2);
    byte[]? GetWordTemplate();
    object UploadFiles(StorageFile[] dto);
}