using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Doc
{
    public interface IMonoApplicationListModel
    {
        public int? BandlikResponseStatusId { get; set; }
        public string BandlikResponseStatus { get; set; }
    }
}
