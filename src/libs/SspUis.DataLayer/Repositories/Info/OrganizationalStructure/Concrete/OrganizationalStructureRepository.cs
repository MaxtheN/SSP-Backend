using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationalStructureRepository : BaseEntityRepository<int, OrganizationalStructure, CreateOrganizationalStructureDlDto, UpdateOrganizationalStructureDlDto>, IOrganizationalStructureRepository
    {
        public OrganizationalStructureRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override void OnCreate(OrganizationalStructure entity, CreateOrganizationalStructureDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(OrganizationalStructure entity, UpdateOrganizationalStructureDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        private void SetEntityProperties<TDto>(OrganizationalStructure entity, OrganizationalStructureDlDto<TDto> dto)
            where TDto : OrganizationalStructureDlDto<TDto>
        {
            if (!string.IsNullOrEmpty(dto.Code))
            {
                entity.CodeSymbol = dto.Code[0].ToString();

                if (dto.Code.Length == 1)
                    entity.CodeNumber = 0;
                else
                {
                    var numberPart = dto.Code.Substring(1);
                    int.TryParse(numberPart, out var codeNumber);
                    entity.CodeNumber = codeNumber;
                }
            }
        }

        public OrganizationalStructure ByWbCode(string wbCode)
        {
            if (wbCode.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(w => w.Code == wbCode);
        }

        protected override IQueryable<OrganizationalStructure> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Translates)
                                  .Include(a => a.StructureCalculationKind)
                                  .Include(a => a.StructurePosition)
                                  .Include(a => a.StructureStaffingIndicator).ThenInclude(a => a.Tables);
        }

        //private void Validate<TDto>(OrganizationalStructure entity, OrganizationalStructureDlDto<TDto> dto)
        //    where TDto : OrganizationalStructureDlDto<TDto>
        //{
        //    var query = DbSet.AsQueryable();

        //    if (entity != null)
        //        query = query.Where(a => a.Id != entity.Id);

        //    if (query.ByNumberCode(dto.Code, isIncludePassive: true).Any())
        //        AddError($"Тип организации по штат. расписанию с этим кодом ({dto.Code}) уже существует.", nameof(dto.Code));
        //}

    }
}
