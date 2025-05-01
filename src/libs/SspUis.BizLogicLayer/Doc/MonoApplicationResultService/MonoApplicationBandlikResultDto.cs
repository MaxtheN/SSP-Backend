using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Doc
{
    public class MonoApplicationBandlikResultDto : ILinkToEntity<MonoApplicationBandlikResult>
    {
        public long ApplicationId { get; set; }
        public int Status { get; set; }
        public string SubsidyAmount { get; set; }
        public string ResponsibleFio { get; set; }
        public string ResponsiblePhone { get; set; }
        public string RejectReason { get; set; }
    }
}
