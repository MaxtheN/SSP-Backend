using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;
public class UpdateUniteOfMeasureDlDto : UniteOfMeasureDlDto<UpdateUniteOfMeasureDlDto>
	, IHaveIdProp<int>
{
	[LocalizedRequired]
	[LocalizedRange(1, int.MaxValue)]
	public int Id { get; set; }
	[LocalizedRequired]
	[LocalizedRange(1, int.MaxValue)]
	public int StateId { get; set; }
}

