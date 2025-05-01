using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Stat.AuthModels
{
	public class StatRespone
	{
		public int Code { get; set; }
		public string Detail { get; set; }
		public DateTime Time { get; set; }
		public int Import_id { get; set; }
	}
}
