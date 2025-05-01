using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Fund;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.Integration.Soliq.Models;
using SspUis.Integration.TadbirkorFund;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FundTadbirkorRunners
{
    public interface IFundTadbirkorActionExecutor : ICustomJobActionExecuter<FundTadbirkorActionInputData>
    {
        
    }

    public class FundTadbirkorActionExecutor : StatusGenericHandler, IFundTadbirkorActionExecutor
    {
        private readonly ITadbirkorFundContractorService _tadbirkorFundService;
        private readonly IUnitOfWork _unitOfWork;

        public FundTadbirkorActionExecutor(ITadbirkorFundContractorService tadbirkorFundService, IUnitOfWork unitOfWork)
        {
            _tadbirkorFundService = tadbirkorFundService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, FundTadbirkorActionInputData actionInputData)
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
                entity = new FundTadbirkor
                {
                    TinPinfl = actionInputData.Tin,
                };
                _unitOfWork.Context.Add(entity);
            }
            else if (job.IsForceUpdate)
                isGetFromApi = true;

            if (isGetFromApi)
            {
                var tadbirkorFundData = await _tadbirkorFundService.GetByInn(actionInputData.Tin);
                CombineStatuses(_tadbirkorFundService);
                if (tadbirkorFundData == null || HasErrors || tadbirkorFundData.IsNullOrEmptyObject())
                {
                    _unitOfWork.Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
                else
                {
                    foreach(var cred in entity.Credits)
                    {
                        _unitOfWork.Context.FundTadbirkorCredits.Remove(cred);
                    }

                    isHaveAnyData = true;
                    entity.ExternalId = tadbirkorFundData.Id;
                    entity.DateOn = tadbirkorFundData.DateOn;
                    entity.Name = tadbirkorFundData.Name;
                    entity.BusinessSectorId = tadbirkorFundData.BusinessSectorId;
                    entity.BusinessSectorName = tadbirkorFundData.BusinessSectorName;
                    entity.BusinessSectortypeid = tadbirkorFundData.BusinessSectorId;
                    entity.BusinessSectorTypeName = tadbirkorFundData.BusinessSectorTypeName;
                    entity.FinancialAssistanceId = tadbirkorFundData.FinancialAssistanceId;
                    entity.FinancialAsistanceName = tadbirkorFundData.FinancialAsistanceName;
                    entity.BankCode = tadbirkorFundData.BankCode;
                    entity.BankName = tadbirkorFundData.BankName;
                    entity.RegionSoato = tadbirkorFundData.RegionSoato;
                    entity.RegionName = tadbirkorFundData.RegionName;
                    entity.DistrictSoato = tadbirkorFundData.DistrictSoato;
                    entity.DistrictName = tadbirkorFundData.DistrictName;
                    entity.AidAmount = tadbirkorFundData.AidAmount;
                    entity.NewJobPosition = tadbirkorFundData.NewJobPosition;
                    entity.RealJobPosition = tadbirkorFundData.RealJobPosition;
                    entity.Credits = new List<FundTadbirkorCredit>();
                    foreach (var tadbirkorCredit in tadbirkorFundData.Credits)
                    {
                        var credit = new FundTadbirkorCredit()
                        {
                            Amount = tadbirkorCredit.Amount,
                            CurrencyId = tadbirkorCredit.CurrencyId,
                            Currency = tadbirkorCredit.Currency,
                        };
                        entity.Credits.Add(credit);
                    }
                    _unitOfWork.Save();
                }
            }
            if (HasErrors)
                return null;

            if (!result.FromCache && !isHaveAnyData)
            {
                AddError("Tadbirkorlik jamg'armasidan ma'lumot topilmadi!");
                return null;
            }
            return result;
        }

        private FundTadbirkor GetEntity(string tin)
        {
            return _unitOfWork.Context.FundTadbirkors.Include(a => a.Credits)
                .FirstOrDefault(a => a.TinPinfl == tin);
        }
    }
}
