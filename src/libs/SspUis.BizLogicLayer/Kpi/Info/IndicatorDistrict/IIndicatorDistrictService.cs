using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface IIndicatorDistrictService : IStatusGeneric
    {
        PagedResult<IndicatorDistrictListDto> GetList(SortFilterPageOptions option);
        IndicatorDistrictDto Get();
        IndicatorDistrictDto GetById(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateIndicatorDistrictDlDto dto);
        void Update(UpdateIndicatorDistrictDlDto dto);
        public void Delete(int id);
    }
}
