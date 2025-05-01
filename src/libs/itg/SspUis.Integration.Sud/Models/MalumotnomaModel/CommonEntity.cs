using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models.MalumotnomaModel
{
    public class CommonEntity
    {
        public Guid id { get; set; }
        public CommonName Names { get; set; }
    }

    public class CommonName
    {
        public string Uz { get; set; }
        public string Ru { get; set; }
        public string Uz_cyr { get; set; }
    }
}
