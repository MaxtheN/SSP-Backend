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
    public class PrtnContractTypeRepository : BaseEntityRepository<int, PrtnContractType, CreatePrtnContractTypeDlDto, UpdatePrtnContractTypeDlDto>, IPrtnContractTypeRepository
    {
        public PrtnContractTypeRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override IQueryable<PrtnContractType> ByIdQuery()
            => AllAsQueryable.Include(a => a.Tables).Include(a => a.Translates);

    }
}
