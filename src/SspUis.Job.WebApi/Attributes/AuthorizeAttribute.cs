using SspUis.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.AspNet.Security;

namespace SspUis.Job.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute()
            : base(typeof(AuthorizeFilter))
        {
            Arguments = new[] { new string[] { } };
        }

        public AuthorizeAttribute(params ModuleCode[] moduleCodes)
            : base(typeof(AuthorizeFilter))
        {
            Arguments = new[] { moduleCodes.Select(c => c.ToString()).ToArray() };
        }
    }
}
