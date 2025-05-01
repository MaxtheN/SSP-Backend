using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer;

public class CreateEmployeeDto : CreateEmployeeDlDto
{
    public CreatePersonDlDto Person { get; set; } = new();
}
public class CreateEmployeeAndUserDto : CreateEmployeeDto
{
    public CreateEployeeUserDto User { get; set; }
}

public class UpdateEmployeeAndUserDto : UpdateEmployeeDlDto
{
    public UpdateEployeeUserDto User { get; set; }
}

public class CreateEployeeUserDto : ILinkToEntity<User>
{
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string UserName { get; set; }
    [LocalizedRequired]
    public string Password { get; set; }
    public List<int> Roles { get; set; } = new List<int>();
}

public class UpdateEployeeUserDto : ILinkToEntity<User>
{
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string UserName { get; set; }
    public string Password { get; set; }
    public List<int> Roles { get; set; } = new List<int>();
}

public class CreateUserEmployeeOnUpdateDto : CreateEployeeUserDto
{
    public int EmployeeId { get; set; }
}

public class UpdateEployeeUserDtoConfig : PerDtoConfig<UpdateEployeeUserDto, User>
{
    public override Action<IMappingExpression<User, UpdateEployeeUserDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Roles, x => x.MapFrom(ent => ent.UserRoles.Where(a => a.StateId == StateIdConst.ACTIVE).Select(a => a.RoleId)))
            .ForMember(x => x.Password, x => x.Ignore());
}
