using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusCompleteService : UpdateStatusCompletedServiceDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        public override void UpdateEntity(CompletedService entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusId;
            entity.Details = Details;
        }

        public class CancelStatusCompletedSrvDto : UpdateStatusCompleteService
        {
            public CancelStatusCompletedSrvDto()
            {
                base.StatusId = StatusIdConst.CANCELED;
            }
            public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        }

        public class AcceptStatusCompletedSrvDto : UpdateStatusCompleteService
        {
            public AcceptStatusCompletedSrvDto()
            {
                base.StatusId = StatusIdConst.CANCELED;
            }
            public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        }
    }
}