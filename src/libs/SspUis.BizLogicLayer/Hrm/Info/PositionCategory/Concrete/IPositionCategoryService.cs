using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.PositionCategoryServices
{
    public interface IPositionCategoryService : IStatusGeneric
    {
        PagedResult<PositionCategoryListDto> GetList(SortFilterPageOptions dto);
        PositionCategoryDto Get();
        PositionCategoryDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreatePositionCategoryDlDto dto);
        void Update(UpdatePositionCategoryDlDto dto);
        void Delete(int id);
    }
}
