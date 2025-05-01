using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxFinBenefitRunners
{
    public interface ITaxFinBenefitJobRunner : ICustomJobRunner
    {

    }

    public class TaxFinBenefitJobRunner : 
        CustomJobRunner<TaxFinBenefitActionInputData, ITaxFinBenefitActionInputDataSource, ITaxFinBenefitActionExecutor>, 
        ITaxFinBenefitJobRunner
    {
        public TaxFinBenefitJobRunner(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override int JobTypeId => CustomJobTypeIdConst.TaxFinBenefit;
    }

}
