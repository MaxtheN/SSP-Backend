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

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxQqsAylanmaRunners;

public interface ITaxQqsAylanmaActionExecutor : ICustomJobActionExecuter<TaxQqsAylanmaActionInputData>
{

}
public class TaxQqsAylanmaActionExecutor : StatusGenericHandler, ITaxQqsAylanmaActionExecutor
{
    private readonly ISoliqContractorService _contractorService;
    private readonly IUnitOfWork _unitOfWork;

    public TaxQqsAylanmaActionExecutor(ISoliqContractorService contractorService, IUnitOfWork unitOfWork)
    {
        _contractorService = contractorService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, TaxQqsAylanmaActionInputData actionInputData)
    {

        int currentMonth = 1;
        int year;
        int allMonthCount;
        if (job.ExtendData.Length == 4)
        {

            year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);
            allMonthCount = year == DateTime.Now.Year ? DateTime.Now.Month - 1 : 12;
        }
        else
        {
            currentMonth = DateOnly.Parse(job.ExtendData).Month;
            year = DateOnly.Parse(job.ExtendData).Year;
            allMonthCount = currentMonth;
        }


        bool isHaveAnyData = false;

        var result = new CustomJobActionExecuteResult
        {
            
        };
        try
        {
            if (_unitOfWork.Context.AosAylanmas.Any(a => a.Inn == actionInputData.Tin && a.Year == year))
                return result;

            for (int month = currentMonth; month <= allMonthCount; month++)
            {
                var entity = GetEntity(year, actionInputData.Tin, month);

                if (entity != null) 
                {
                    isHaveAnyData = true;
                    continue;
                }

                int inn = Int32.Parse(actionInputData.Tin);
                var dataFromIntegration = await _contractorService.GetQqsAylanmaData(month, inn, year);
                CombineStatuses(_contractorService);
                
                if (HasErrors || dataFromIntegration == null || (dataFromIntegration.NetIncomeWithoutVat == 0 && dataFromIntegration.VatSum == 0))
                {
                    continue;
                }

                isHaveAnyData = true;
                entity = new QqsAylanma
                {
                    Inn = actionInputData.Tin,
                    Year = year,
                    Month = month,
                };
                
                _unitOfWork.Context.Add(entity);
                
                entity.NetIncomeWithoutVat = dataFromIntegration.NetIncomeWithoutVat;
                entity.VatSum = dataFromIntegration.VatSum;
                
                _unitOfWork.Save();
            }

            if (!isHaveAnyData)
                AddError("Soliqdan ma'lumot topilmadi!");

            return result;
        }
        catch (Exception ex)
        {
            AddError(ex.GetInnermostException().Message);
            result.UserMessage = "Catchga tushdi";
            return result;
        }
    }


    public QqsAylanma GetEntity(int year, string inn, int month)
    {
        return _unitOfWork.Context.QqsAylanmas.FirstOrDefault(s =>
                                    s.Inn == inn && s.Year == year
                                    && s.Month == month
                                    );
    }
   
}
