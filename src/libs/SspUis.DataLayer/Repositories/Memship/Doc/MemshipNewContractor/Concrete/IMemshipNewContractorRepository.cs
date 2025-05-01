using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IMemshipNewContractorRepository : IBaseEntityRepository<long, MemshipNewContractor, CreateMemshipNewContractorDlDto, UpdateMemshipNewContractorDlDto, UpdateStatusMemshipNewContractorDlDto>
{

}

