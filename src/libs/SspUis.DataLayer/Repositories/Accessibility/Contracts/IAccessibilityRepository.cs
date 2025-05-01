using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IAccessibilityRepository : IBaseEntityRepository<int, Accessibility, CreateAccessibilityDlDto, UpdateAccessibilityDlDto>
    {
    }
}