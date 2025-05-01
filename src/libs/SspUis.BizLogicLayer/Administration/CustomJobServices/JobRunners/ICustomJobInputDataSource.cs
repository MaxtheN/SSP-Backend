using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners
{
    public interface ICustomJobInputDataSource<TActionInputData> : IStatusGenericHandler
    {
        Task<CustomJobActionExecuteResult> Execute(TActionInputData item);
    }
}
