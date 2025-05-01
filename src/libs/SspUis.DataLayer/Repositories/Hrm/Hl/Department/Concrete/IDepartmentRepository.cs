using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IDepartmentRepository : IBaseEntityRepository<int, Department, CreateDepartmentDlDto, UpdateDepartmentDlDto>
{
}
