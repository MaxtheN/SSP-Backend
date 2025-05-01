using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Extensions
{
    public static class HttpContectExtensions
    {
        public static string GetUserIP(this HttpContext httpContext)
        {
            string userip = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (userip == "127.0.0.1")
                userip = "";
            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                userip += (!string.IsNullOrEmpty(userip) ? ";" : "") + httpContext.Request.Headers["X-Forwarded-For"];
            return userip;
        }
    }
}
