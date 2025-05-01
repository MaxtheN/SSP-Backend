using System;
using System.Linq;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class FixedMinimumValueRepository : BaseEntityRepository<long, FixedMinimumValue, CreateFixedMinimumValueDlDto, UpdateFixedMinimumValueDlDto>, IFixedMinimumValueRepository
    {
        private readonly ICrudServices _crudServices;

        public FixedMinimumValueRepository(ICrudServices crudServices)
            : base(crudServices)
        {
            _crudServices = crudServices;
        }

        protected override IQueryable<FixedMinimumValue> InjectFilter(IQueryable<FixedMinimumValue> query)
        {
            return query.Where(x => x.StateId == StateIdConst.ACTIVE);
        }
        public FixedMinimumValue GetBhmByDate(DateOnly dateTime, int minimumValueTypeId)
        {
            var query = _crudServices.Context.Set<FixedMinimumValue>()/* ReadAsNoTracked<FixedMinimumValue>(false)*/
                .OrderByDescending(x => x.DateOn);
            var value = query.FirstOrDefault(x =>
                   x.DateOn <= dateTime
                   && x.MinimumValueTypeId == minimumValueTypeId);
            return value;
        }
    }
}
