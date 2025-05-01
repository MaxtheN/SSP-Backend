using SspUis.WebApi;

namespace Microsoft.AspNetCore.Builder;

public static class SwaggerAppBuilderExtentions
{
    public static void ConfigureSwagger(this IApplicationBuilder app)
    {
        if (AppSettings.Instance.Swagger.Enabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (AppSettings.Instance.System.IsLocalHost)
                {
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                }
            });
        }
    }
}
