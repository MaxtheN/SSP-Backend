using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models.MalumotnomaModel
{
    public class CurrencyModel
    {
        public string id { get; set; }
        public int code { get; set; }
        public CurrencyModel names { get; set; }
    }

    public class CurrencyName
    {
        public string uz { get; set; }
    }
}
