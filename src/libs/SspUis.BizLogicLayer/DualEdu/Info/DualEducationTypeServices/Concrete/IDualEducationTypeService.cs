using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.DualEducationTypeServices
{
    public interface IDualEducationTypeService : IStatusGeneric
    {
        PagedResult<DualEducationTypeListDto> GetList(SortFilterPageOptions dto);
        DualEducationTypeDto Get();
        DualEducationTypeDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateDualEducationTypeDlDto dto);
        void Update(UpdateDualEducationTypeDlDto dto);
        void Delete(int id);
    }
}
