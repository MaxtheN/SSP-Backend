using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateMemshipNewContractorDlDto : MemshipNewContractorDlDto<UpdateMemshipNewContractorDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
