using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.LevelCodeServices
{
    public interface ILevelCodeService : IStatusGeneric
    {
        PagedResult<LevelCodeListDto> GetList(SortFilterPageOptions dto);
        LevelCodeDto Get();
        LevelCodeDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateLevelCodeDlDto dto);
        void Update(UpdateLevelCodeDlDto dto);
        void Delete(int id);
    }
}
