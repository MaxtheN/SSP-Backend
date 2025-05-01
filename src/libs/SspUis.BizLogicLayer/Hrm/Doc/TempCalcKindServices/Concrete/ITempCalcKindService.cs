using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public interface ITempCalcKindService
    : IBaseEntityService<long, TempCalcKind, TempCalcKindListDto, TempCalcKindDto, CreateTempCalcKindDlDto, UpdateTempCalcKindDlDto, TempCalcKindSortFilterOptions>
{
    public ValueTask<HaveId<long>> Create(CreateTempCalcKindDlDto dto);
    PagedResult<TempCalcKindListDto> GetList(TempCalcKindSortFilterOptions dto);
    SelectList<long> AsSelectList(TempCalcKindSortFilterOptions options);
    Task Sign(SignStatusTempCalcKindDto dto);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    void Accept(UpdateStatusTempCalcKindDto dTo, bool isLoged = false);
    ValueTask<string> PostToIMZOAndSentUrl(TempCalcKind contract);
    ValueTask<string?> SendUrl(long contractId);
    TempCalcKindDto Get(long id);
    void Cancel(UpdateStatusTempCalcKindDto dTo);
    Task<byte[]> DownloadPdf(Guid id2, string? lang);
    byte[]? GetWordTemplate();
    object UploadFiles(StorageFile[] dto);
}
