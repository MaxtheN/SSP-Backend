using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.SpecialtyServices
{
    public interface ISpecialtyService : IStatusGeneric
    {
        PagedResult<SpecialtyListDto> GetList(SortFilterPageOptions dto);
        SpecialtyDto Get();
        SpecialtyDto Get(int id);
        SelectList<int> AsSelectList(int? instituteId = null);
        HaveId<int> Create(CreateSpecialtyDlDto dto);
        void Update(UpdateSpecialtyDlDto dto);
        void Delete(int id);
    }
}
