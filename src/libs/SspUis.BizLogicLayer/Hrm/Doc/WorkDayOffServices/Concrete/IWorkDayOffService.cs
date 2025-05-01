using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface IWorkDayOffService
    : IBaseEntityService<long, WorkDayOff, WorkDayOffListDto, WorkDayOffDto, CreateWorkDayOffDlDto, UpdateWorkDayOffDlDto, WorkDayOffSortFilterOptions>
{
    ValueTask<HaveId<long>> Create(CreateWorkDayOffDlDto dto);
    ValueTask<string> PostToIMZOAndSentUrl(WorkDayOff contract);
    ValueTask<string?> SendUrl(long contractId);
    void Accept(UpdateStatusWorkDayOffDto dTo, bool isLoged = false);
    PagedResult<WorkDayOffListDto> GetList(WorkDayOffSortFilterOptions dto);
    WorkDayOffDto Get();
    SelectList<long> AsSelectList(WorkDayOffSortFilterOptions options);
    Task Sign(SignStatusWorkDayOffDto dto);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    Guid SaveFile(long docId, string data, string fileName);
    void Cancel(UpdateStatusWorkDayOffDto dTo);
}
