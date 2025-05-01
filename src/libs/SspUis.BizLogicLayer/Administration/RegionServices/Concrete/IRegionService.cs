using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.RegionServices
{
    public interface IRegionService : IStatusGeneric
    {
        PagedResult<RegionListDto> GetList(SortFilterPageOptions dto);
        RegionDto Get();
        RegionDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateRegionDlDto dto);
        void Update(UpdateRegionDlDto dto);
        void Delete(int id);
    }
}
