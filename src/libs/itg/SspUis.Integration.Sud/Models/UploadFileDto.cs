using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models;

public class UploadFileDto
{
    public string name { get; set; }
    public long size { get; set; }
    public string data { get; set; }
    public string type { get; set; }
}
