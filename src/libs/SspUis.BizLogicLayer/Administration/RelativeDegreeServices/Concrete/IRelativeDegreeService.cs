using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.RelativeDegreeServices
{
    public interface IRelativeDegreeService : IStatusGeneric
    {
        PagedResult<RelativeDegreeListDto> GetList(SortFilterPageOptions dto);
        RelativeDegreeDto Get();
        RelativeDegreeDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateRelativeDegreeDlDto dto);
        void Update(UpdateRelativeDegreeDlDto dto);
        void Delete(int id);
    }
}
