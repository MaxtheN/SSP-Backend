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
    public class SourceCodeRepository : BaseEntityRepository<int, SourceCode, CreateSourceCodeDlDto, UpdateSourceCodeDlDto>, ISourceCodeRepository
    {
        public SourceCodeRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }
        protected override IQueryable<SourceCode> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates);
    }
}
