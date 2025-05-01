using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Repositories.Appeal;
using WbImzo.Models;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Appeal;

public interface IAppealApplicationService
    : IBaseEntityService<long, AppealApplication, AppealApplicationListDto, AppealApplicationDto, CreateAppealApplicationDlDto, UpdateAppealApplicationDlDto, AppealApplicationSortFilterOptions>
{
    AppealApplicationDto Get();
    long GetCount();
    int GetCountMy();
	SelectList<long> AsSelectList(AppealApplicationSortFilterOptions options);
    Task Accept(AcceptStatusAppealApplicationDto dto);
    Task Reject(RejectStatusAppealApplicationDto dto);
   // Task InExecution(InExecutionStatusAppealApplicationDto dto);
   // Task Executed(ExecutedStatusAppealApplicationDto dto);
    Task Sign(SignStatusAppealApplicationDto dto);
    IEnumerable<AppealApplicationFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
    Task<byte[]> DownloadPdf(long id, string land);
    Task SendToEdoc(long id, int organizationId, bool IsCreatedByChamber = false);
    Stream PrinAppealApplicationExcel(AppealApplicationSortFilterOptions dto);
    AppealApplicationDto Get(string docNumber);
    Task<byte[]> DownloadAttachment(Guid fileId, bool isView);
    Task Executed(ExecutedStatusAppealApplicationDto dto);
    Task<HaveId<long>> Create(CreateAppealApplicationDlDto dto);
    Task HasEdocResponce(HasEdocResponceStatusAppealApplicationDto dto);
    Task Update(UpdateAppealApplicationDlDto dto);
    ValueTask<string> PostToIMZOAndSentUrl(AppealApplication application);
    ValueTask<string?> WebImzoSign(WebImzoSignedFilter filter);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    Guid SaveFile(long docId, string data, string fileName);
}
