using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.BojGTDByInnRunners
{
    public interface IBojGTDByInnJobRunner : ICustomJobRunner
    {

    }

    public class BojGTDByInnJobRunner : 
        CustomJobRunner<BojGTDByInnActionInputData, IBojGTDByInnActionInputDataSource, IBojGTDByInnActionExecutor>, 
        IBojGTDByInnJobRunner
    {
        public BojGTDByInnJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.BojGTDByInn;
    }

}
