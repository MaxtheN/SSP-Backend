using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;
public interface IAppointEmployeeRepository : IBaseEntityRepository<long, AppointEmployee, CreateAppointEmployeeDlDto, UpdateAppointEmployeeDlDto, UpdateStatusAppointEmployeeDlDto>
{
}
