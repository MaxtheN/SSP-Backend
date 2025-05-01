using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Srv.Doc.SrvApplicationService.Dtos
{
    public class SrvStatisticsDto
    {
        public ApplicationDto Application { get; set; }
        public Contract Contract { get; set; }
        public Deed Deed { get; set; }
    }

    public class ApplicationDto
    {
        public int ApplicationCount {  get; set; }
        public int FreeApplicationCount {  get; set; }
        public int RejectApplicationCount { get; set; }
    }

    public class Contract
    {
        public int ContractCount { get; set; }
        public int FreeContractnCount { get; set; }
        public int RejectContractCount { get; set; }
    }

    public class Deed
    {
        public int DeedCount { get; set; }
        public int FreeDeedCount { get; set; }
        public int RejectDeedCount { get; set; }
    }
}
