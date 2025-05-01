using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface IScientificDegreeService : IStatusGeneric
{
    PagedResult<ScientificDegreeListDto> GetList(SortFilterPageOptions dto);
    ScientificDegreeDto Get();
    ScientificDegreeDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateScientificDegreeDlDto dto);
    void Update(UpdateScientificDegreeDlDto dto);
    void Delete(int id);
}
