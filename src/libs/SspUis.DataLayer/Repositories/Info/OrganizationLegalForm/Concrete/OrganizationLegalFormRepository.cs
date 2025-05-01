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
    public class OrganizationLegalFormRepository : BaseEntityRepository<int, OrganizationLegalForm, CreateOrganizationLegalFormDlDto, UpdateOrganizationLegalFormDlDto>, IOrganizationLegalFormRepository
    {
        public OrganizationLegalFormRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override IQueryable<OrganizationLegalForm> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Translates);
        }
    }
}
