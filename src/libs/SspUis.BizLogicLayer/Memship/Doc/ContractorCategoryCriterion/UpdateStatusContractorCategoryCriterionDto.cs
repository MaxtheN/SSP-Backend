using GenericServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusContractorCategoryCriterionDto : UpdateStatusContractorCategoryCriterionDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
}
