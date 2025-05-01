using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FundTadbirkorRunners
{
    public interface IFundTadbirkorJobRunner : ICustomJobRunner
    {

    }

    public class FundTadbirkorJobRunner : 
        CustomJobRunner<FundTadbirkorActionInputData, IFundTadbirkorActionInputDataSource, IFundTadbirkorActionExecutor>, 
        IFundTadbirkorJobRunner
    {
        public FundTadbirkorJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.FundTadbirkor;
    }

}
