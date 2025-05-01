using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Stat.AuthModels
{
	public class StatAuthCreateResponseDto
	{
		public string token {  get; set; }
		public string access_token { get; set; }
		public string token_type { get; set; }
		public long expires_in { get; set; }
	}
}
