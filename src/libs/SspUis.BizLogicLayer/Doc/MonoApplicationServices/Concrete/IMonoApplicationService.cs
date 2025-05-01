using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    public interface IMonoApplicationService : IBaseApplicationService
            <long,
            MonoApplication,
            MonoApplicationListDto,
            MonoApplicationDto,
            CreateMonoApplicationDlDto,
            UpdateMonoApplicationDlDto,
            MonoApplicationSortFilterOptions>
    {
        Task Send(SendStatusMonoApplicationDto dto);
        PagedResult<MonoApplicationListDto> GetList(MonoApplicationSortFilterOptions options);
        int GetCount();
        MonoApplicationDto Get(Guid id2);
        bool CanCreate(string inn = null);
        Task<HaveId<long>> Create(CreateMonoApplicationDlDto dto);
        void Reject(RejectStatusMonoApplicationDto dto);
        void Cancel(CancelStatusMonoApplicationDto dto);
        void Accept(AcceptStatusMonoApplicationDto dto);
        void Test();
        IEnumerable<MonoApplicationFileDto> UploadFiles(params StorageFile[] files);
        public ValueTask<byte[]> DownloadPdf(Guid id2, string? lang);
        StorageFile DownloadFile(Guid fileId);
        Task SentForReview(long id, string userIp = null, string userAgent = null);
        void DeleteFile(Guid fileId);
    }
}