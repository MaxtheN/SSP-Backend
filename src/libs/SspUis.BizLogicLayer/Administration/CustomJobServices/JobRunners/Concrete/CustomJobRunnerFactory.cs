using Microsoft.Extensions.DependencyInjection;
using SspUis.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners
{
    public interface ICustomJobRunnerFactory
    {
        ICustomJobRunner GetJobRunner(int jobTypeId);
    }

    public class CustomJobRunnerFactory : ICustomJobRunnerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomJobRunnerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICustomJobRunner GetJobRunner(int jobTypeId)
        {
            switch (jobTypeId)
            {
                case CustomJobTypeIdConst.TaxEmployeeCount:
                    return GetRequiredService<TaxEmployeeCountRunners.ITaxEmployeeCountJobRunner>();
                case CustomJobTypeIdConst.TaxFinBenefit:
                    return GetRequiredService<TaxFinBenefitRunners.ITaxFinBenefitJobRunner>();
                case CustomJobTypeIdConst.FundTadbirkor:
                    return GetRequiredService<FundTadbirkorRunners.IFundTadbirkorJobRunner>();
                case CustomJobTypeIdConst.TaxDebt:
                    return GetRequiredService<TaxDebtRunners.ITaxDebtJobRunner>();
                case CustomJobTypeIdConst.BojGTDByInn:
                    return GetRequiredService<BojGTDByInnRunners.IBojGTDByInnJobRunner>();
                case CustomJobTypeIdConst.ImtiyozData:
                    return GetRequiredService<ImtiyozDataRunners.IImtiyozDataJobRunner>();
                case CustomJobTypeIdConst.FarmerRefund:
                    return GetRequiredService<FarmerRefundRunners.IFarmerRefundJobRunner>();
                case CustomJobTypeIdConst.BankCreditApplication:
                    return GetRequiredService<BankCreditApplicationRunners.IBankCreditApplicationJobRunner>();
                case CustomJobTypeIdConst.InvestmentByInn:
                    return GetRequiredService<InvestmentByInnRunners.IInvestmentByInnJobRunner>();
                case CustomJobTypeIdConst.TaxQqsAylanma:
                    return GetRequiredService<TaxQqsAylanmaRunners.ITaxQqsAylanmaJobRunner>();
                case CustomJobTypeIdConst.TaxAosAylanma:
                    return GetRequiredService<TaxAosAylanmaRunners.ITaxAosAylanmaJobRunner>();
                case CustomJobTypeIdConst.FinancePayDocsByAcc:
                    return GetRequiredService<FinancePayDocsByAccRunners.IFinancePayDocsByAccJobRunner>();
                default:
                    throw new NotImplementedException($"CustomJobRunner for jobTypeId = {jobTypeId} not implemented");
            }
        }

        private TService GetRequiredService<TService>() 
        {
            return _serviceProvider.GetRequiredService<TService>();
        }
    }
}
