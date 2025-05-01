using System;
using System.IO;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public interface IDualApplicationService :
        IBaseApplicationService<long, DualApplication,
            DualApplicationListDto, DualApplicationDto,
            CreateDualApplicationDlDto, UpdateDualApplicationDlDto,
            DualApplicationSortFilterOptions>
    {
        void Accept(AcceptStatusDualApplicationDto dto);
        void Cancel(CancelStatusDualApplicationDto dto);
        void Revoke(RevokeStatusDualApplicationDto dto);
        bool CanCreate(string inn = null);
        DualApplicationDto Get(Guid id2);
        public Stream SaveExcelDualApplication(DualApplicationSortFilterOptions options);
        int GetCount();

		void Reject(RejectStatusDualApplicationDto dto);
        Task Send(SendStatusDualApplicationDto dto);
        Task SendToHierEdu(long id);
        Task<byte[]> DownloadPdf(Guid id2, string lang);
        ValueTask<string> PostToIMZOAndSentUrl(DualApplication contract);
        ValueTask<string?> WebImzoSign(WebImzoSignedFilter filter);
    }
}