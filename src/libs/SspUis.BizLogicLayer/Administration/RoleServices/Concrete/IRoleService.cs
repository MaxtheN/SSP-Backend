using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.RoleServices
{
    public interface IRoleService : IStatusGeneric
    {
        PagedResult<RoleListDto> GetList(SortFilterPageOptions dto);
        RoleDto Get();
        RoleDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateRoleDlDto dto);
        void Update(UpdateRoleDlDto dto);
        void Delete(int id);
    }
}
