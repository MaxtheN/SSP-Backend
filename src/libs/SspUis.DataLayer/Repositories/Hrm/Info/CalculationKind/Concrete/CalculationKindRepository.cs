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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.DataLayer.Repositories
{
    public class CalculationKindRepository : BaseEntityRepository<int, CalculationKind, CreateCalculationKindDlDto, UpdateCalculationKindDlDto>, ICalculationKindRepository
    {
        public CalculationKindRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }
        protected override IQueryable<CalculationKind> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates)
            .Include(x=>x.Percents)
            .Include(x=>x.UsedTables)
            .Include(x=>x.AllowedDocs)
            .Include(x=>x.CalculationStructure);
    }
}
