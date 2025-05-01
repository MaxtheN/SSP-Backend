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
    public class DualEducationTypeRepository : BaseEntityRepository<int, DualEducationType, CreateDualEducationTypeDlDto, UpdateDualEducationTypeDlDto>, IDualEducationTypeRepository
    {
        public DualEducationTypeRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<DualEducationType> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates);
    }
}
