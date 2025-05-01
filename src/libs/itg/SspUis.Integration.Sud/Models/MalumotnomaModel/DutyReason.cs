using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models.MalumotnomaModel
{
    public class DutyReason
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public DutyName Names { get; set; }
    }

    public class DutyName
    {
        public string Uz { get; set; }
    }
}
