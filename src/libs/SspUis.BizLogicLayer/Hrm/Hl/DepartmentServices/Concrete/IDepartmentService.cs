using Microsoft.EntityFrameworkCore.Storage;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;
using WEBASE;

namespace SspUis.BizLogicLayer.Hrm;

public interface IDepartmentService : IStatusGeneric
{
    PagedResult<DepartmentListDto> GetList(TableSortFilterPageOptions dto);
    DepartmentDto Get();
    DepartmentDto Get(int id);
    SelectList<int> AsSelectList(int? organizationId = null);
    HaveId<int> Create(CreateDepartmentDlDto dto);
    void Update(UpdateDepartmentDlDto dto);
    void Delete(int id);
    public void SyncEdocDepartment(IDbContextTransaction outTransaction = null);
}
