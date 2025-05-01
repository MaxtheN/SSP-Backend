using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners
{
    public interface ICustomJobActionExecuter<TActionInputData> : IStatusGenericHandler
    {
        Task<CustomJobActionExecuteResult> Execute(CustomJob job, TActionInputData actionInputData);
    }
}
