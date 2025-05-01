using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IBusinessActivityTypeRepository : IBaseEntityRepository<long, BusinessActivityType, CreateBusinessActivityTypeDlDto, UpdateBusinessActivityTypeDlDto>
    {

    }
}
