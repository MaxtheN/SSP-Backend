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
using SspUis.DataLayer;

namespace SspUis.DataLayer.Repositories
{
    public class DistrictRepository : BaseEntityRepository<int, District, CreateDistrictDlDto, UpdateDistrictDlDto>, IDistrictRepository
    {
        public DistrictRepository(DbContext context, ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public District ByWbCode(string wbCode)
        {
            if (wbCode.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(a => a.Soato == wbCode);
        }

        public District ByRoamingCode(string roamingCode)
        {
            if (roamingCode.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(a => a.RoamingCode == roamingCode);
        }
        public District BySoato(string soato)
        {
            return ByIdQuery().FirstOrDefault(a => a.Soato == soato
                                                || soato.StartsWith(a.Soato)
                                                || a.Soato.StartsWith(soato));
        }

        protected override IQueryable<District> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Translates).Where(a => a.StateId == StateIdConst.ACTIVE);
        }

        protected override void CreateValidate(CreateDistrictDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(District entity, UpdateDistrictDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(District entity, DistrictDlDto<TDto> dto)
            where TDto : DistrictDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (!string.IsNullOrWhiteSpace(dto.Code) && query.ByNumberCode(dto.Code, isIncludePassive: true).Any())
                AddError($"Окед с этим кодом ({dto.Code}) уже существует.", nameof(dto.Code));
        }

    }
}
