using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.Core.Security
{
    public class IntegrationUserAuthModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Inn { get; set; } 
    }

}
