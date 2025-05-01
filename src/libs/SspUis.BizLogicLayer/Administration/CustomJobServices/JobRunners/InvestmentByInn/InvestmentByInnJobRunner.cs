using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.InvestmentByInnRunners
{
    public interface IInvestmentByInnJobRunner : ICustomJobRunner
    {

    }

    public class InvestmentByInnJobRunner : 
        CustomJobRunner<InvestmentByInnJobRunnerActionInputData, IInvestmentByInnActionInputDataSource, IInvestmentByInnActionExecutor>, 
        IInvestmentByInnJobRunner
    {
        public InvestmentByInnJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.InvestmentByInn;
    }

}
