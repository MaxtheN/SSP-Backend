using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.PartisanshipServices;

public class PartisanshipDto : UpdatePartisanshipDlDto, ILinkToEntity<Partisanship>
{
    public string State { get; internal set; }
    new public List<PartisanshipTranslateDto> Translates { get; set; } = new();
}
