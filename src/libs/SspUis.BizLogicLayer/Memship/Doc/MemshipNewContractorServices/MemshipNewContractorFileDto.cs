
using SspUis.DataLayer.Repositories;
using System;


namespace SspUis.BizLogicLayer.Memship;

public class MemshipNewContractorFileDto : MemshipNewContractorFileDlDto
{
    public string FileName { get; internal set; }
    public DateTime CreatedAt { get; set; }
}
