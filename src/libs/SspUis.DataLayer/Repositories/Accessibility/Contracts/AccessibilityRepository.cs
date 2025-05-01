using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AccessibilityRepository : BaseEntityRepository<int, Accessibility, CreateAccessibilityDlDto, UpdateAccessibilityDlDto>, IAccessibilityRepository
    {
        public AccessibilityRepository(ICrudServices crudServices) : base(crudServices) { }

        protected override IQueryable<Accessibility> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.AccessModules);
        }

        protected override void CreateValidate(CreateAccessibilityDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Accessibility entity, UpdateAccessibilityDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Accessibility entity, AccessibilityDlDto<TDto> dto)
            where TDto : AccessibilityDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
