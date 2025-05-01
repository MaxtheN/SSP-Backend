using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;

namespace SspUis.BizLogicLayer.IntegrationServices
{
    public class MonoApplicationIntegrationService : StatusGenericHandler, IMonoApplicationIntegrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MonoApplicationIntegrationService(IUnitOfWork unitOfWork)

        {
            _unitOfWork = unitOfWork;
        }

        public MonoAppResultDto CreateMonoApplicationBandlik(MonoApplicationBandlikResultDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {

                    MonoApplication application = _unitOfWork.Context.MonoApplications.FirstOrDefault(x => x.ApplicationId == dto.ApplicationId);
                    var entity = new MonoApplicationBandlikResult()
                    {
                        ApplicationId = application.Id,
                        Status = dto.Status,
                        SubsidyAmount = dto.SubsidyAmount,
                        ResponsiblePhone = dto.ResponsiblePhone,
                        ResponsibleFio = dto.ResponsibleFio,
                        RejectReason = dto.RejectReason
                    };
                    if (application != null)
                        _unitOfWork.Context.MonoApplicationBandlikResults.Add(entity);
                    else
                        AddError($"Ariza mavjud emas");

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    _unitOfWork.Context.SaveChanges();
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    transaction.Commit();
                    return new MonoAppResultDto()
                    {
                        Message = "success"
                    };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}