using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class PlannedCalculationDto : UpdatePlannedCalculationDlDto, ILinkToEntity<PlannedCalculation>, IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }
    public int? DepartmentId { get; set; }
    public string CalculationKind { get; set; }
    public bool IsCancelation { get; set; }
    public string RoundingType { get; set; }
    new public List<PlannedCalculationTableDto> Tables { get; set; } = new();
}
