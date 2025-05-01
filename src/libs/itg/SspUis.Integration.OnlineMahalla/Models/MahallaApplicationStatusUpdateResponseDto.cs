using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.OnlineMahalla
{
    public class MahallaApplicationStatusUpdateResponseDto
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public int Status { get; set; }
        public string Path { get; set; }
        public DataContent Data { get; set; }
        public string Response { get; set; }
    }

    public class DataContent
    {
        public string Case { get; set; }
    }
}
