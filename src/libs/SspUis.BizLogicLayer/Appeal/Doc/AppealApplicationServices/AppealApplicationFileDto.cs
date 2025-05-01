using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Appeal;

public class AppealApplicationFileDto : AppealApplicationFileDlDto, ILinkToEntity<AppealApplicationFile>
{
    public string FileName { get; internal set; }
    public DateTime CreatedAt { get; set; }
}
