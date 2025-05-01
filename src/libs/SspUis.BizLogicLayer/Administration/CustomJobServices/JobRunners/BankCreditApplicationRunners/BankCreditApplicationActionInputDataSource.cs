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

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.BankCreditApplicationRunners
{
    public interface IBankCreditApplicationActionInputDataSource : ICustomJobActionInputDataSource<BankCreditApplicationJobRunnerActionInputData>
    {
        
    }

    public class BankCreditApplicationJobRunnerActionInputDataSource : StatusGenericHandler, IBankCreditApplicationActionInputDataSource
    {
        private readonly DbContext _context;

        public BankCreditApplicationJobRunnerActionInputDataSource(DbContext context)
        {
            _context = context;
        }

        public async Task<BankCreditApplicationJobRunnerActionInputData[]> GetSource(CustomJob entity)
        {
            var query = _context.Set<Contractor>().IsActive()
                .Where(a => a.Inn != null)
                .Select(a => new BankCreditApplicationJobRunnerActionInputData 
                {
                    Tin = a.Inn
                });
            var source = query.ToArray();

            return source;
        }

    }
}
