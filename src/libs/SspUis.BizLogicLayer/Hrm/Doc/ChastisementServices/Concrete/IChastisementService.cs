using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public interface IChastisementService
    : IBaseEntityService<long, Chastisement, ChastisementListDto, ChastisementDto, CreateChastisementDlDto, UpdateChastisementDlDto, ChastisementSortFilterOptions>
{
    //PagedResult<ChastisementListDto> GetListAll(ChastisementSortFilter dto);
    PagedResult<ChastisementListDto> GetList(ChastisementSortFilterOptions dto);
    PagedResult<ChastisementListDto> GetListForSigner(ChastisementSortFilterOptions dto);
    //PagedResult<UnpaidChastisementListDto> GetUnPaidList(UnpaidChastisementSortFilterPageOptions dto);
    ChastisementDto Get();
    ChastisementDto Get(long id);
    SelectList<long> AsSelectList(int? employeeId);
    //SelectList<long> GetTableAsSelectList(long ownerId, long? employeeId);
    ValueTask<HaveId<long>> Create(CreateChastisementDlDto dto);
    ValueTask<string> PostToIMZOAndSentUrl(Chastisement contract);
    ValueTask<string?> SendUrl(long contractId);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    void Accept(UpdateStatusChastisementDto dTo, bool isLoged = false);
    void Cancel(UpdateStatusChastisementDto dTo);
    void Update(UpdateChastisementDlDto dto);
    void Delete(long id);
    Task Sign(SignStatusChastisementDto dto);
    byte[]? GetWordTemplate();
    object UploadFiles(StorageFile[] dto);
    Task<byte[]> DownloadPdf(Guid id2);
    StorageFile DownloadByIdFile(Guid fileId);
    byte[] DownloadFile(Guid? id2, Guid? fileId);
}
