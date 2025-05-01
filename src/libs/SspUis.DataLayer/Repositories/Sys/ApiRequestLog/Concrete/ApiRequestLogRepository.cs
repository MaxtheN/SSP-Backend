using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ApiRequestLogRepository : BaseEntityRepository<Guid, ApiRequestLog, CreateApiRequestLogDlDto, UpdateApiRequestLogDlDto>, IApiRequestLogRepository
    {
        public ApiRequestLogRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }
    }
}
