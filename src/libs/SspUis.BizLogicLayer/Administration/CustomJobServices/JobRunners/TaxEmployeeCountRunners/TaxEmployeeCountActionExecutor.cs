using DocumentFormat.OpenXml.Bibliography;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxEmployeeCountRunners
{
    public interface ITaxEmployeeCountActionExecutor : ICustomJobActionExecuter<TaxEmployeeCountActionInputData>
    {
        
    }

    public class TaxEmployeeCountActionExecutor : StatusGenericHandler, ITaxEmployeeCountActionExecutor
    {
        private readonly IContractorService _contractorService;
        private readonly IUnitOfWork _unitOfWork;

        public TaxEmployeeCountActionExecutor(IContractorService contractorService, IUnitOfWork unitOfWork)
        {
            _contractorService = contractorService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, TaxEmployeeCountActionInputData actionInputData)
        {
            int year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);
            int greaterMonth = year == DateTime.Today.Year ? DateTime.Today.Month : 12;
            bool isHaveAnyData = false;
            var result = new CustomJobActionExecuteResult
            {
                FromCache = !job.IsForceUpdate
            };

            for (int month = greaterMonth; month >= 1; month--)
            {
                var entity = GetEntity(actionInputData.Tin, year, month);
                bool isGetFromApi = false;

                if (entity == null)
                {
                    isGetFromApi = true;
                    result.FromCache = false;
                    entity = new EmployeeCount
                    {
                        Tin = actionInputData.Tin,
                        Year = year,
                        Month = month
                    };
                    _unitOfWork.Context.Add(entity);
                }
                else if (job.IsForceUpdate)
                    isGetFromApi = true;

                if (isGetFromApi)
                {
                    var dataFromIntegration = await _contractorService.GetEmployeeCountFromSoliq(actionInputData.Tin, year, month);
                    if (dataFromIntegration == null)
                    {
                        _unitOfWork.Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                        continue;
                    }

                    isHaveAnyData = true;
                    entity.MonthlyNumberEmployees = dataFromIntegration.MonthlyNumberEmployees;
                    entity.PaymentTax = dataFromIntegration.PaymentTax;
                    _unitOfWork.Save();
                }
            }

            if (!result.FromCache && !isHaveAnyData)
                AddError("Soliqdan ma'lumot topilmadi!");

            return result;
        }

        private EmployeeCount GetEntity(string tin, int year, int month)
        {
            return _unitOfWork.Context.EmployeeCounts
                .FirstOrDefault(a => a.Tin == tin && a.Year == year && a.Month == month);
        }

    }
}
