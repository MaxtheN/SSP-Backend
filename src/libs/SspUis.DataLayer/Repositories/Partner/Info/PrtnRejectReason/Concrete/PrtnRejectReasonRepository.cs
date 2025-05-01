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
    public class PrtnRejectReasonRepository : BaseEntityRepository<int, PrtnRejectReason, CreatePrtnRejectReasonDlDto, UpdatePrtnRejectReasonDlDto>, IPrtnRejectReasonRepository
    {
        public PrtnRejectReasonRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override IQueryable<PrtnRejectReason> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
