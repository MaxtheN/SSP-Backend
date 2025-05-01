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
    public class IdentityDocumentRepository : BaseEntityRepository<int, IdentityDocument, CreateIdentityDocumentDlDto, UpdateIdentityDocumentDlDto>, IIdentityDocumentRepository
    {
        public IdentityDocumentRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        public IdentityDocument ByCode(string code)
        {
            if (code.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(a => a.Code == code);
        }

        protected override IQueryable<IdentityDocument> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Translates);
        }

        protected override void CreateValidate(CreateIdentityDocumentDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(IdentityDocument entity, UpdateIdentityDocumentDlDto dto)
        {
            Validate(entity, dto);
        }

        protected void Validate<TDto>(IdentityDocument entity, IdentityDocumentDlDto<TDto> dto)
            where TDto : IdentityDocumentDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(x => x.Id == entity.Id);
        }
    }
}
