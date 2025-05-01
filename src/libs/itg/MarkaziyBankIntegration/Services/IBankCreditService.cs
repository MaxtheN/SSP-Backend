using SspUis.Integration.BankCredit.Models;
using StatusGeneric;

namespace SspUis.Integration.BankCredit
{
    public interface IBankCreditService :
        IStatusGeneric
    {
        Task<List<BankCreditReportResponse>> GetReport();
        Task<BankCreditApplicationsResponse> GetApplications(string tin, int offerSigned = 1);
        Task<List<BankCreditContractStatusResponse>> GetContractStatus();
    }
}