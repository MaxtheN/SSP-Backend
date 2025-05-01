using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class PositionRepository : BaseEntityRepository<int, Position, CreatePositionDlDto, UpdatePositionDlDto>, IPositionRepository
    {
        private readonly ICrudServices crudServices;
        public PositionRepository(ICrudServices crudServices)
            : base(crudServices)
        {
            this.crudServices = crudServices;
        }
        
        protected override IQueryable<Position> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Translates);
        }

        protected override void CreateValidate(CreatePositionDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Position entity, UpdatePositionDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Position entity, PositionDlDto<TDto> dto)
            where TDto : PositionDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            /*if (query.ByNumberCode(dto.Code, isIncludePassive: true).Any())
                AddError($"Должность с этим кодом ({dto.Code}) уже существует.", nameof(dto.Code));*/
            var existingData = this.crudServices.Context.Set<Position>()
                                                            .IsActive()
                                                            .FirstOrDefault(a => a.FullName == dto.FullName ||
                                                                      a.ShortName == dto.ShortName);

            if (existingData != null &&
               (
                (entity == null) ||
                 (entity != null &&
                  entity.Id != existingData.Id
                 )
                )
               )
                AddError("Эта должность уже существует в системе");
        }

    }
}
