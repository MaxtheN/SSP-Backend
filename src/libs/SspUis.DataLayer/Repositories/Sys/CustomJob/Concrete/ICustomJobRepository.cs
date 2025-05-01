using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface ICustomJobRepository : IBaseEntityRepository<long, CustomJob, CreateCustomJobDlDto, UpdateCustomJobDlDto>
{
    //CustomJob UpdateStatus(UpdateStatusCustomJobDlDto dto);
    CustomJob UpdateStatus(UpdateStatusCustomJobDlDto dto);
}
