using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;

public class ChastisementFileDto :
    ChastisementFileDlDto,ILinkToEntity<ChastisementFile>
{
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }
}