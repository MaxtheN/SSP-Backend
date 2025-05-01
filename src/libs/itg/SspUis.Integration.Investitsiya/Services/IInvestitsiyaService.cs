using SspUis.Integration.Investitsiya.Models;
using StatusGeneric;
namespace SspUis.Integration.Investitsiya.Services
{
    public interface IInvestitsiyaService : IStatusGeneric
    {
        Task<List<InvestmentContract>> GetInvestmentContracts(InvestitsiyaRequestDto dto);
        Task<List<InvestitsiyaCntrTypeRecord>> GetInvestmentContractTypes();
        Task<List<InvestitsiyaAgreementStates>> GetInvestmentAgreementStates();
        Task<List<CntrSubjectDto>> GetCntrSubject();
    }
}
