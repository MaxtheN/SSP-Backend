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
    public class PositionClassificationRepository : BaseEntityRepository<int, PositionClassification, CreatePositionClassificationDlDto, UpdatePositionClassificationDlDto>, IPositionClassificationRepository
    {
        public PositionClassificationRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<PositionClassification> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates);
    }
}
