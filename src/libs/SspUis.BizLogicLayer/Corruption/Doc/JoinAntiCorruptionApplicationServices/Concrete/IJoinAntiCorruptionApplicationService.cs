using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices;

public interface IJoinAntiCorruptionApplicationService : IBaseApplicationService
        <long,
        JoinAntiCorruptionApplication,
        JoinAntiCorruptionApplicationListDto,
        JoinAntiCorruptionApplicationDto,
        CreateJoinAntiCorruptionApplicationDlDto,
        UpdateJoinAntiCorruptionApplicationDlDto,
        JoinAntiCorruptionApplicationSortFilterOptions>
{
    void Accept(AcceptStatusJoinAntiCorruptionApplicationDto dto);
    void Cancel(CancelStatusJoinAntiCorruptionApplicationDto dto);
    void Revoke(RevokeStatusJoinAntiCorruptionApplicationDto dto);
    bool CanCreate(string inn = null);
    JoinAntiCorruptionApplicationDto Get(Guid id2);
    void Reject(RejectStatusJoinAntiCorruptionApplicationDto dto);
    Task Send(SendStatusJoinAntiCorruptionApplicationDto dto);
    IEnumerable<JoinAntiCorruptionApplicationFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
    ValueTask<byte[]> DownloadPdf(Guid id2, string lang);
}
