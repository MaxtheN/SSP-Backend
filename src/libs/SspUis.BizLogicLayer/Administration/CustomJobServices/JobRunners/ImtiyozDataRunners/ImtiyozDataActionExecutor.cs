using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
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

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.ImtiyozDataRunners
{
    public interface IImtiyozDataActionExecutor : ICustomJobActionExecuter<ImtiyozDataJobRunnerActionInputData>
    {
        
    }

    public class ImtiyozDataJobRunnerActionExecutor : StatusGenericHandler, IImtiyozDataActionExecutor
    {
        private readonly IContractorService _contractorService;
        private readonly IUnitOfWork _unitOfWork;

        public ImtiyozDataJobRunnerActionExecutor(IContractorService contractorService, IUnitOfWork unitOfWork)
        {
            _contractorService = contractorService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, ImtiyozDataJobRunnerActionInputData actionInputData)
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
                entity = new ImtiyozData
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
                var dataFromIntegration = await _contractorService.GetImtiyozDataByInnFromSoliq(actionInputData.Tin, year);
                CombineStatuses(_contractorService);
                if (dataFromIntegration == null || HasErrors)
                {
                    _unitOfWork.Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
                else
                {
                    isHaveAnyData = true;
                    entity.Tin = actionInputData.Tin;
                    entity.Year = year;
                    entity.Tables.Clear();
                    foreach (var table in dataFromIntegration)
                    {
                        ImtiyozDataTable tableItem = new ImtiyozDataTable();
                        tableItem.OwnerId = entity.Id;
                        tableItem.Name = table.Name;
                        tableItem.Cnt = table.Cnt;
                        tableItem.Summa = table.Summa;  
                        tableItem.LgotaId = table.LgotaId;
                        entity.Tables.Add(tableItem);
                    }
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

        private ImtiyozData GetEntity(string tin, int year)
        {
            return _unitOfWork.Context.ImtiyozDatas.Include(a=> a.Tables)
                .FirstOrDefault(a => a.Tin == tin && a.Year == year);
        }

    }
}
