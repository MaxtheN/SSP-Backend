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
    public interface IPrtnCreditDemandRepository : IBaseEntityRepository<long, PrtnCreditDemand, CreatePrtnCreditDemandDlDto, UpdatePrtnCreditDemandDlDto, UpdateStatusPrtnCreditDemandDlDto>
    {
    }
}
