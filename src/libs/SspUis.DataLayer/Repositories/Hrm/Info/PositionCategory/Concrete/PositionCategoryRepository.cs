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
    public class PositionCategoryRepository : BaseEntityRepository<int, PositionCategory, CreatePositionCategoryDlDto, UpdatePositionCategoryDlDto>, IPositionCategoryRepository
    {
        public PositionCategoryRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<PositionCategory> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates);
    }
}
