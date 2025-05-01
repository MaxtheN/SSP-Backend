using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.FixedMinimumValueServices
{
    public interface IFixedMinimumValueService : IStatusGeneric
    {
        PagedResult<FixedMinimumValueListDto> GetList(SortFilterPageOptions dto);
        FixedMinimumValueDto Get();
        FixedMinimumValueDto Get(long id);
        SelectList<long> AsSelectList();
        HaveId<long> Create(CreateFixedMinimumValueDlDto dto);
        void Update(UpdateFixedMinimumValueDlDto dto);
        void Delete(long id);
    }
}
