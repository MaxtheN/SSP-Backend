using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.AppealTypeArriveServices;

public class AppealTypeArriveDto : UpdateAppealTypeArriveDlDto, ILinkToEntity<AppealTypeArrive>, IInfoHl
{
    public string State { get; set; } = null!;
    public new List<AppealTypeArriveTranslateDto> Translates { get; set; } = new();
}
