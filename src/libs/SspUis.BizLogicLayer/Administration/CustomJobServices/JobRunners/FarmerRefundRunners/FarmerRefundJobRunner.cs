using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FarmerRefundRunners
{
    public interface IFarmerRefundJobRunner : ICustomJobRunner
    {

    }

    public class FarmerRefundJobRunner : 
        CustomJobRunner<FarmerRefundJobRunnerActionInputData, IFarmerRefundActionInputDataSource, IFarmerRefundActionExecutor>, 
        IFarmerRefundJobRunner
    {
        public FarmerRefundJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.FarmerRefund;
    }

}
