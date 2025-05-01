using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxDebtRunners;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxQqsAylanmaRunners;



public interface ITaxQqsAylanmaActionInputDataSource : ICustomJobActionInputDataSource<TaxQqsAylanmaActionInputData>
{

}
public class TaxQqsAylanmaActionInputDataSource : StatusGenericHandler, ITaxQqsAylanmaActionInputDataSource
{
    private readonly DbContext _context;

    public TaxQqsAylanmaActionInputDataSource(DbContext context)
    {
        _context = context;
    }

    public async Task<TaxQqsAylanmaActionInputData[]> GetSource(CustomJob entity)
    {
        var query = _context.Set<Contractor>().IsActive()
                .Where(a => a.Inn != null)
                .Select(a => new TaxQqsAylanmaActionInputData
                {
                    Tin = a.Inn
                });
        var source = query.ToArray();

        return source;
    }
}