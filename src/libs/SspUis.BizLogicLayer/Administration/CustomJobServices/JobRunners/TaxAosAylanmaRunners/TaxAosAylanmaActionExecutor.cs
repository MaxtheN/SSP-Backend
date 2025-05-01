using DocumentFormat.OpenXml.Bibliography;
using OpenXmlPowerTools;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.Integration.Soliq;
using StatusGeneric;
using System;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxAosAylanmaRunners;

public interface ITaxAosAylanmaActionExecutor : ICustomJobActionExecuter<TaxAosAylanmaActionInputData>
{

}
public class TaxAosAylanmaActionExecutor : StatusGenericHandler, ITaxAosAylanmaActionExecutor
{
    private readonly ISoliqContractorService _contractorService;
    private readonly IUnitOfWork _unitOfWork;

    public TaxAosAylanmaActionExecutor(ISoliqContractorService contractorService, IUnitOfWork unitOfWork)
    {
        _contractorService = contractorService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, TaxAosAylanmaActionInputData actionInputData)
    {

        int year;
        if (job.ExtendData.Length == 4)
        {
            year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);
        }
        else
        {
            year = DateOnly.Parse(job.ExtendData).Year;
        }

        var result = new CustomJobActionExecuteResult();

        try
        {

            var entity = GetEntity(year, actionInputData.Tin);

            if (entity != null || _unitOfWork.Context.QqsAylanmas.Any(a => a.Year == year && a.Inn == actionInputData.Tin && (a.NetIncomeWithoutVat != 0 || a.VatSum != 0)))
            {
                return result;
            }

            int inn = Int32.Parse(actionInputData.Tin);
            var dataFromIntegration = await _contractorService.GetAosAylanmaData(inn, year);
            CombineStatuses(_contractorService);

            if (HasErrors || dataFromIntegration == null || (dataFromIntegration.NetIncome == 0))
            {
                AddError("Soliqdan ma'lumot topilmadi!");
                return result;
            }

            entity = new AosAylanma
            {
                Inn = actionInputData.Tin,
                Year = year,
            };

            _unitOfWork.Context.Add(entity);
            entity.NetIncome = dataFromIntegration.NetIncome;
            _unitOfWork.Save();
            return result;
        }
        catch (Exception ex)
        {
            AddError(ex.GetInnermostException().Message);
            result.UserMessage = "Catchga tushdi";
            return result;
        }
    }


    public AosAylanma GetEntity(int year, string inn)
    {
        return _unitOfWork.Context.AosAylanmas.FirstOrDefault(s =>
                                    s.Inn == inn && s.Year == year
                                    );
    }
   
}
