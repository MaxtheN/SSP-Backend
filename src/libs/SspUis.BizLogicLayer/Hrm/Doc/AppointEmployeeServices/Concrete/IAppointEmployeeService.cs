using System;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.Models;
using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using StatusGeneric;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm
{
    public interface IAppointEmployeeService : IBaseEntityService<long, AppointEmployee, AppointEmployeeListDto, AppointEmployeeDto, CreateAppointEmployeeDlDto, UpdateAppointEmployeeDlDto, AppointEmployeeSortFilterOptions>
    {
        PagedResult<AppointEmployeeListDto> GetList(AppointEmployeeSortFilterOptions dto);
        PagedResult<AppointEmployeeListDto> GetListForHeader(AppointEmployeeSortFilterOptions dto);
        PagedResult<AppointEmployeeListDto> GetListForSigner(AppointEmployeeSortFilterOptions dto);
        void CheckEmploymentRateBeforSave(int employeeId, DateTime startOn, decimal employeeRate, int? fromPositionId);
        AppointEmployeeDto Get();
        AppointEmployeeDto Get(long id);
        SelectList<long> AsSelectList(int? employeeId = null);
        ValueTask<HaveId<long>> Create(CreateAppointEmployeeDlDto dto);
        Task<CreateUserResponseModel> AcceptAsync(UpdateStatusAppointEmployeeDto dto, bool isLoged = false);
        HaveId<long> Cancel(UpdateStatusAppointEmployeeDto dto);
        Task Sign(SignStatusAppointEmployeeDto dto);
        Task SignUpdate(long id);
        ValueTask<string> PostToIMZOAndSentUrl(AppointEmployee contract);
        ValueTask<string?> SendUrl(long contractId);
        HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
        void Update(UpdateAppointEmployeeDlDto dto);
        void Delete(long id);
        byte[] GenerateWord();
        ValueTask<byte[]> Print(long id);
        Task<byte[]> DownloadPdf(Guid id2, string? lang);
        byte[]? GetWordTemplate();
        object UploadFiles(StorageFile[] dto);
    }
}
