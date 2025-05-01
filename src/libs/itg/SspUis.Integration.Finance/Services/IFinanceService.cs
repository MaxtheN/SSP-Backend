using SspUis.Integration.Finance.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Finance.Services;

public interface IFinanceService : IStatusGeneric
{
    Task<List<GetPayDocsDto>>  GetPayDocsAsync(string code, string date);
    Task Login();
}
