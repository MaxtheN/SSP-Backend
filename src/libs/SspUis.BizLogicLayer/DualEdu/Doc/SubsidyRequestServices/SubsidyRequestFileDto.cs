using System;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestFileDto : SubsidyRequestFileDlDto
{
    public string FileName { get; internal set; }
    public DateTime CreatedAt { get; set; }
}
