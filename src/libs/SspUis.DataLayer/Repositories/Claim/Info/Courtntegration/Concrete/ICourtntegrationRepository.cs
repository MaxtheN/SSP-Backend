using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface ICourtntegrationRepository : IBaseEntityRepository<int, Courtntegration, CreateCourtntegrationDlDto, UpdateCourtntegrationDlDto>
    {
    }
}
