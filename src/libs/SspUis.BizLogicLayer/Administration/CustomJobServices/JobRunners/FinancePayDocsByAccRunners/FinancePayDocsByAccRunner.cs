

using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FundTadbirkorRunners;
using SspUis.DataLayer;
using System;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FinancePayDocsByAccRunners;

public interface IFinancePayDocsByAccJobRunner : ICustomJobRunner
{

}

public class FinancePayDocsByAccJobRunner :
    CustomJobRunner<FinancePayDocsByAccActionInputData, IFinancePayDocsByAccActionInputDataSource, IFinancePayDocsByAccActionExecutor>,
    IFinancePayDocsByAccJobRunner
{
    public FinancePayDocsByAccJobRunner(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override int JobTypeId => CustomJobTypeIdConst.FinancePayDocsByAcc;
}
