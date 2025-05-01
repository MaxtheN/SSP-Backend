using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using System;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using System.Linq;

namespace SspUis.Job.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AccessServiceAttribute : ActionFilterAttribute
    {
        private readonly string _serviceCode;
        private readonly IServiceProvider _serviceProvider;

        public AccessServiceAttribute(string serviceCode)
        {
            _serviceCode = serviceCode;
            _serviceProvider = null;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsServiceAccessible(context))
            {
                context.Result = new BadRequestObjectResult("Ushbu xizmat vaqtinchalik ishlamayapti!");
            }

            base.OnActionExecuting(context);
        }

        private bool IsServiceAccessible(ActionExecutingContext context)
        {
            var unitOfWork = (IUnitOfWork)context.HttpContext.RequestServices.GetService(typeof(IUnitOfWork));
            var service = unitOfWork.Context.Set<Accessibility>().FirstOrDefault(x => x.Code == _serviceCode);

            return service != null ? service.HasAccess : false;
        }
    }

}
