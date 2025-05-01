using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer;

public class IndicatorDto : UpdateIndicatorDlDto, ILinkToEntity<Indicator>
{
    public string State { get; set; } = null;
    public string Department { get; set; }
    public List<IndicatorTableDto> Tables { get; set; } = new();
    public List<IndicatorTranslateDto> Translates{ get; set; } = new();
}
