using SspUis.BizLogicLayer.ClaimApplicationServices;
using SspUis.BizLogicLayer.DualContractServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Repositories;
using System;
using System.Threading.Tasks;
using WbImzo.Models;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IDualContractService
        : IBaseEntityService<long, DualContract, DualContractListDto, DualContractDto, CreateDualContractDlDto, UpdateDualContractDlDto, DualContractDtoSortFilterOptions>
{
    PagedResult<DualContractListDto> GetList(DualContractDtoSortFilterOptions options);
    Task<byte[]> DownloadPdf(Guid id2);
    Task Sign(UpdateDualContract dto);
    void Reject(RejectStatusDualContractDto dto);
    Task<byte[]> DownloadPdf(Guid id2, string? lang);
    bool GetStatus(string pinfl);
    int GetCount();
    ValueTask<string> PostToIMZOAndSentUrl(DualContract contract);
    ValueTask<string?> WebImzoSign(WebImzoSignedFilter filter);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId);
    Guid SaveFile(long docId, string data, string fileName);
}