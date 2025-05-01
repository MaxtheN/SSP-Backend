using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WEBASE;

namespace SspUis.WebApi.Controllers
{
    public class WebaseController : ControllerBase
    {
        protected ControllerConfig Config { get; }

        public WebaseController()
            : this(new())
        {
        }

        public WebaseController(ControllerConfig config)
        {
            Config = config;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<SecurityInfoModel>), 200)]
        public virtual IActionResult GetSecurityInfo()
        {
            if (Config.EnableSecurityInfo)
            {
                var securityInfos = ResolveSecurityInfos();
                return Ok(securityInfos);
            }
            return StatusCode(404);
        }

        private List<SecurityInfoModel> ResolveSecurityInfos()
        {
            var securityInfos = new List<SecurityInfoModel>();

            var controllerType = GetType();
            string controllerName = controllerType.Name.Remove(controllerType.Name.IndexOf("Controller"));

            var authorizeAttributes = controllerType.GetCustomAttributes<TypeFilterAttribute>()
                                                    .Where(a => a.ImplementationType == typeof(AuthorizeFilter)
                                                             || a.ImplementationType.BaseType == typeof(AuthorizeFilter));
            var hasAuthorizeInController = authorizeAttributes.Any();
            var contollerModuleCodes = authorizeAttributes.Where(a => a.Arguments != null && a.Arguments.Any())
                                                          .SelectMany(a => a.Arguments.FirstOrDefault() as string[])
                                                          .Distinct();

            var actions = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                        .Where(m => !m.IsSpecialName
                                                  && m.DeclaringType != typeof(ControllerBase)
                                                  && m.DeclaringType != typeof(object));

            SecurityInfoModel securityInfo;
            HttpMethodAttribute methodAttribute;
            bool hasAuthorizeInAction, hasAllowAnonymous;
            foreach (var action in actions)
            {
                methodAttribute = action.GetCustomAttribute<HttpMethodAttribute>();
                authorizeAttributes = action.GetCustomAttributes<TypeFilterAttribute>()
                                            .Where(a => a.ImplementationType == typeof(AuthorizeFilter)
                                                     || a.ImplementationType.BaseType == typeof(AuthorizeFilter));
                hasAuthorizeInAction = authorizeAttributes.Any();
                hasAllowAnonymous = action.GetCustomAttributes<AllowAnonymousAttribute>().Any();

                securityInfo = new SecurityInfoModel
                {
                    Action = $"/{controllerName}/{action.Name}",
                    Method = methodAttribute.HttpMethods.FirstOrDefault("GET"),
                    UnauthorizedAccess = hasAllowAnonymous || !hasAuthorizeInAction && !hasAuthorizeInController,
                    ModuleCodes = authorizeAttributes.Where(a => a.Arguments != null && a.Arguments.Any())
                                                     .SelectMany(a => a.Arguments.FirstOrDefault() as string[])
                                                     .Distinct()
                                                     .ToHashSet()
                };

                if (!methodAttribute.Template.NullOrEmpty())
                {
                    securityInfo.Action += $"/{methodAttribute.Template}";
                }

                foreach (var moduleCode in contollerModuleCodes)
                {
                    securityInfo.ModuleCodes.Add(moduleCode);
                }

                securityInfos.Add(securityInfo);
            }
            return securityInfos.OrderBy(x => x.Action).ToList();
        }
    }
}
