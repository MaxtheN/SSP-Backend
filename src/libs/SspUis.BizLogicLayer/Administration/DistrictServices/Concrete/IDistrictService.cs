using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DistrictServices
{
    public interface IDistrictService : IStatusGeneric
    {
        PagedResult<DistrictListDto> GetList(SortFilterPageOptions dto);
        DistrictDto Get();
        DistrictDto Get(int id);
        SelectList<int> AsSelectList(int? regionId);
        HaveId<int> Create(CreateDistrictDlDto dto);
        void Update(UpdateDistrictDlDto dto);
        void Delete(int id);
    }
}
