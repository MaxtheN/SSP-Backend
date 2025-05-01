using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models.AuthModels;
public class SudAuthCreateResponseDto
{
    public int statusCode { get; set; }

    public Guid refresh_token_id { get; set; }

    public Guid session_id { get; set; }

    public DateTime session_active_until { get; set; }
}

