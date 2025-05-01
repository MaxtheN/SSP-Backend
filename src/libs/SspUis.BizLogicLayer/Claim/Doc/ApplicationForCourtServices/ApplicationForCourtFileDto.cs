using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;

public class ApplicationForCourtFileDto : ApplicationForCourtFileDlDto
{
	public string FileName { get; internal set; }
	public DateTime CreatedAt { get; set; }
}
