using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IIndicatorDistrictRepository :
        IBaseEntityRepository<int, IndicatorDistrict,
            CreateIndicatorDistrictDlDto, UpdateIndicatorDistrictDlDto>
    {

    }
}
