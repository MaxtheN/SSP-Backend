using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class MfyRepository :
        BaseEntityRepository<long, Mfy, CreateMfyDlDto, UpdateMfyDlDto>,
        IMfyRepository
    {
        public MfyRepository(DbContext context,
                             ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public Mfy ByExternalId(long externalId)
           => ByIdQuery().FirstOrDefault(a => a.ExternalId == externalId);
        

        protected override void CreateValidate(CreateMfyDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Mfy entity,
                                               UpdateMfyDlDto dto)
        =>  Validate(entity, dto);
        

        private void Validate<TDto>(Mfy entity,
                                    MfyDlDto<TDto> dto)
            where TDto : MfyDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (query.ByExternalId(dto.ExternalId, isIncludePassive: true).Any())
                AddError($"Окед с этим кодом ({dto.ExternalId}) уже существует.", nameof(dto.ExternalId));
        }

    }
}
