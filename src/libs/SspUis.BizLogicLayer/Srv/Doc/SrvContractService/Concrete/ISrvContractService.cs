using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Threading.Tasks;
using WbImzo.Models;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface ISrvContractService 
        : IBaseEntityService<long, ServiceContract, SrvContractListDto, SrvContractDto, CreateServiceContractDlDto, UpdateServiceContractDlDto, SrvContractSortFilterOption>
    {
        SrvContractDto GetByApplicationId(long applicationId);
        ValueTask<HaveId<long>> Create(CreateServiceContractDlDto dto);
        int GetCount();
        Task Signed(SignStatusSrvContractDto dto);
        Task Signing(SigningStatusSrvContractDto dto);
        ValueTask Cancel(CancelStatusSrvContractDto dto);
        ValueTask Reject(RejectStatusSrvContractDto dto);
        byte[] DownloadPdf(Guid id2, string? lang);
        HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null);
        Guid SaveFile(long docId, string data, string fileName);
        ValueTask<string> PostToIMZOAndSentUrl(ServiceContract contract);
        ValueTask<(string? Url, bool Result)> WebImzoSign(WebImzoSignedFilter filter);
        ValueTask<ApiResult<bool>> UpdateSignRequestStateAsync(ServiceContract contract);
    }
}