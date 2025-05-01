using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.InvestmentByInnRunners;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.SoliqImtiyozRunners;




public interface ISoliqImtiyozActionInputDataSource : ICustomJobActionInputDataSource<SoliqImtiyozActionInputData>
{

}

public class SoliqImtiyozActionInputDataSource : StatusGenericHandler, ISoliqImtiyozActionInputDataSource
{
    private readonly DbContext _context;

    public SoliqImtiyozActionInputDataSource(DbContext context)
    {
        _context = context;
    }

    public async Task<SoliqImtiyozActionInputData[]> GetSource(CustomJob entity)
    {
        var query = _context.Set<Contractor>().IsActive()
            .Where(a => a.Inn != null)
            .Select(a => new SoliqImtiyozActionInputData
            {
                Tin = a.Inn,
                RegionId = a.RegionId,
                DistrictId = a.DistrictId
            });
        var source = query.ToArray();

        return source;
    }

}