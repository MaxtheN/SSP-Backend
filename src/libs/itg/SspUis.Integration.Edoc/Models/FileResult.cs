using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Edoc.Models
{
    public class FileResultResponse
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
    }
}
