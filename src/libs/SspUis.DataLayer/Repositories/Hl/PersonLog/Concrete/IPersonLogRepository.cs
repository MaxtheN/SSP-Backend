using SspUis.DataLayer.EfClasses.Public.Hl;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;


public interface IPersonLogRepository : IBaseEntityRepository<int, PersonLog, CreatePersonLogDlDto, UpdatePersonLogDlDto>
{

}
