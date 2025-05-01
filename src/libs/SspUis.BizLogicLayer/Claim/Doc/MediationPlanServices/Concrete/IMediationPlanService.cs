using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer.Repositories.Claim;
using StatusGeneric;
using System;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim
{
    public interface IMediationPlanService : IStatusGeneric
    {
        PagedResult<MediationPlanListDto> GetList(MediationPlanSortFilterOptions options);
        MediationPlanDto Get();
        MediationPlanDto Get(long id);
        SelectList<long> AsSelectList(MediationPlanSortFilterOptions options);
        HaveId<long> Create(CreateMediationPlanDlDto dto);
        void Update(UpdateMediationPlanDlDto dto);
        Task Accept(UpdateStatusMediationPlanDto dTo);
        Task Cancel(UpdateStatusMediationPlanDto dTo);
        void Delete(long id);
        MediationPlanDto GetByApplication(int applicationId);
        byte[] DownloadPdf(Guid id2, string? lang);
    }
}


