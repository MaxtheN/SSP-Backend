using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices
{
    public interface IEmployeeManageService : IStatusGeneric
    {
        PagedResult<EmployeeManageListDto> GetList(EmployeeManageSortFilterPageOptions dto);
        EmployeeManageDto Get();
        EmployeeCheckForSingDto CheckSigners();
        EmployeeManageDto Get(long id);
        //SelectList<long> AsSelectList(long? employeeManageId = null);
        HaveId<long> Create(CreateEmployeeManageDlDto dto);
        void Update(UpdateEmployeeManageDlDto dto);
        void Delete(long id);
    }
}
