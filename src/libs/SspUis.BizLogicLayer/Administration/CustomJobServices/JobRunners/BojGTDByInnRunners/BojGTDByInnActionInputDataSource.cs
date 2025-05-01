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

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.BojGTDByInnRunners
{
    public interface IBojGTDByInnActionInputDataSource : ICustomJobActionInputDataSource<BojGTDByInnActionInputData>
    {
        
    }

    public class BojGTDByInnActionInputDataSource : StatusGenericHandler, IBojGTDByInnActionInputDataSource
    {
        private readonly DbContext _context;

        public BojGTDByInnActionInputDataSource(DbContext context)
        {
            _context = context;
        }

        public async Task<BojGTDByInnActionInputData[]> GetSource(CustomJob entity)
        {
            var query = _context.Set<Contractor>().IsActive()
                .Where(a => a.Inn != null)
                .Select(a => new BojGTDByInnActionInputData
                {
                    Tin = a.Inn
                });
            var source = query.ToArray();

            return source;
        }

    }
}
