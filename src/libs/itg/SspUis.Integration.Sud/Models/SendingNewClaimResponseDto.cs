using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models;

public class SendingNewClaimResponseDto
{
    public string statusCode { get; set; }
    public string case_id { get; set; }
    public string is_single_window { get; set; }
}

