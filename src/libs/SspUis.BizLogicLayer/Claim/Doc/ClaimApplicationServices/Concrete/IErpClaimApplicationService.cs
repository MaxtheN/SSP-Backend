using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public interface IErpClaimApplicationService : IBaseApplicationService
            <long,
            ClaimApplication,
            ClaimApplicationListDto,
            ClaimApplicationDto,
            CreateClaimApplicationDlDto,
            UpdateClaimApplicationDlDto,
            ClaimApplicationSortFilterOptions>
    {
        Task Send(SendStatusClaimApplicationDto dto);
        void Revoke(RevokeStatusClaimApplicationDto dto);
        ClaimApplicationIntegrationForGetDto GetForFiles(long mediationId);
        ClaimApplicationDto Get(Guid id2);
        bool CanCreate(string inn = null);
        Task<HaveId<long>> CreateClaimApplication(CreateClaimApplicationDlDto dto);
        void Reject(RejectStatusClaimApplicationDto dto);
        void Cancel(CancelStatusClaimApplicationDto dto);
        void Accept(AcceptStatusClaimApplicationDto dto);
        IEnumerable<ClaimApplicationFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        void DeleteFile(Guid fileId);
        public ValueTask<byte[]> DownloadPdf(Guid id2, string? lang);
        List<DocNumbersByClaimAppTypeDto> GetDataDocNumbersByClaimAppTypeId(ByClaimAppTypeFilter dto);
        void EmployeeAttachment(UpdateEmployeeAttechmentDlDto dto);
        ClaimAppApplicationForCourtMediationDto GetForInfo(long prevAppId, int stepId);
        bool UpdateClaimApplicationTable(long id, string newAddress);
    }
}