

using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IQualificationCategoryRepository : IBaseEntityRepository<int, QualificationCategory, CreateQualificationCategoryDlDto, UpdateQualificationCategoryDlDto>
    {
    }
}
