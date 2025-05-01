using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface ISubsidyRequestService
    : IBaseEntityService<
        long,
        SubsidyRequest,
        SubsidyRequestListDto,
        SubsidyRequestDto,
        CreateSubsidyRequestDlDto,
        UpdateSubsidyRequestDlDto,
        SubsidyRequestSortFilterOption>
{
    void Accept(UpdateStatusSubsidyRequestDlDto dto);
    void Cancel(UpdateStatusSubsidyRequestDlDto dto);
    Task<HaveId<long>> Create(CreateSubsidyRequestDlDto dto);
    void DeleteFile(Guid fileId);
    int GetCount();
    StorageFile DownloadFile(Guid fileId);
    void Reject(UpdateStatusSubsidyRequestDlDto dto);
    void Revoke(UpdateStatusSubsidyRequestDlDto dto);
    Task Send(SendStatusSubsidyRequestDto dto);
    Task Update(UpdateSubsidyRequestDlDto dto);
    IEnumerable<SubsidyRequestFileDto> UploadFiles(params StorageFile[] files);
}
