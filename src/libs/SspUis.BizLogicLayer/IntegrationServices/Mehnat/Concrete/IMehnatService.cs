using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusGeneric;

namespace SspUis.BizLogicLayer.IntegrationServices
{
    public interface IMehnatService : IStatusGenericHandler
    {
        void UpdateEmployeesWorkData();
    }
}
