using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
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

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FarmerRefundRunners
{
    public interface IFarmerRefundActionExecutor : ICustomJobActionExecuter<FarmerRefundJobRunnerActionInputData>
    {
        
    }

    public class FarmerRefundJobRunnerActionExecutor : StatusGenericHandler, IFarmerRefundActionExecutor
    {
        private readonly IContractorService _contractorService;
        private readonly IUnitOfWork _unitOfWork;

        public FarmerRefundJobRunnerActionExecutor(IContractorService contractorService, IUnitOfWork unitOfWork)
        {
            _contractorService = contractorService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, FarmerRefundJobRunnerActionInputData actionInputData)
        {
            int year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);
            bool isHaveAnyData = false;
            var result = new CustomJobActionExecuteResult
            {
                FromCache = !job.IsForceUpdate
            };
            var entity = GetEntity(actionInputData.Tin, year);
            bool isGetFromApi = false;

            if (entity == null)
            {
                isGetFromApi = true;
                result.FromCache = false;
                entity = new FarmerRefund
                {
                    Tin = actionInputData.Tin,
                    Year = year
                };
                _unitOfWork.Context.Add(entity);
            }
            else if (job.IsForceUpdate)
                isGetFromApi = true;

            if (isGetFromApi)
            {
                var dataFromIntegration = await _contractorService.GetFarmerRefundByInnFromSoliq(actionInputData.Tin, year);
                CombineStatuses(_contractorService);
                if (dataFromIntegration == null || HasErrors)
                {
                    _unitOfWork.Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
                else
                {
                    isHaveAnyData = true;
                    entity.Summa = dataFromIntegration.Summa;
                    entity.Year = year;
                    entity.Tin = actionInputData.Tin;
                    entity.ApplicationCount = dataFromIntegration.ApplicationCount;
                    _unitOfWork.Save();
                }
            }
            if (HasErrors)
                return null;

            if (!result.FromCache && !isHaveAnyData)
            {
                AddError("Soliqdan ma'lumot topilmadi!");
                return null;
            }
            return result;
        }

        private FarmerRefund GetEntity(string tin, int year)
        {
            return _unitOfWork.Context.FarmerRefunds
                .FirstOrDefault(a => a.Tin == tin && a.Year == year);
        }

    }
}
