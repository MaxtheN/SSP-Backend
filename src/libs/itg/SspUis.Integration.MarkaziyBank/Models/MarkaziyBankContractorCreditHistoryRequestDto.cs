using Newtonsoft.Json;
using SspUis.Core;

namespace SspUis.Integration.MarkaziyBank
{
    public class MarkaziyBankContractorCreditHistoryRequestDto
    {
        public string Inn { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
      
    }
}