using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.SettlementAccountSourceServices
{
    public interface ISettlementAccountSourceService : IStatusGeneric
    {
        PagedResult<SettlementAccountSourceListDto> GetList(SortFilterPageOptions dto);
        SettlementAccountSourceDto Get();
        SettlementAccountSourceDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateSettlementAccountSourceDlDto dto);
        void Update(UpdateSettlementAccountSourceDlDto dto);
        void Delete(int id);
    }
}
