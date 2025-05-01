using Microsoft.AspNetCore.Mvc.Filters;

namespace SspUis.WebApi;

[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute, IFilterMetadata
{
}
