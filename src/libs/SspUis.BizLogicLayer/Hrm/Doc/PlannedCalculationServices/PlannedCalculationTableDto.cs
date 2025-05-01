using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class PlannedCalculationTableDto : PlannedCalculationTableDlDto, ILinkToEntity<PlannedCalculationTable>
{
    public string Employee { get; set; }
    public string Position { get; set; }
    public string TempCalcKindType { get; set; }

}
