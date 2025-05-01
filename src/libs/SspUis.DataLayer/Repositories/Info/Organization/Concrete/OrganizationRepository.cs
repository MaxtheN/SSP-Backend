using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using WEBASE;
using WEBASE.EF;
using System.Text.RegularExpressions;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationRepository : BaseEntityRepository<int, Organization, CreateOrganizationDlDto, UpdateOrganizationDlDto>, IOrganizationRepository
    {
        public OrganizationRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<Organization> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.SettlementAccounts)
                .Include(a => a.Signs.Where(a => !a.ExpireOn.HasValue))
                .Include(a => a.Translates)
                .Include(a => a.Oked)
                .Include(a => a.Files);
        }

        protected override void OnCreate(Organization entity, CreateOrganizationDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }
        protected override void OnUpdate(Organization entity, UpdateOrganizationDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        private void SetEntityProperties<TDto>(Organization entity, OrganizationDlDto<TDto> dto)
           where TDto : OrganizationDlDto<TDto>
        {

            foreach (var signers in entity.Signs)
            {

            }
            foreach (var fileName in entity.Files)
            {
                fileName.ColumnName = dto.Files.FirstOrDefault().ColumnName;
            }
        }

    }
}
