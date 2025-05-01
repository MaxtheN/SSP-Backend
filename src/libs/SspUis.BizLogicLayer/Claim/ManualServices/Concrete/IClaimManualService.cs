using SspUis.Core;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim;

public interface IClaimManualService : IStatusGeneric
{
    SelectList<int> ClaimApplicationTypeSelectList(int? langId);
    SelectList<int> ClaimNeedCourtSelectList();
    SelectList<int> ClaimResponsibleTypeSelectList(int? langId);
    SelectList<int> MediationResultSelectList();
    SelectList<int> MediationTypeSelectList();
}
