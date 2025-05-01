using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public interface IClaimApplicationService : IBaseApplicationService
            <long,
            ClaimApplication,
            ClaimApplicationListDto,
            ClaimApplicationDto,
            CreateClaimApplicationDlDto,
            UpdateClaimApplicationDlDto,
            ClaimApplicationSortFilterOptions>
    {
        PagedResult<ClaimApplicationListDto> GetList(ClaimApplicationSortFilterOptions options);
        Task Send(SendStatusClaimApplicationDto dto);
        void Revoke(RevokeStatusClaimApplicationDto dto);
        ClaimApplicationDto Get(Guid id2);
        ClaimApplicationIntegrationForGetDto GetForIntegration(long id);
        bool CanCreate(string inn = null);
        Task<HaveId<long>> CreateClaimApplication(CreateClaimApplicationDlDto dto);
        void Reject(RejectStatusClaimApplicationDto dto);
        void Cancel(CancelStatusClaimApplicationDto dto);
        void Accept(AcceptStatusClaimApplicationDto dto);
        Task<ClaimApplicationIntegrationResponseDto> CreateFromIntegration(ClaimApplicationIntegrationRequestDto  dto);
        IEnumerable<ClaimApplicationFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        void DeleteFile(Guid fileId);
        public ValueTask<byte[]> DownloadPdf(Guid id2, string? lang);
        List<DocNumbersByClaimAppTypeDto> GetDataDocNumbersByClaimAppTypeId(ByClaimAppTypeFilter dto);
        void EmployeeAttachment(UpdateEmployeeAttechmentDlDto dto);
    }
}