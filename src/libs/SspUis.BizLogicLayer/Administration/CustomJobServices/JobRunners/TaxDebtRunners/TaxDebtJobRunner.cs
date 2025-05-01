using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxDebtRunners
{
    public interface ITaxDebtJobRunner : ICustomJobRunner
    {

    }

    public class TaxDebtJobRunner : 
        CustomJobRunner<TaxDebtJobRunnerActionInputData, ITaxDebtActionInputDataSource, ITaxDebtActionExecutor>, 
        ITaxDebtJobRunner
    {
        public TaxDebtJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.TaxDebt;
    }

}
