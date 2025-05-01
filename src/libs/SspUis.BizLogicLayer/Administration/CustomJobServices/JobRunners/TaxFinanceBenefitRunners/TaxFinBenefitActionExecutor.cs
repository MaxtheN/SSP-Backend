using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using StatusGeneric;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxFinBenefitRunners
{
    public interface ITaxFinBenefitActionExecutor : ICustomJobActionExecuter<TaxFinBenefitActionInputData>
    {
        
    }

    public class TaxFinBenefitActionExecutor : StatusGenericHandler, ITaxFinBenefitActionExecutor
    {
        private readonly IContractorService _contractorService;
        private readonly IUnitOfWork _unitOfWork;

        public TaxFinBenefitActionExecutor(IContractorService contractorService, IUnitOfWork unitOfWork)
        {
            _contractorService = contractorService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, TaxFinBenefitActionInputData actionInputData)
        {
            int year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);
            int greaterPeriod = year == DateTime.Today.Year ? AsPeriod(DateTime.Today.Month) : 4;
            bool isHaveAnyData = false;
            var result = new CustomJobActionExecuteResult
            {
                FromCache = !job.IsForceUpdate
            };

            for (int period = greaterPeriod; period >= 1; period--)
            {
                var entity = GetEntity(actionInputData.Tin, year, period);
                bool isGetFromApi = false;

                if (entity == null)
                {
                    isGetFromApi = true;
                    result.FromCache = false;
                    entity = new FinBenefit
                    {
                        Tin = actionInputData.Tin,
                        Year = year,
                        Period = period
                    };
                    _unitOfWork.Context.Add(entity);
                }
                else if (job.IsForceUpdate)
                    isGetFromApi = true;

                if (isGetFromApi)
                {
                    var dataFromIntegration = await _contractorService.GetFinanceBenefitFromSoliq(actionInputData.Tin, year, period);
                    if (dataFromIntegration == null)
                    {
                        _unitOfWork.Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                        continue;
                    }

                    isHaveAnyData = true;
                    entity.Name = dataFromIntegration.Name;
                    entity.NetIncome = dataFromIntegration.NetIncome.Value;
                    _unitOfWork.Save();
                }
            }

            if (!result.FromCache && !isHaveAnyData)
                AddError("Soliqdan ma'lumot topilmadi!");

            return result;

        }

        private FinBenefit GetEntity(string tin, int year, int period)
        {
            return _unitOfWork.Context.FinBenefits
                .FirstOrDefault(a => a.Tin == tin && a.Year == year && a.Period == period);
        }
        private int AsPeriod(int month)
        {
          
            if(month>0 && month <= 3)
            {
                return 1;
            }
            if (month > 3 && month <= 6)
            {
                return 2;
            }
            if (month > 6 && month <= 9)
            {
                return 3;
            }
            if (month > 9 && month <= 12)
            {
                return 4;
            }
            else
            {
                return 0;
            }


        }

    }
}
