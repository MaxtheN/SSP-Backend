using SspUis.Integration.XalqBank.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.XalqBank.Services
{
    public interface IXalqBankService : IStatusGeneric
    { 
        Task<ChekAmountResponse> ChekAmountXalqBank(string loanId);
    }
}
