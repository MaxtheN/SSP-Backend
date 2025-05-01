using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class MemshipCertificateFileDlDto : EntityDto<MemshipCertificateFileDlDto, MemshipCertificateFile>, 
    IHaveIdProp<Guid>, 
    ILinkToEntity<MemshipCertificateFile>
{
    [LocalizedRequired]
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
}