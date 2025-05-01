using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners
{
    public interface ICustomJobActionInputDataSource<TActionInputData> : IStatusGenericHandler
    {
        Task<TActionInputData[]> GetSource(CustomJob entity);
    }
}
