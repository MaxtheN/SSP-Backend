using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm.SettlementAccountSourceServices;

public class SettlementAccountSourceDto : UpdateSettlementAccountSourceDlDto, ILinkToEntity<SettlementAccountSource>
{
    public string State { get; set; }
    public string? Parent { get; set; }
    new public List<SettlementAccountSourceTranslateDto> Translates { get; set; } = new();
    new public List<SettlementAccountSourceDto> Children { get; set; } = new();
}
