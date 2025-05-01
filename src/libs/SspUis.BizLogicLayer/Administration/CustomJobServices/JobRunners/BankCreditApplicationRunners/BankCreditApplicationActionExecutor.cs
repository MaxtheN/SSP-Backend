using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.BankCredit;
using StatusGeneric;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.BankCreditApplicationRunners
{
    public interface IBankCreditApplicationActionExecutor : ICustomJobActionExecuter<BankCreditApplicationJobRunnerActionInputData>
    {
        
    }

    public class BankCreditApplicationJobRunnerActionExecutor : StatusGenericHandler, IBankCreditApplicationActionExecutor
    {
        private readonly IContractorService _contractorService;
        private readonly IUnitOfWork _unitOfWork;

        public BankCreditApplicationJobRunnerActionExecutor(IContractorService contractorService, IUnitOfWork unitOfWork)
        {
            _contractorService = contractorService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, BankCreditApplicationJobRunnerActionInputData actionInputData)
        {
            int year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);
            bool isHaveAnyData = false;
            var result = new CustomJobActionExecuteResult
            {
                FromCache = !job.IsForceUpdate
            };
            var entity = GetEntity(actionInputData.Tin);
            bool isGetFromApi = false;

            if (entity == null)
            {
                isGetFromApi = true;
                result.FromCache = false;
                entity = new BankCreditApplication
                {
                    Tin = actionInputData.Tin,
                };
                _unitOfWork.Context.Add(entity);
            }
            else if (job.IsForceUpdate)
                isGetFromApi = true;

            if (isGetFromApi)
            {
                var dataFromIntegration = await _contractorService.GetApplicationsFromBankCredit(actionInputData.Tin, 1);
                CombineStatuses(_contractorService);
                if (dataFromIntegration == null || HasErrors ||(dataFromIntegration.ContractorTin==0&& dataFromIntegration.ContractorName==null))
                {
                    _unitOfWork.Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
                else
                {
                    isHaveAnyData = true;
                    entity.Tin = actionInputData.Tin;
                    entity.BankName = dataFromIntegration.BankName;
                    entity.BankMfo = dataFromIntegration.BankMfo;
                    entity.Tables.Clear();
                    if (dataFromIntegration.Applications != null && dataFromIntegration.Applications.Count() != 0)
                    {
                        foreach (var table in dataFromIntegration.Applications)
                        {
                            BankCreditApplicationTable tableItem = new BankCreditApplicationTable();
                            tableItem.OwnerId = entity.Id;
                            tableItem.DocNum = table.DocNum;
                            tableItem.DocDate = table.DocDate;
                            tableItem.CreditSum = table.CreditSum;
                            tableItem.DocStatus = table.DocStatus;
                            tableItem.IssuanceSum = table.IssuanceSum;
                            entity.Tables.Add(tableItem);
                        }
                    }
                    _unitOfWork.Save();
                }
            }
            if (HasErrors)
                return null;

            if (!result.FromCache && !isHaveAnyData)
            {
                AddError("BankCredit tizimidan ma'lumot topilmadi!");
                return null;
            }
            return result;
        }

        private BankCreditApplication GetEntity(string tin)
        {
            return _unitOfWork.Context.BankCreditApplications.Include(a=> a.Tables)
                .FirstOrDefault(a => a.Tin == tin);
        }

    }
}
