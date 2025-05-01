using SspUis.Integration.AgroBank.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.AgroBank.Services;

public interface IAgroBankService : IStatusGeneric
{
    Task<GetLoanActualDataResponse2> GetLoanActualData(string loanId);
}
