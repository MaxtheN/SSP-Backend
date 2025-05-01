using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Salary.Core.Model;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public interface ICalculationKindService : IStatusGeneric
    {
        PagedResult<CalculationKindListDto> GetList(SortFilterPageOptions dto);
        CalculationKindDto Get();
        CalculationKindDto Get(int id);
        SelectList<int> AsSelectList(int? workScheduleKindId = null);
        HaveId<int> Create(CreateCalculationKindDlDto dto);
        void Update(UpdateCalculationKindDlDto dto);
        void Delete(int id);
        List<CalculationKindCore> GetAllCalculationKindCore();
        CalculationKindDto GetByMethod(int calculationMethodId);
    }
}
