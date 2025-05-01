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

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class BusinessmanLoginResultDto
    {
        public bool TrustedDevice { get; set; }
        public bool RequiredPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int BusinessmanUserId { get; set; }
        public string Token { get; set; }
        public BusinessmanAccountUserDto User { get; set; }
    }
}
