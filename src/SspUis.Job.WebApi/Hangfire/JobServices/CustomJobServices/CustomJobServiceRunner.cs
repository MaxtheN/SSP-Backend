using Hangfire;
using SspUis.Core;
using SspUis.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Job.WebApi.Hangfire.JobServices
{
    public interface ICustomJobServiceRunner
    {

        [Queue("ssp-custom-job-TaxEmployeeCount")]
        [AutomaticRetry(Attempts = 0)]
        Task TaxEmployeeCountExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);
        
        [Queue("ssp-custom-job-TaxFinBenefit")]
        [AutomaticRetry(Attempts = 0)]
        Task TaxFinBenefitExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);
        
        [Queue("ssp-custom-job-TaxDebt")]
        [AutomaticRetry(Attempts = 0)]
        Task TaxDebtExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-FundTadbirkor")]
        [AutomaticRetry(Attempts = 0)]
        Task FundTadbirkorExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-BojGTDByInn")]
        [AutomaticRetry(Attempts = 0)]
        Task BojGTDByInnExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-ImtiyozData")]
        [AutomaticRetry(Attempts = 0)]
        Task ImtiyozDataExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-FarmerRefund")]
        [AutomaticRetry(Attempts = 0)]
        Task FarmerRefundExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-BankCreditApplication")]
        [AutomaticRetry(Attempts = 0)]
        Task BankCreditApplicationExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-InvestmentByInn")]
        [AutomaticRetry(Attempts = 0)]
        Task InvestmentByInnExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-TaxQqsAylanma")]
        [AutomaticRetry(Attempts = 0)]
        Task TaxQqsAylanmaExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-FinancePayDocsByAcc")]
        [AutomaticRetry(Attempts = 0)]
        Task FinancePayDocsByAccExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);
        [Queue("ssp-custom-job-SoliqImtiyoz")]
        [AutomaticRetry(Attempts = 0)]
        Task SoliqImtiyozExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);

        [Queue("ssp-custom-job-queue")]
        [AutomaticRetry(Attempts = 0)]
        Task ExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken);
    }

    public class CustomJobServiceRunner : BaseHangfireAsyncJobServiceRunner<ICustomJobHangfireService, CustomJobParameter>, ICustomJobServiceRunner
    {
        public CustomJobServiceRunner(IServiceScopeAccessor serviceScopeAccessor, IServiceProvider serviceProvider) 
            : base(serviceScopeAccessor, serviceProvider)
        {

        }

        public async Task TaxEmployeeCountExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task TaxFinBenefitExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task TaxDebtExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task FundTadbirkorExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task BojGTDByInnExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task ImtiyozDataExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task FarmerRefundExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task BankCreditApplicationExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task InvestmentByInnExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task TaxQqsAylanmaExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            await ExecuteAsync(parameters, cancellationToken);
        }

        public async Task FinancePayDocsByAccExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
           
        }

        public async Task SoliqImtiyozExecuteAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        { 
            await ExecuteAsync(parameters, cancellationToken);
        }
    }   
}
