using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class BusinessActivityTypeRepository : BaseEntityRepository<long, BusinessActivityType, CreateBusinessActivityTypeDlDto, UpdateBusinessActivityTypeDlDto>, IBusinessActivityTypeRepository
    {
        public BusinessActivityTypeRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override IQueryable<BusinessActivityType> ByIdQuery()
            => AllAsQueryable.Include(a => a.Tables);
    }
}
