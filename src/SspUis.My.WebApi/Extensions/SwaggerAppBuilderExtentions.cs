using SspUis.My.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerAppBuilderExtentions
    {
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            if (AppSettings.Instance.Swagger.Enabled)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                });
            }
        }
    }
}
