using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface IDegreeTitleService : IStatusGeneric
{
    PagedResult<DegreeTitleListDto> GetList(SortFilterPageOptions dto);
    DegreeTitleDto Get();
    DegreeTitleDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateDegreeTitleDlDto dto);
    void Update(UpdateDegreeTitleDlDto dto);
    void Delete(int id);
}
