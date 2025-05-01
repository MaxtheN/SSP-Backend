using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using WbImzo.Models;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Memship;

public interface IMemshipContractService
    : IBaseEntityService<long, MemshipContract,
        MemshipContractListDto, 
        MemshipContractDto, 
        CreateMemshipContractDlDto,
        UpdateMemshipContractDlDto, 
        MemshipContractSortFilterOptions>
{
    PagedResult<MemshipContractListDto> GetList(MemshipContractSortFilterOptions options);
    int GetCount();
    MemshipContractDto Get();
    MemshipContractDto Get(long id);
    SelectList<long> AsSelectList();
    ValueTask<HaveId<long>> Create(CreateMemshipContractDlDto dto);
    //ValueTask<string> PostToIMZOAndSentSMS(MemshipContract contract);
    ValueTask<string?> PostToIMZOAndSentUrl(MemshipContract contract);
    ValueTask<(string? Url, bool Result)> WebImzoSign(SignWebImzoContractFilter filter);
    ValueTask<ApiResult<bool>> UpdateSignRequestStateAsync(MemshipContract contract);
    HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null);
    void Sign(SignStatusMemshipContractDto dTo);
    ValueTask Cancel(CancelStatusMemshipContractDto dto);
    //ValueTask Reject(RejectStatusMemshipContractDto dto);
    void Update(UpdateMemshipContractDlDto dto);
    HaveId<long> UpdateStatus(UpdateStatusMemshipContractDlDto dto, Action<MemshipContract> validation);
    void Delete(long id);
    Task<byte[]> GetWordTemplate(Guid id2);
    public byte[] DownloadFile(Guid? id2, Guid? fileId);
    object UploadFiles(StorageFile[] dto);
    public StorageFile DownloadByIdFile(Guid fileId);
    MemshipContractDto GetByApplicationId(int applicationId);
    Task Import(List<ImportMemshipContractDlDto> listDto);
    byte[] DownloadPdf(Guid id2, string? lang);
    byte[] DownloadPdf(MemshipContractDto model);
    Task<string> ImportJismoniy(List<ImportJismoniyMemshipContractDlDto> listDto);
    ValueTask ChangeContractorToPaid(ChangeContractToPayedDto dTo);
    void UpdateSecretKeyAndRequestId(long contractId);
    void ChangeContractorDocnumber(ChangeContractParametirsDto dTo);
    void Comfirm(long id);
    //ValueTask<byte[]> Print(long id);
    //ValueTask<byte[]> GenerateWord();
    Guid SaveFile(long docId, string data, string fileName);
    Stream SaveAsExcel(MemshipContractSortFilterOptions options);
    Task<string> ImportYuridik(List<ImportYuridikMemshipContractDlDto> listDto);
    void Update(UpdatingBirdMemshipContractDlDto dto);
}
