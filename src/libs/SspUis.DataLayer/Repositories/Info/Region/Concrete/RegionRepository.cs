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
    public class RegionRepository : BaseEntityRepository<int, Region, CreateRegionDlDto, UpdateRegionDlDto>, IRegionRepository
    {
        public RegionRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public Region ByWbCode(string wbCode)
        {
            if (wbCode.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(a => a.Code == wbCode);
        }

        public Region ByRoamingCode(string roamingCode)
        {
            if (roamingCode.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(a => a.RoamingCode == roamingCode);
        }

        protected override IQueryable<Region> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Translates);
        }

        protected override void CreateValidate(CreateRegionDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Region entity, UpdateRegionDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Region entity, RegionDlDto<TDto> dto)
            where TDto : RegionDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (!string.IsNullOrWhiteSpace(dto.Code) && query.ByNumberCode(dto.Code, isIncludePassive: true).Any())
                AddError($"Окед с этим кодом ({dto.Code}) уже существует.", nameof(dto.Code));
        }

    }
}
