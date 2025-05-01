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

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.FundTadbirkorRunners
{
    public interface IFundTadbirkorActionInputDataSource : ICustomJobActionInputDataSource<FundTadbirkorActionInputData>
    {
        
    }

    public class FundTadbirkorActionInputDataSource : StatusGenericHandler, IFundTadbirkorActionInputDataSource
    {
        private readonly DbContext _context;

        public FundTadbirkorActionInputDataSource(DbContext context)
        {
            _context = context;
        }

        public async Task<FundTadbirkorActionInputData[]> GetSource(CustomJob entity)
        {
            var query = _context.Set<Contractor>().IsActive()
                .Where(a => a.Inn != null)
                .Select(a => new FundTadbirkorActionInputData 
                {
                    Tin = a.Inn
                });
            var source = query.ToArray();

            return source; 
        }

    }
}
