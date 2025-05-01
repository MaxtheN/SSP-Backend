using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public interface IRecallLeaveService
    : IBaseEntityService<long, RecallLeave, RecallLeaveListDto, RecallLeaveDto, CreateRecallLeaveDlDto, UpdateRecallLeaveDlDto, RecallLeaveSortFilterOptions>
{
    PagedResult<RecallLeaveListDto> GetList(RecallLeaveSortFilterOptions options);
    PagedResult<RecallLeaveListDto> GetListForSigner(RecallLeaveSortFilterOptions dto);
    RecallLeaveDto Get();
    SelectList<long> AsSelectList(RecallLeaveSortFilterOptions options);
    void Accept(UpdateStatusRecallLeaveDto dTo, bool isLoged = false);
    Task Sign(SignStatusRecallLeaveDto dto);
    ValueTask<HaveId<long>> Create(CreateRecallLeaveDlDto dto);
    ValueTask<string> PostToIMZOAndSentUrl(RecallLeave   contract);
    ValueTask<string?> SendUrl(long contractId);
    void Cancel(UpdateStatusRecallLeaveDto dTo);
    void Delete(long id);
    Task<byte[]> DownloadPdf(Guid id2, string? lang);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    byte[]? GetWordTemplate();
    object UploadFiles(StorageFile[] dto);
}