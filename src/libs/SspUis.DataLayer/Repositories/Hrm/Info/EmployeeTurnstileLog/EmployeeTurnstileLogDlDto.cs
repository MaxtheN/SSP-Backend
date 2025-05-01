using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class EmployeeTurnstileLogDlDto<TDto> : EntityDto<TDto, EmployeeTurnstileLog>
    where TDto : EmployeeTurnstileLogDlDto<TDto>
{
    public int EmployeeId { get; set; }
    public int OrganizationId { get; set; } 
    public DateTime EventAt { get; set; }
    public DateOnly EventOn { get; set; }

    public override EmployeeTurnstileLog CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.Id = Guid.NewGuid();
        return entity;
    }
}
