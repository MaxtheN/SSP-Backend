using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Job.WebApi.Hangfire.JobServices
{
    public interface IHangfireService<TParameter>
        where TParameter : BaseHangfireParameter
    {
        Task RunAsync(TParameter parameters, CancellationToken cancellationToken);
    }

    public class BaseHangfireParameter
    {
        public string UserName { get; set; }
    }


}
