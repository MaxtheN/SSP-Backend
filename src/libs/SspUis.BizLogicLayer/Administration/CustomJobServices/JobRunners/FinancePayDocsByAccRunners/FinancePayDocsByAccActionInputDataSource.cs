

using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FundTadbirkorRunners;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FinancePayDocsByAccRunners;


public interface IFinancePayDocsByAccActionInputDataSource : ICustomJobActionInputDataSource<FinancePayDocsByAccActionInputData>
{

}

public class FinancePayDocsByAccActionInputDataSource : StatusGenericHandler, IFinancePayDocsByAccActionInputDataSource
{
    private readonly DbContext _context;

    public FinancePayDocsByAccActionInputDataSource(DbContext context)
    {
        _context = context;
    }
    public async Task<FinancePayDocsByAccActionInputData[]> GetSource(CustomJob entity)
    {
        var query = _context.Set<FinancePayDocsByAcc>()
                .Select(a => new FinancePayDocsByAccActionInputData
                {
                    Id2 = a.Id2
                });
        var source = query.ToArray();

        return source;
    }
}
