using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.ImtiyozDataRunners
{
    public interface IImtiyozDataJobRunner : ICustomJobRunner
    {

    }

    public class BankCreditApplicationJobRunner : 
        CustomJobRunner<ImtiyozDataJobRunnerActionInputData, IImtiyozDataActionInputDataSource, IImtiyozDataActionExecutor>, 
        IImtiyozDataJobRunner
    {
        public BankCreditApplicationJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.ImtiyozData;
    }

}
