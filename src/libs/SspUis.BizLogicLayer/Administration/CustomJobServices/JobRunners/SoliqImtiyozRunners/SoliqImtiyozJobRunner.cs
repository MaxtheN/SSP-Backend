


using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.InvestmentByInnRunners;
using SspUis.DataLayer;
using System;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.SoliqImtiyozRunners;



public interface ISoliqImtiyozJobRunner : ICustomJobRunner
{

}

public class SoliqImtiyozJobRunner :
    CustomJobRunner<SoliqImtiyozActionInputData, ISoliqImtiyozActionInputDataSource, ISoliqImtiyozActionExecutor>,
    IInvestmentByInnJobRunner
{
    public SoliqImtiyozJobRunner(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override int JobTypeId => CustomJobTypeIdConst.SoliqImtiyoz;
}