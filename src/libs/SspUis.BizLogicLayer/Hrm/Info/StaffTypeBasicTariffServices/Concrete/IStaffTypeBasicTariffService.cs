using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.StaffTypeBasicTariffServices
{
    public interface IStaffTypeBasicTariffService : IStatusGeneric
    {
        PagedResult<StaffTypeBasicTariffListDto> GetList(SortFilterPageOptions dto);
        StaffTypeBasicTariffDto Get();
        StaffTypeBasicTariffDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateStaffTypeBasicTariffDlDto dto);
        void Update(UpdateStaffTypeBasicTariffDlDto dto);
        void Delete(int id);
    }
}
