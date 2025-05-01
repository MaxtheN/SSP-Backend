using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IEmployeeManageRepository : IBaseEntityRepository<long, EmployeeManage, CreateEmployeeManageDlDto, UpdateEmployeeManageDlDto>
    {
        EmployeeManage SetEnd(long id, DateOnly endOn, long endDocumentId, int endTableId);
        EmployeeManage SetIsDeleted(long id, bool isDeleted = true, Action<EmployeeManage> validation = null);
        EmployeeManage SetRate(long id, decimal empRate, long endDocumentId, int endTableId);
    }
}
