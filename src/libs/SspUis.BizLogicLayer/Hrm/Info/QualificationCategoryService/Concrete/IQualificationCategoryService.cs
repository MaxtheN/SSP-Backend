using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.QualificationCategoryServices
{
    public interface IQualificationCategoryService : IStatusGeneric
    {
        PagedResult<QualificationCategoryListDto> GetList(SortFilterPageOptions dto);
        QualificationCategoryDto Get();
        QualificationCategoryDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateQualificationCategoryDlDto dto);
        void Update(UpdateQualificationCategoryDlDto dto);
        void Delete(int id);
    }
}
