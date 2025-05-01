using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.BankCreditApplicationRunners
{
    public interface IBankCreditApplicationJobRunner : ICustomJobRunner
    {

    }

    public class BankCreditApplicationJobRunner : 
        CustomJobRunner<BankCreditApplicationJobRunnerActionInputData, IBankCreditApplicationActionInputDataSource, IBankCreditApplicationActionExecutor>, 
        IBankCreditApplicationJobRunner
    {
        public BankCreditApplicationJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.BankCreditApplication;
    }

}
