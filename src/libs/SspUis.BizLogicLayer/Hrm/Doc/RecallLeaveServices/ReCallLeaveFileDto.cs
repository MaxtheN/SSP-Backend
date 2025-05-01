using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;
public class ReCallLeaveFileDto :
    ReCallLeaveFileDlDto, ILinkToEntity<ReCallLeaveFile>
{
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }
}