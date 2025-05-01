using SspUis.Job.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class CorsAppBuilderExtentions
    {
        public static void ConfigureCors(this IApplicationBuilder app)
        {
            if (AppSettings.Instance.Cors.UseCors)
                app.UseCors("AllowedOrgins");
            else
                app.UseCors("AllowAll");
        }
    }
}
