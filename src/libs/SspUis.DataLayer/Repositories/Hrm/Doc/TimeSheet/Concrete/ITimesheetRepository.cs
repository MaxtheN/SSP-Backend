using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public interface ITimesheetRepository : IBaseEntityRepository<long, Timesheet, CreateTimesheetDlDto, UpdateTimesheetDlDto, UpdateStatusTimesheetDlDto>
{
    TimesheetTableWithDaysDlDto UpdateTable(TimesheetTableWithDaysDlDto dto);
    Timesheet Fill(UpdateTimesheetDlDto dto, Action<Timesheet> validation = null);
    void ClearTables(long id);
}
