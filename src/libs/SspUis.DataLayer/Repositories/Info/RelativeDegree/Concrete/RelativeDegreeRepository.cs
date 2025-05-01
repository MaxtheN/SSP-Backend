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

namespace SspUis.DataLayer.Repositories
{
    public class RelativeDegreeRepository : BaseEntityRepository<int, RelativeDegree, CreateRelativeDegreeDlDto, UpdateRelativeDegreeDlDto>, IRelativeDegreeRepository
    {
        public RelativeDegreeRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        public RelativeDegree ByCode(string code)
        {
            if (code.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(a => a.Code == code);
        }

        protected override IQueryable<RelativeDegree> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Translates);
        }

        protected override void CreateValidate(CreateRelativeDegreeDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(RelativeDegree entity, UpdateRelativeDegreeDlDto dto)
        {
            Validate(entity, dto);
        }

        protected void Validate<TDto>(RelativeDegree entity, RelativeDegreeDlDto<TDto> dto)
            where TDto : RelativeDegreeDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(x => x.Id == entity.Id);
        }
    }
}
