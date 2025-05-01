using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.CitizenshipServices
{
    public interface ICitizenshipService : IStatusGeneric
    {
        PagedResult<CitizenshipListDto> GetList(SortFilterPageOptions dto);
        CitizenshipDto Get();
        CitizenshipDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateCitizenshipDlDto dto);
        void Update(UpdateCitizenshipDlDto dto);
        void Delete(int id);
    }
}
