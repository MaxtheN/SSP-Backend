using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;
using WbImzo.Models;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface ISrvDeedService 
        : IBaseEntityService<long, ServiceDeed, SrvDeedListDto, SrvDeedDto, CreateServiceDeedDlDto, UpdateServiceDeedDlDto, SrvDeedSortFilterOption>
    {
        SrvContractForDeedDto GetBySrvContractId(long srvContractId);
        Task Signed(SignStatusSrvDeedDto dto);
        Task Signing(SigningStatusSrvDeedDto dto);
        int GetCount();
        ValueTask Cancel(CancelStatusSrvDeedDto dto);
        ValueTask Reject(RejectStatusSrvDeedDto dto);
		byte[] DownloadPdf(Guid id2, string? lang);
        public byte[] DownloadPdfOld(Guid id2, string? lang);

        Stream SaveAsExcel(SrvDeedSortFilterOption options);
        Guid SaveFile(long docId, string data, string fileName);
        HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null);
        ValueTask<ApiResult<bool>> UpdateSignRequestStateAsync(ServiceDeed contract);
        ValueTask<string> PostToIMZOAndSentUrl(ServiceDeed contract);
        ValueTask<(string? Url, bool Result)> WebImzoSign(WebImzoSignedFilter filter);
    }
}