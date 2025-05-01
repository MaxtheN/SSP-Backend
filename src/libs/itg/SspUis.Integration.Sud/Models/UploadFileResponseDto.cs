using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models;

public class UploadFileResponseDto
{
    public int resultCode { get; set; }
    public string resultMessage { get; set; }
    public Guid id { get; set; }
    public int statusCode { get; set; }
}
