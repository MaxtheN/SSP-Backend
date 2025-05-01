using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Stat.Configs
{
	public class StatConfig
	{
		public string Api { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		internal string BasicTokenClient { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{ClientId}:{ClientSecret}")); } }
		public bool UseProxy { get; set; }
		public string Login { get; set; }
		public string Pswd { get; set; }
		internal string AccessToken { get; set; } = null!;
		internal string BasicToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Login}:{Pswd}")); } }
	}
}
