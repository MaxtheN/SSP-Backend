

using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE.Models;


namespace SspUis.BizLogicLayer.IntegrationServices;

public class CallCenterIntegrationService : StatusGenericHandler, ICallCenterIntegrationService
{

    private readonly IUnitOfWork _unitOfWork;

    public CallCenterIntegrationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public HaveId<long> CreateCallCenter(int callCount)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = new CallCenter()
                {
                    Date = DateTime.UtcNow,
                    CallCount = callCount
                };
                _unitOfWork.Context.CallCenters.Add(entity);

                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }
                _unitOfWork.Save();

                transaction.Commit();

                return HaveId.Create(entity.Id);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

   

    public int GetLastTime()
    {
        var entity  = _unitOfWork.Context.CallCenters.OrderByDescending(s => s.Date).FirstOrDefault();
        
        return entity.CallCount;
        
        
    }
}
