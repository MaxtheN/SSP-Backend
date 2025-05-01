using SspUis.Integration.Bojxona.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.ContractorServices
{
    public class GTDFromBojxonaDto
    {
        public List<GetGTDByInnDataDto> Imports { get; set; }
        public List<GetGTDByInnDataDto> Exports {  get; set; }
    }
}
