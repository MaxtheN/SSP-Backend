using SspUis.Core;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusCandidatesConfirmationTableDto : UpdateStatusCandidatesConfirmationTableDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
    public class CancelStatusCandidatesConfirmationTableDto : UpdateStatusCandidatesConfirmationTableDto
    {
        public CancelStatusCandidatesConfirmationTableDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
    }



}
