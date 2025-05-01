using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.Integration.Finance.Services;
using SspUis.Integration.TadbirkorFund;
using StatusGeneric;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FinancePayDocsByAccRunners;

public interface IFinancePayDocsByAccActionExecutor : ICustomJobActionExecuter<FinancePayDocsByAccActionInputData>
{

}
public class FinancePayDocsByAccActionExecuter : StatusGenericHandler, IFinancePayDocsByAccActionExecutor
{
    private readonly IFinanceService _financeService;
    private readonly IUnitOfWork _unitOfWork;

    public FinancePayDocsByAccActionExecuter(IFinanceService financeService, IUnitOfWork unitOfWork)
    {
        _financeService = financeService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, FinancePayDocsByAccActionInputData actionInputData)
    {
        //int year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);
        var date = DateTime.Now.ToString();
        bool isHaveAnyData = false;
        var result = new CustomJobActionExecuteResult
        {
            FromCache = !job.IsForceUpdate
        };
        var entity = GetEntity(actionInputData.Id2);
        bool isGetFromApi = false;
        if (entity == null)
        {
            isGetFromApi = true;
            result.FromCache = false;
            entity = new FinancePayDocsByAcc
            {
                Id2 = actionInputData.Id2
            };
            _unitOfWork.Context.Add(entity);
        }
        else if (job.IsForceUpdate)
            isGetFromApi = true;

        if(isGetFromApi)
        {
            var datas = await _financeService.GetPayDocsAsync("",date);
            CombineStatuses(_financeService);
            var data = datas.FirstOrDefault(s => s.Id == actionInputData.Id2);
            if (data == null || HasErrors || data.IsNullOrEmptyObject())
            {
                _unitOfWork.Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            
            entity.Acc = data.Acc;
            entity.Id2 = data.Id;
            entity.ClAcc = data.ClAcc;
            entity.ClInn = data.ClInn;
            entity.ClMfo = data.ClMfo;
            entity.ClName = data.ClName;
            entity.CoAcc = data.CoAcc;
            entity.CoInn = data.CoInn;
            entity.CoMfo = data.CoMfo;
            entity.CoName = data.CoName;
            entity.Purpose = data.Purpose;
            entity.SumPay = data.SumPay;
            entity.BankDocId = data.BankDocId;
            entity.DocNumb = data.DocNumb;
            entity.FinYear = data.FinYear;
            entity.BankDate = DateOnly.Parse(data.BankDate);
            entity.DocDate = DateOnly.Parse(data.DocDate);
            _unitOfWork.Save();
        }
        if (HasErrors)
            return null;

        if (!result.FromCache && !isHaveAnyData)
        {
            AddError("Moliyadan ma'lumot topilmadi");
            return null;
        }
        return result;

    }
    private FinancePayDocsByAcc? GetEntity(long id)
    {
        return _unitOfWork.Context.FinancePayDocsByAccs.FirstOrDefault(s => s.Id2 == id);

    }
}
