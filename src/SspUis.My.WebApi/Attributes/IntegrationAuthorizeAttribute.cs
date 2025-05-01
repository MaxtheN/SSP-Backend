using SspUis.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SspUis.My.WebApi
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class IntegrationAuthorizeAttribute : TypeFilterAttribute
    {
        public IntegrationAuthorizeAttribute()
            : base(typeof(IntegrationAuthorizeFilter))
        {
            Arguments = new[] { new string[] { } };
        }

        public IntegrationAuthorizeAttribute(params string[] userNames)
            : base(typeof(IntegrationAuthorizeFilter))
        {
            Arguments = new[] { userNames.Select(c => c.ToString()).ToArray() };
        }
    }
}
