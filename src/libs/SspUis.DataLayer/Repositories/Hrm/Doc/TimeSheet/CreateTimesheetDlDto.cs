using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class CreateTimesheetDlDto : TimesheetDlDto<CreateTimesheetDlDto>
{
    public List<TimesheetTableWithDaysDlDto> Tables { get; set; } = new();

    public override Timesheet CreateEntity()
    {
        var entity = base.CreateEntity();
        Tables.AddTo(entity.Tables, (e, d) => d.TableDays.AddTo(e.TableDays));
        return entity;
    }
}
