using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public interface IEmployeeLeaveOrderRepository
    :IBaseEntityRepository<long,EmployeeLeaveOrder,CreateEmployeeLeaveOrderDlDto,UpdateEmployeeLeaveOrderDlDto,UpdateStatusEmployeeLeaveOrderDlDto>
{
}
