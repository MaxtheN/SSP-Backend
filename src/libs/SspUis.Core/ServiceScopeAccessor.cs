using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SspUis.Core
{
    public interface IServiceScopeAccessor
    {
        IServiceScope? Scope { get; set; }
    }

    public class ServiceScopeAccessor : IServiceScopeAccessor
    {
        private static readonly AsyncLocal<ScopeHolder> _scopeCurrent = new AsyncLocal<ScopeHolder>();

        public IServiceScope? Scope
        {
            get
            {
                return _scopeCurrent.Value?.Context;
            }
            set
            {
                var holder = _scopeCurrent.Value;
                if (holder != null)
                {
                    // Clear current Scope trapped in the AsyncLocals, as its done.
                    holder.Context = null;
                }

                if (value != null)
                {
                    // Use an object indirection to hold the Scope in the AsyncLocal,
                    // so it can be cleared in all ExecutionContexts when its cleared.
                    _scopeCurrent.Value = new ScopeHolder { Context = value };
                }
            }
        }

        private sealed class ScopeHolder
        {
            public IServiceScope? Context;
        }
    }
}
