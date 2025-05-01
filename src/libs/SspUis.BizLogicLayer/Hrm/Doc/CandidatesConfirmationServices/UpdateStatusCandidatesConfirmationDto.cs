using SspUis.Core;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusCandidatesConfirmationDto : UpdateStatusCandidatesConfirmationDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }

    public class CancelStatusCandidatesConfirmationDto : UpdateStatusCandidatesConfirmationDto
    {
        public CancelStatusCandidatesConfirmationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
    }
}
