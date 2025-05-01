using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SspUis.DataLayer;
using SspUis.Job.WebApi.Hangfire.JobServices;

namespace SspUis.Job.WebApi.Controllers
{
    [ApiController]
    [Route("hangfire/customjob/[action]")]
    public class CustomJobController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(JobActionOutDto), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public string Execute(CustomJobParameter dto)
        {
            if (ModelState.IsValid)
            {

                string jobId = string.Empty;
                switch (dto.CustomJobId)
                {
                    case CustomJobTypeIdConst.TaxEmployeeCount:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.TaxEmployeeCountExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.TaxFinBenefit:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.TaxFinBenefitExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.TaxDebt:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.TaxDebtExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.FundTadbirkor:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.FundTadbirkorExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.BojGTDByInn:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.BojGTDByInnExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.ImtiyozData:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.ImtiyozDataExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.FarmerRefund:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.FarmerRefundExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.BankCreditApplication:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.BankCreditApplicationExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.InvestmentByInn:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.InvestmentByInnExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.TaxQqsAylanma:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.TaxQqsAylanmaExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.FinancePayDocsByAcc:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.FinancePayDocsByAccExecuteAsync(dto, CancellationToken.None));
                        break;
                    case CustomJobTypeIdConst.SoliqImtiyoz:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.SoliqImtiyozExecuteAsync(dto, CancellationToken.None));
                        break;
                    default:
                        jobId = BackgroundJob.Enqueue<ICustomJobServiceRunner>(service => service.ExecuteAsync(dto, CancellationToken.None));
                        break;
                }

                return jobId;
            }

            return null;
        }

        [HttpPost]
        public IActionResult Cancel(string jobId)
        {
            if (ModelState.IsValid)
            {
                BackgroundJob.Delete(jobId);
                return Ok();
            }
            return ValidationProblem(ModelState);
        }


        [HttpPost]
        public IActionResult Test1(string jobId)
        {
            if (ModelState.IsValid)
            {
                BackgroundJob.Delete(jobId);
                return Ok();
            }
            return ValidationProblem(ModelState);
        }

    }
}
