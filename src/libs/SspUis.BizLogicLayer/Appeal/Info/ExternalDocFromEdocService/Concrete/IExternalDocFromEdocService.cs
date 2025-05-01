using SspUis.DataLayer.Repositories;
using SspUis.Integration.Edoc.Models;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface IExternalDocFromEdocService : IStatusGeneric
    {
        HaveId<int> Create(CreateExternalDocumentFromEdocDlDto dto);
        void Update(UpdateExternalDocumentFromEdocDlDto dto);
        void Delete(int id);
        ExternalIncomingDocumentDto GetByCallCenterAppealId(long callCenterAppealId);
        ExternalIncomingDocumentDto GetByAppealId(long appealApplicationId);
        void UpdateForAppeal(UpdateExternalDocumentFromEdocDlDto dto);
    }
}
