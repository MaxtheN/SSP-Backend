using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;
using SspUis.DataLayer;

namespace SspUis.DataLayer.Repositories
{
    public class NotBudgetContractorRepository : BaseEntityRepository<long, NotBudgetContractor, CreateNotBudgetContractorDlDto, UpdateNotBudgetContractorDlDto>, INotBudgetContractorRepository
    {
        public NotBudgetContractorRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public NotBudgetContractor ByInn(string inn)
        {
            return ByIdQuery().FirstOrDefault(a => a.Inn == inn);
        }

        public IQueryable<NotBudgetContractor> ByInns(params string[] inns)
        {
            return ByIdQuery().Where(a => inns.Contains(a.Inn));
        }
    }
}
