using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System.Collections.Generic;
using System.Linq;

namespace SspUis.BizLogicLayer.Doc;

public class MonoApplicationResultService : StatusGenericHandler, IMonoApplicationResultService
{
    private IUnitOfWork _unitOfWork;
    private readonly DbSet<MonoApplicationBandlikResult> _repository;

    public MonoApplicationResultService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.Context.Set<MonoApplicationBandlikResult>();
    }
    public List<MonoApplicationBandlikResultDto> GetList()
    {

        var list = _repository.Select(a => new MonoApplicationBandlikResultDto
        {
            ApplicationId = a.ApplicationId,
            Status = a.Status,
            SubsidyAmount = a.SubsidyAmount,
            ResponsibleFio = a.ResponsibleFio,
            ResponsiblePhone = a.ResponsiblePhone,
            RejectReason = a.RejectReason
        }).ToList();

        return list;
    }

    public MonoApplicationBandlikResultDto GetAppId(long appId)
    {
        var monoAppRes = _repository
            .Where(a => a.ApplicationId == appId)
            .Select(a => new MonoApplicationBandlikResultDto
            {
                ApplicationId = a.ApplicationId,
                Status = a.Status,
                SubsidyAmount = a.SubsidyAmount,
                ResponsibleFio = a.ResponsibleFio,
                ResponsiblePhone = a.ResponsiblePhone,
                RejectReason = a.RejectReason
            })
            .FirstOrDefault();

        if( monoAppRes == null )
        {
            AddError("404 Mono Application Not Found");
            return null;
        }

        return monoAppRes;
    }

}