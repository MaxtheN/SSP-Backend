using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IEmployeeTurnstileLogRepository
    : IBaseEntityRepository<Guid, EmployeeTurnstileLog, CreateEmployeeTurnstileLogDlDto, UpdateEmployeeTurnstileLogDlDto>
{
}
