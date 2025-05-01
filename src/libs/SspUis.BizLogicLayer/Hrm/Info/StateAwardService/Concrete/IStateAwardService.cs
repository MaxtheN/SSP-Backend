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

public interface IStateAwardService : IStatusGeneric
{
    PagedResult<StateAwardListDto> GetList(SortFilterPageOptions dto);
    StateAwardDto Get();
    StateAwardDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateStateAwardDlDto dto);
    void Update(UpdateStateAwardDlDto dto);
    void Delete(int id);
}
