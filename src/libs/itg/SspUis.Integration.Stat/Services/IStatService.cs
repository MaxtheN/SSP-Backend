using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.Integration.Stat.AuthModels;
using StatusGeneric;

namespace SspUis.Integration.Stat.Services
{
	public interface IStatService : IStatusGeneric
	{
		Task<StatRespone> ImportAcquisition(StatRequest statRequest);
	}
}
