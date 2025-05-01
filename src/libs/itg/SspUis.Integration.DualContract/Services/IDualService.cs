using SspUis.Integration.Dual.Models;
using StatusGeneric;

namespace SspUis.Integration.Dual.Services;

public interface IDualService: IStatusGenericHandler
{
    Task<string> RejectDualContract(DualContractRejectDto dto);
}