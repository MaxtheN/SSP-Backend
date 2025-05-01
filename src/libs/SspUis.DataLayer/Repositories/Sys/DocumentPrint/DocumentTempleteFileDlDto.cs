using System;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DocumentTempleteFileDlDto<TDto> : EntityDto<TDto, DocumentTempleteFile>
    where TDto : DocumentTempleteFileDlDto<TDto>
{
    [LocalizedRequired]
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public int TableId { get; set; }
    public int? LanguageId {  get; set; }
}
