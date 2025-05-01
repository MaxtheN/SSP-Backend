using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface ICallCenterAppealService
    : IBaseEntityService<long, CallCenterAppeal, CallCenterAppealListDto, CallCenterAppealDto, CreateCallCenterAppealDlDto, UpdateCallCenterAppealDlDto, CallCenterAppealSortFilterOptions>
{
    CallCenterAppealDto Get();
    SelectList<long> AsSelectList(CallCenterAppealSortFilterOptions options);
    Task Accept(AcceptStatusCallCenterAppealDto dto);
    Task Reject(RejectStatusCallCenterAppealDto dto);
    //Task Sign(SignStatusCallCenterAppealDto dto);
    IEnumerable<CallCenterAppealFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
    Task<byte[]> DownloadPdf(long id, string land);
    Task SendToEdoc(long id);
    Stream PrinCallCenterAppealExcel(CallCenterAppealSortFilterOptions dto);
    CallCenterAppealDto Get(string docNumber);
    Task<byte[]> DownloadAttachment(Guid fileId, bool isView);
    Task Executed(ExecutedStatusCallCenterAppealDto dto);
    Task HasEdocResponce(HasEdocResponceStatusCallCenterAppealDto dto);
    Task Update(UpdateCallCenterAppealDlDto dto);
}
