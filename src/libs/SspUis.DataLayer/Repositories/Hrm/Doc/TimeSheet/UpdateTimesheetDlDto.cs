using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateTimesheetDlDto : TimesheetDlDto<UpdateTimesheetDlDto> , IHaveIdProp<long>
{
    public long Id { get; set; }
    public List<TimesheetTableWithDaysDlDto> Tables { get; set; } = new();

    public override Timesheet CreateEntity()
    {
        var entity = base.CreateEntity();
        Tables.AddTo(entity.Tables);
        return entity;
    }

    public override void UpdateEntity(Timesheet entity)
    {
        base.UpdateEntity(entity);
        Tables.ApplyChangesByIsDeletedTo<long, TimesheetTableWithDaysDlDto, TimesheetTable>(entity.Tables);
    }

    public void UpdateEntityHead(Timesheet entity)
    {
        base.UpdateEntity(entity);
    }
}
