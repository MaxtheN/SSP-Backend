using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models.AuthModels;

public class SudAuthLogoutResponseDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public Guid SessionId { get; set; }
    public bool IsDeleted { get; set; }
}
