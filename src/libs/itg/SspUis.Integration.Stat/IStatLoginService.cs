using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusGeneric;

namespace SspUis.Integration.Stat
{
	public interface IStatLoginService : IStatusGeneric
	{
		Task<string> StatAuthLoginCreate();
	}
}
