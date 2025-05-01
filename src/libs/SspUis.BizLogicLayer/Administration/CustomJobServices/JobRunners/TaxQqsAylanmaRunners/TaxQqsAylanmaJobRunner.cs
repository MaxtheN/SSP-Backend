using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxDebtRunners;
using SspUis.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxQqsAylanmaRunners;

public interface ITaxQqsAylanmaJobRunner : ICustomJobRunner
{
}
public class TaxQqsAylanmaJobRunner : CustomJobRunner<TaxQqsAylanmaActionInputData, ITaxQqsAylanmaActionInputDataSource, ITaxQqsAylanmaActionExecutor>,
        ITaxQqsAylanmaJobRunner
{
    public TaxQqsAylanmaJobRunner(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override int JobTypeId => CustomJobTypeIdConst.TaxQqsAylanma;
}
