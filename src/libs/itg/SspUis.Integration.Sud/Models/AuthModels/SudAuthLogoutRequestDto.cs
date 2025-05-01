using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models.AuthModels;

public class SudAuthLogoutRequestDto
{
    public Guid ActiveSessionId { get; set; }
}
