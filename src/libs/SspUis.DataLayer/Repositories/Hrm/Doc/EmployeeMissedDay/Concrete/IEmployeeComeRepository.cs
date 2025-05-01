using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public interface IEmployeeMissedDayRepository : IBaseEntityRepository<long, EmployeeMissedDay, CreateEmployeeMissedDayDlDto, UpdateEmployeeMissedDayDlDto>
{
    EmployeeMissedDay Create(CreateEmployeeMissedDayDlDto createEmployeeMissedDayDlDto, int organizationId);
    EmployeeMissedDay UpdateStatus(UpdateStatusEmployeeMissedDayDlDto dto);
}
