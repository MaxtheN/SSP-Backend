using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.AppealDescriptionServices;

public class AppealDescriptionDto : UpdateAppealDescriptionDlDto, ILinkToEntity<AppealDescription>, IInfoHl
{
    public string State { get; set; } = null!;
    public new List<AppealDescriptionTranslateDto> Translates { get; set; } = new();
}
