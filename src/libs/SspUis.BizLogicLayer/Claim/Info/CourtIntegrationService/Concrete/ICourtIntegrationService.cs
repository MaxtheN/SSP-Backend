using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim.Info.CourtIntegrationService
{
    public interface ICourtIntegrationService : IStatusGeneric
    {
        HaveId<int> Create(CreateCourtntegrationDlDto dto);
    }
}
