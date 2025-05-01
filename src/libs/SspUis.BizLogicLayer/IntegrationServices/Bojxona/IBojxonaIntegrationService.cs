using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.IntegrationServices.Bojxona
{
    public interface IBojxonaIntegrationService:IStatusGenericHandler
    {
        HaveId<int> CreateBojxonaImtiyoz(BojxonaImtiyozDto dto);
    }
}
