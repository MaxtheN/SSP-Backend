using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public interface IEmployeeSendStudyRepository : IBaseEntityRepository<long, EmployeeSendStudy, CreateEmployeeSendStudyDlDto, UpdateEmployeeSendStudyDlDto,UpdateStatusEmployeeSendStudyDlDto>
{
}
