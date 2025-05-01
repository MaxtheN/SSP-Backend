using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class CallCenterAppealFileDto : CallCenterAppealFileDlDto, ILinkToEntity<CallCenterAppealFile>
{
    public string FileName { get; internal set; }
    public DateTime CreatedAt { get; set; }
}
