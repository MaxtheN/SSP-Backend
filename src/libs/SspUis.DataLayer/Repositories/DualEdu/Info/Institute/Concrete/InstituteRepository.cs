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
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.DataLayer.Repositories
{
    public class InstituteRepository : BaseEntityRepository<int, Institute, CreateInstituteDlDto, UpdateInstituteDlDto>, IInstituteRepository
    {
        public InstituteRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<Institute> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates);
    }
}
