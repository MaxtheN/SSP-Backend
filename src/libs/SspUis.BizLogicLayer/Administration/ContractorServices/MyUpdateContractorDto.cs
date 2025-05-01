using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.BizLogicLayer.ContractorServices
{
    public class MyUpdateContractorDto : MyUpdateContractorDlDto
    {
        new public long Id { get => base.Id; internal set => base.Id = value; }
    }
}
