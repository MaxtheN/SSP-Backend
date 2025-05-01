using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IExecutionApplicationRepository
        : IBaseEntityRepository<long, ExecutionApplication, CreateExecutionApplicationDlDto, UpdateExecutionApplicationDlDto, UpdateStatusExecutionApplicationDlDto>
    {
    }
}
