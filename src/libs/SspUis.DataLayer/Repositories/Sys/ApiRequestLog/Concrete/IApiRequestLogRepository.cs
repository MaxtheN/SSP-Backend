using WEBASE.EF;
using WEBASE.Models;
using System;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public interface IApiRequestLogRepository : IBaseEntityRepository<Guid, ApiRequestLog, CreateApiRequestLogDlDto, UpdateApiRequestLogDlDto>
    {
    }
}
