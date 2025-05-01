using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.ItemOfExpenseServices
{
    public interface IItemOfExpenseService : IStatusGeneric
    {
        PagedResult<ItemOfExpenseListDto> GetList(SortFilterPageOptions dto);
        ItemOfExpenseDto Get();
        ItemOfExpenseDto Get(int id);
        SelectList<int> AsSelectList(int? workScheduleKindId = null);
        HaveId<int> Create(CreateItemOfExpenseDlDto dto);
        void Update(UpdateItemOfExpenseDlDto dto);
        void Delete(int id);
    }
}
