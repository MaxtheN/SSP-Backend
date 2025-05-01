using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bandlik.Models
{
    public class BandlikResponseDto
    {
        public object Error { get; set; }

        public int Id { get; set; }

        public string Jsonrpc { get; set; }
    }
    public class BandlikResultDto
    {
        public string Message { get; set; }

        public bool Success { get; set; }
    }
}
