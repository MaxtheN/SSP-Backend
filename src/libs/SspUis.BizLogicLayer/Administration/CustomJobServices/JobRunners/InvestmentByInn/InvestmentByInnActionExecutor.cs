using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Investment;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.InvestmentByInnRunners
{
    public interface IInvestmentByInnActionExecutor : ICustomJobActionExecuter<InvestmentByInnJobRunnerActionInputData>
    {
        
    }

    public class InvestmentByInnJobRunnerActionExecutor : StatusGenericHandler, IInvestmentByInnActionExecutor
    {
        private readonly IContractorService _contractorService;
        private readonly IUnitOfWork _unitOfWork;

        public InvestmentByInnJobRunnerActionExecutor(IContractorService contractorService, IUnitOfWork unitOfWork)
        {
            _contractorService = contractorService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, InvestmentByInnJobRunnerActionInputData actionInputData)
        {
            bool isHaveAnyData = false;
            var result = new CustomJobActionExecuteResult
            {
                FromCache = !job.IsForceUpdate
            };
            var entities = GetEntities(actionInputData.Tin);
            bool isGetFromApi = false;

            if (entities.Count == 0 || entities == null)
            {
                isGetFromApi = true;
                result.FromCache = false;
                //entity = new Debt
                //{
                //    Tin = actionInputData.Tin,
                //    Year = year
                //};
                //_unitOfWork.Context.Add(entity);
            }
            else if (job.IsForceUpdate)
                isGetFromApi = true;

            if (isGetFromApi)
            {
                var dataFromIntegration = await _contractorService.GetInvestmentContracts(
                    new Integration.Investitsiya.Models.InvestitsiyaRequestDto() { ContractorUzInn = actionInputData.Tin });
                CombineStatuses(_contractorService);
                
                if (dataFromIntegration.Count() != 0 && dataFromIntegration != null && !HasErrors)
                {
                    isHaveAnyData = true;
                    _unitOfWork.Context.InvestmentByInns.RemoveRange(entities);
                   foreach (var data in dataFromIntegration) 
                   {
                        if (data != null)
                        {
                            var investEntity = new InvestmentByInn()
                            {
                                Tin = actionInputData.Tin,
                                Idn = data.Idn,

                                DocNo = data.DocNo,
                                DocDate = data.DocDate,

                                ContractorUzName = data.ContractorUzName,
                                ContractorForName = data.ContractorForName,
                                ContractorForCountryCode = data.ContractorForCountryCode,
                                ContractorCountry = data.ContractorCountry,

                                BankId = data.BankId,
                                BankName = data.BankName,

                                ContractStatus = data.CntrStatus,
                                ContractStatusName = data.CntrStatusName,

                                ContractType = data.CntrType,
                                ContractTypeName = data.CntrTypeName,

                                ContractSubject = data.CntrSubject,
                                ContractSubjectName = data.ContractSubject,

                                Amount1 = data.Amount1,
                                Amount2 = data.Amount2,
                            };
                            _unitOfWork.Context.InvestmentByInns.Add(investEntity);
                        }
                   }
                   _unitOfWork.Save();
                }
            }
            if (HasErrors)
                return null;

            if (!result.FromCache && !isHaveAnyData)
            {
                AddError("Investitsiyadan ma'lumot topilmadi!");
                return null;
            }
            return result;
        }

        private List<InvestmentByInn> GetEntities(string tin)
        {
            return _unitOfWork.Context.InvestmentByInns
                .Where(a => a.Tin == tin).ToList();
        }

    }
}
