using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class UniteOfMeasureDto : UpdateUniteOfMeasureDlDto, ILinkToEntity<UniteOfMeasure>
{
	public string State { get; set; }
}
