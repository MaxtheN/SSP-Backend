using DocumentFormat.OpenXml.Bibliography;
using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.InvestmentByInnRunners;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.Integration.Soliq;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.SoliqImtiyozRunners;



public interface ISoliqImtiyozActionExecutor : ICustomJobActionExecuter<SoliqImtiyozActionInputData>
{

}

public class SoliqImtiyozActionExecutor : StatusGenericHandler, ISoliqImtiyozActionExecutor
{
    private readonly ISoliqContractorService _contractorService;
    private readonly IUnitOfWork _unitOfWork;

    public SoliqImtiyozActionExecutor(ISoliqContractorService contractorService, IUnitOfWork unitOfWork)
    {
        _contractorService = contractorService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, SoliqImtiyozActionInputData actionInputData)
    {
        var year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);

        int inn = Int32.Parse(actionInputData.Tin);

        bool isHaveAnyData = false;
        var result = new CustomJobActionExecuteResult
        {
            FromCache = !job.IsForceUpdate
        };
        var entity = GetEntity(inn, year);
        bool isGetFromApi = false;

        if (entity == null)
        {
            isGetFromApi = true;
            result.FromCache = false;
            entity = new SoliqImtiyoz
            {
                Tin = inn,
                Year = year
            };
            _unitOfWork.Context.Add(entity);
        }
        else if (job.IsForceUpdate)
            isGetFromApi = true;

        if (isGetFromApi)
        {
            var dataFromIntegration = await  _contractorService.GetSoliqImtiyozlari(inn, year);
            CombineStatuses(_contractorService);
            if (dataFromIntegration == null || dataFromIntegration.Data == null || HasErrors )
            {
                _unitOfWork.Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            else
            {
                isHaveAnyData = true;
                entity.Summa = dataFromIntegration.Data.Summa;
                entity.ImtiyozCount = dataFromIntegration.Data.Count;
                entity.DistrictId = actionInputData.DistrictId;
                entity.RegionId = actionInputData.RegionId;
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
    public SoliqImtiyoz GetEntity(int inn, int year)
    {
        return _unitOfWork.Context.SoliqImtiyozs.FirstOrDefault(s =>
                                    s.Tin == inn && s.Year == year);
    }
}
