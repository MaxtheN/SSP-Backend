using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationResultFileDlDto : EntityDto<
	ArbitrationResultFileDlDto,
	ArbitrationResultFile>, IHaveIdProp<Guid>
{
	[LocalizedRequired]
	public Guid Id { get; set; }
	public string FileName { get; set; }
	public string FileExtension { get; set; }
}
