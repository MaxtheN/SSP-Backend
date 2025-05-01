using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class EmployeeTurnstileLogRepository : BaseEntityRepository<Guid, EmployeeTurnstileLog, CreateEmployeeTurnstileLogDlDto, UpdateEmployeeTurnstileLogDlDto>,
    IEmployeeTurnstileLogRepository
{
    public EmployeeTurnstileLogRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }
}
