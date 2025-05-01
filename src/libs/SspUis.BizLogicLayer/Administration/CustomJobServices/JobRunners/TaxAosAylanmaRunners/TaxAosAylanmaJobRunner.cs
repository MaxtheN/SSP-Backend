using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxDebtRunners;
using SspUis.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxAosAylanmaRunners;

public interface ITaxAosAylanmaJobRunner : ICustomJobRunner
{
}
public class TaxAosAylanmaJobRunner : CustomJobRunner<TaxAosAylanmaActionInputData, ITaxAosAylanmaActionInputDataSource, ITaxAosAylanmaActionExecutor>,
        ITaxAosAylanmaJobRunner
{
    public TaxAosAylanmaJobRunner(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override int JobTypeId => CustomJobTypeIdConst.TaxAosAylanma;
}
