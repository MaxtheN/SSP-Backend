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
    public interface IDistrictRepository : IBaseEntityRepository<int, District, CreateDistrictDlDto, UpdateDistrictDlDto>
    {
        District ByWbCode(string wbCode);
        District ByRoamingCode(string wbCode);
        District BySoato(string soato);
    }
}
