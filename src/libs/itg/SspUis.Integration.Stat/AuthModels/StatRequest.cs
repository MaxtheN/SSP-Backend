using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Stat.AuthModels
{
	public class StatRequest
	{
		public string inn { get; set; }
		public  string contractorname { get; set; }
		public string docNumber { get; set; }
		public DateOnly expireOn { get; set; }
		public string telNumber { get; set; }
	}
}
