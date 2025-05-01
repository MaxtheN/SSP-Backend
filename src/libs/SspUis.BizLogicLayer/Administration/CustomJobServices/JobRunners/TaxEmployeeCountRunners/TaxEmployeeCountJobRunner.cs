using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxEmployeeCountRunners
{
    public interface ITaxEmployeeCountJobRunner : ICustomJobRunner
    {

    }

    public class TaxEmployeeCountJobRunner : 
        CustomJobRunner<TaxEmployeeCountActionInputData, ITaxEmployeeCountActionInputDataSource, ITaxEmployeeCountActionExecutor>, 
        ITaxEmployeeCountJobRunner
    {
        public TaxEmployeeCountJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.TaxEmployeeCount;
    }

}
