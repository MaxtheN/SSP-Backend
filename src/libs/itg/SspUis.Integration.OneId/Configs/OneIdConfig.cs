using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.OneId;

public class OneIdConfig
{
    public string BaseUri { get; set; }
    public string ClientId { get; set; }
    public string Secret { get; set; }
    public string Scope { get; set; }
    public bool UseProxy { get; set; }
}
