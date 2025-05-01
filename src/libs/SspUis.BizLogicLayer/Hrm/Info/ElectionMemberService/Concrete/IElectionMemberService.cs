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

public interface IElectionMemberService : IStatusGeneric
{
    PagedResult<ElectionMemberListDto> GetList(SortFilterPageOptions dto);
    ElectionMemberDto Get();
    ElectionMemberDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateElectionMemberDlDto dto);
    void Update(UpdateElectionMemberDlDto dto);
    void Delete(int id);
}
