using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public interface IEmployeeSendStudyService
    : IBaseEntityService<long, EmployeeSendStudy, EmployeeSendStudyListDto, EmployeeSendStudyDto, CreateEmployeeSendStudyDlDto, UpdateEmployeeSendStudyDlDto,EmployeeSendStudySortFilterOptions>
{
    PagedResult<EmployeeSendStudyListDto> GetList(EmployeeSendStudySortFilterOptions dto);
    PagedResult<EmployeeSendStudyListDto> GetListForSigner(EmployeeSendStudySortFilterOptions dto);
    EmployeeSendStudyDto Get();
    EmployeeSendStudyDto Get(long id);
    EmployeeSendStudyTableDto GetByEmployeeId(int employeeId, DateOnly? startOn, DateOnly? endOn);
    SelectList<long> AsSelectList(int? employeeId = null);
    ValueTask<HaveId<long>> Create(CreateEmployeeSendStudyDlDto dto);
    ValueTask<string> PostToIMZOAndSentUrl(EmployeeSendStudy contract);
    ValueTask<string?> SendUrl(long contractId);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    void Accept(UpdateStatusEmployeeSendStudyDto dTo, bool isLoged = false);
    Task Sign(SignStatusEmployeeSendStudyDto dto);
    void Cancel(UpdateStatusEmployeeSendStudyDto dTo);
    void Update(UpdateEmployeeSendStudyDlDto dto);
    void Delete(long id);
    Task<byte[]> DownloadPdf(Guid id2);
    byte[]? GetWordTemplate();
    object UploadFiles(StorageFile[] dto);
}
