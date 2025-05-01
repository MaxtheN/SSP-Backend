using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IPrtnContractRepository : IBaseEntityRepository<long, PrtnContract, CreatePrtnContractDlDto, UpdatePrtnContractDlDto, UpdateStatusPrtnContractDlDto>
    {
        public PrtnContract ChangeDate(long id, DateOnly newDocOn, Action<PrtnContract> validation = null);
        public PrtnContractSign ChangeSigner(long prtnContractSignId, int newOrganizationSignId, Action<PrtnContractSign> validation = null);
    }
}
