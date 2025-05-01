using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.InstituteServices
{
    public interface IInstituteService : IStatusGeneric
    {
        PagedResult<InstituteListDto> GetList(SortFilterPageOptions dto);
        InstituteDto Get();
        InstituteDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateInstituteDlDto dto);
        void Update(UpdateInstituteDlDto dto);
        void Delete(int id);
    }
}
