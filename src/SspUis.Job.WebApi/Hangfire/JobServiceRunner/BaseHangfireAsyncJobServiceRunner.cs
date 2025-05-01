using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE;
using SspUis.Core;
using SspUis.Core.Security;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using WEBASE.DependencyInjection;

namespace SspUis.Job.WebApi.Hangfire.JobServices
{
    public abstract class BaseHangfireAsyncJobServiceRunner<TService, TParameter>
        where TService : IHangfireService<TParameter>
        where TParameter : BaseHangfireParameter
    {
        private readonly IServiceScopeAccessor _serviceScopeAccessor;
        private readonly IServiceProvider _serviceProvider;

        public BaseHangfireAsyncJobServiceRunner(
            IServiceScopeAccessor serviceScopeAccessor,
            IServiceProvider serviceProvider)
        {
            _serviceScopeAccessor = serviceScopeAccessor;
            _serviceProvider = serviceProvider;
        }

        public virtual async Task ExecuteAsync(TParameter parameters, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _serviceScopeAccessor.Scope = scope;
                try
                {
                    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                    if (parameters?.UserName?.NullOrEmpty() == false)
                        authService.ResetUserName(parameters.UserName);

                    var service = scope.ServiceProvider.GetRequiredService<TService>();
                    await service.RunAsync(parameters, cancellationToken);
                }
                finally
                {
                    _serviceScopeAccessor.Scope = null;
                }
            }
        }

    }
}
