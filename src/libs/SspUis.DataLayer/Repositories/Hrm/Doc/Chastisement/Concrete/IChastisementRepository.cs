using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public interface IChastisementRepository
    :IBaseEntityRepository<long,Chastisement,CreateChastisementDlDto,UpdateChastisementDlDto,UpdateStatusChastisementDlDto>
{
}
