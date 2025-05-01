using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class IndicatorDistrictRepository : 
        BaseEntityRepository<int, IndicatorDistrict, CreateIndicatorDistrictDlDto, UpdateIndicatorDistrictDlDto>, IIndicatorDistrictRepository 
    {
        public IndicatorDistrictRepository(ICrudServices crudServices) : base(crudServices)
        {  
        }

        protected override IQueryable<IndicatorDistrict> InjectFilter(IQueryable<IndicatorDistrict> query)
        {
            query = query.Where(a => a.StateId != StateIdConst.PASSIVE);
            return query.AsQueryable();
        }

        protected override IQueryable<IndicatorDistrict> ByIdQuery() => 
                AllAsQueryable.Include(a => a.Translates).Include(b => b.Tables);
    }
}
