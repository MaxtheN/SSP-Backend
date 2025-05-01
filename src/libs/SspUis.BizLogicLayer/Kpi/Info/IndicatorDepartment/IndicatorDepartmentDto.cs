using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer;

public class IndicatorDepartmentDto : UpdateIndicatorDepartmentDlDto, ILinkToEntity<IndicatorDepartment>
{
    public string State { get; set; } = null;
    public List<IndicatorDepartmentTranslateDto> Translates{ get; set; } = new();
}
