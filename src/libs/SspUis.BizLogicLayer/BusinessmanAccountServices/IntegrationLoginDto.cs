using AutoMapper;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using SspUis.DataLayer;
using WEBASE.Integration.EImzo;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class IntegrationLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
