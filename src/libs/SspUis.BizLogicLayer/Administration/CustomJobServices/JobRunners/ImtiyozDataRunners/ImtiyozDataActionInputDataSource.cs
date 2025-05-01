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

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.ImtiyozDataRunners
{
    public interface IImtiyozDataActionInputDataSource : ICustomJobActionInputDataSource<ImtiyozDataJobRunnerActionInputData>
    {
        
    }

    public class ImtiyozDataJobRunnerActionInputDataSource : StatusGenericHandler, IImtiyozDataActionInputDataSource
    {
        private readonly DbContext _context;

        public ImtiyozDataJobRunnerActionInputDataSource(DbContext context)
        {
            _context = context;
        }

        public async Task<ImtiyozDataJobRunnerActionInputData[]> GetSource(CustomJob entity)
        {
            var query = _context.Set<Contractor>().IsActive()
                .Where(a => a.Inn != null)
                .Select(a => new ImtiyozDataJobRunnerActionInputData
                {
                    Tin = a.Inn
                });
            var source = query.ToArray();

            return source;
        }

    }
}
