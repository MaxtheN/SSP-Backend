using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IUniteOfMeasureRepository : IBaseEntityRepository<int, UniteOfMeasure, CreateUniteOfMeasureDlDto, UpdateUniteOfMeasureDlDto>
{
}


