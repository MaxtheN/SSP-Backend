using System.IO;
using SspUis.DataLayer.Repositories.Hrm;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface ITimesheetService : IStatusGeneric
{
    PagedResult<TimesheetListDto> GetList(TimesheetSortFilterOptions options);
    TimesheetDto Get();
    TimesheetDto Get(long id);
    TimesheetTableDto GetTable(long id);
    SelectList<long> AsSelectList(int? employeeId = null);
    HaveId<long> Create(CreateTimesheetDlDto dto);
    void Update(UpdateTimesheetDlDto dto);
    void Delete(long id);
    void Accept(UpdateStatusTimesheetDto dTo);
    void Cancel(UpdateStatusTimesheetDto dTo);
    TimesheetTableWithDaysDlDto UpdateTable(TimesheetTableWithDaysDlDto dto);
    HaveId<long> ClearTimeSheetTable(ClearTableDto dto);
    HaveId<long> FillTimeSheet(TimesheetFillDto dto);
    public Stream SaveAsExecelForTabel(long id);
}
