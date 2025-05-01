using SspUis.Core.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.DependencyInjection;
using WEBASE.Storage;
using Microsoft.AspNetCore.Http;

namespace SspUis
{
    public class ServiceProvider : BaseServiceProvider<IAuthService>
    {
        public static DbContext Context { get => GetService<DbContext>(); }
        public static IHttpContextAccessor HttpContextAccessor { get => GetService<IHttpContextAccessor>(); }
    }
}
