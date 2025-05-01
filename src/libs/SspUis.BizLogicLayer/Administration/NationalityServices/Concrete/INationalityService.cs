using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.NationalityServices
{
    public interface INationalityService : IStatusGeneric
    {
        PagedResult<NationalityListDto> GetList(SortFilterPageOptions dto);
        NationalityDto Get();
        NationalityDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateNationalityDlDto dto);
        void Update(UpdateNationalityDlDto dto);
        void Delete(int id);
    }
}
