using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.TaxEmployeeCountRunners
{
    public interface ITaxEmployeeCountActionInputDataSource : ICustomJobActionInputDataSource<TaxEmployeeCountActionInputData>
    {
        
    }

    public class TaxEmployeeCountActionInputDataSource : StatusGenericHandler, ITaxEmployeeCountActionInputDataSource
    {
        private readonly DbContext _context;

        public TaxEmployeeCountActionInputDataSource(DbContext context)
        {
            _context = context;
        }

        public async Task<TaxEmployeeCountActionInputData[]> GetSource(CustomJob entity)
        {
            var query = _context.Set<Contractor>().IsActive().Where(x => x.Inn != null)
                .Select(a => new TaxEmployeeCountActionInputData 
                {
                    Tin = a.Inn
                });
            var source = query.ToArray();

            return source;
        }

    }
}
