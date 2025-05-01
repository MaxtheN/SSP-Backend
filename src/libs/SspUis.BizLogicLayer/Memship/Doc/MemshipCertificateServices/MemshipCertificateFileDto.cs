using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;

public class MemshipCertificateFileDto : MemshipCertificateFileDlDto/*, ILinkToEntity<MemshipCertificateFile>*/
{
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }
}