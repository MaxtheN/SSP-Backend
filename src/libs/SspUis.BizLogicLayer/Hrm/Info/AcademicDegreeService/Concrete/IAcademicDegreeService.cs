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

public interface IAcademicDegreeService : IStatusGeneric
{
    PagedResult<AcademicDegreeListDto> GetList(SortFilterPageOptions dto);
    AcademicDegreeDto Get();
    AcademicDegreeDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateAcademicDegreeDlDto dto);
    void Update(UpdateAcademicDegreeDlDto dto);
    void Delete(int id);
}
