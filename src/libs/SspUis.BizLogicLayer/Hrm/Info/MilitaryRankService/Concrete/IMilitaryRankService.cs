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

public interface IMilitaryRankService : IStatusGeneric
{
    PagedResult<MilitaryRankListDto> GetList(SortFilterPageOptions dto);
    MilitaryRankDto Get();
    MilitaryRankDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateMilitaryRankDlDto dto);
    void Update(UpdateMilitaryRankDlDto dto);
    void Delete(int id);
}
