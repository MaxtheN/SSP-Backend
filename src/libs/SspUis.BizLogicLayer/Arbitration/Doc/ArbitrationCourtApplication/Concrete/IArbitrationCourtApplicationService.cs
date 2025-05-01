using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices
{
    public interface IArbitrationCourtApplicationService
        : IBaseApplicationService
            <long,
            ArbitrationCourtApplication,
            ArbitrationCourtApplicationListDto,
            ArbitrationCourtApplicationDto,
            CreateArbitrationCourtApplicationDlDto,
            UpdateArbitrationCourtApplicationDlDto,
            ArbitrationCourtApplicationSortFilterOptions>
    {
        void Accept(AcceptStatusArbitrationCourtApplicationDto dto);
        void Cancel(CancelStatusArbitrationCourtApplicationDto dto);
        void Reject(RejectStatusArbitrationCourtApplicationDto dto);
        int GetCount();
        Task Send(SendStatusArbitrationCourtApplicationDto dto);
        IEnumerable<ArbitrationCourtApplicationFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        void DeleteFile(Guid fileId);
        Task<ArbitrationCourtApplicationDto> Get();
        HaveId<long> Create(CreateArbitrationCourtApplicationDlDto dto);
        HaveId<long> Update(UpdateArbitrationCourtApplicationDlDto dto);
        HaveId<long> NextStep(long id);
        Task Sign(SignStatusArbitrationCourtApplicationDto dto);
        ArbitrationCourtApplicationDto GetByContractorId(long contractorId);
        Task<byte[]> DownloadTemplate(string? lang);
        void CourtDecisionStep(CourtDecisionStepArbitrationCourtApplication dto);
        void DelayedStep(DelayedStepArbitrationCourtApplication dto);
        void DiscussionStep(DiscussionStepArbitrationCourtApplication dto);
        ValueTask<string> PostToIMZOAndSentUrl(ArbitrationCourtApplication application);
        ValueTask<string?> WebImzoSign(WebImzoSignedFilter filter);
    }
}
