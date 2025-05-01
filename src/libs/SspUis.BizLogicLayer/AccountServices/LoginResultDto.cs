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

namespace SspUis.BizLogicLayer.AccountServices
{
    public class LoginResultDto
    {
        public string Token { get; set; }
        public AccountUserDto User { get; set; }
    }
}
