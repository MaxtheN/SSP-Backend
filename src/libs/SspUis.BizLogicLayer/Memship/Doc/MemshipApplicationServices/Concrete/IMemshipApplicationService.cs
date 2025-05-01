using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.MemshipApplicationServices
{
    public interface IMemshipApplicationService
        : IBaseApplicationService
            <long,
            MemshipApplication,
            MemshipApplicationListDto,
            MemshipApplicationDto,
            CreateMemshipApplicationDlDto,
            UpdateMemshipApplicationDlDto,
            MemshipApplicationSortFilterOptions>
    {
        ValueTask<(long contractorId, Guid contractorId2)> Accept(AcceptStatusMemshipApplicationDto dto);
        void Cancel(CancelStatusMemshipApplicationDto dto);
        bool CanCreate(string inn = null);
        MemshipApplicationDto Get(Guid id2);
        void Revoke(RevokeStatusMemshipApplicationDto dto);
        //void Reject(RejectStatusMemshipApplicationDto dto);
        Task Send(SendStatusMemshipApplicationDto dto);
        byte[] DownloadPdf(Guid id2, string lang);
        byte[] DownloadPdf(MemshipApplicationForPdf model);
        IEnumerable<MemshipApplicationFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        void DeleteFile(Guid fileId);
        ValueTask<MemshipApplicationDto> Get();
        int GetCount();
        ValueTask<MemshipApplicationDto> GetForErp(string inn);
        ValueTask<CreateMemshipApplicationDto> Create(CreateMemshipApplicationDlDto dto, CancellationToken token);
        ValueTask<CreateMemshipApplicationDto> CreateForErp(CreateMemshipApplicationDlDto dto);
        Stream SaveAsExcel(MemshipApplicationSortFilterOptions dto);
        Task<string> SummQqsOrAos(string innProp, int year, int n);
        Task<List<string>> MistakeSetPropsInContractorList(int type, int pageSize, int pageNumber);
        void AcceptForErp(AcceptStatusMemshipApplicationDto dto, out long contractId, out Guid contractId2);
    }
}
