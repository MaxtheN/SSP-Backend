using System;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateEmployeeTurnstileLogDlDto : EmployeeTurnstileLogDlDto<UpdateEmployeeTurnstileLogDlDto>, IHaveIdProp<Guid>
{
    public Guid Id { get; set; }
}
