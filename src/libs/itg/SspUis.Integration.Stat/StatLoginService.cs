using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OpenXmlPowerTools;
using SspUis.Integration.Stat.AuthModels;
using SspUis.Integration.Stat.Configs;
using StatusGeneric;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SspUis.Integration.Stat
{
	public class StatLoginService : StatusGenericHandler, IStatLoginService
	{
		private readonly StatConfig _statConfig;
		private readonly HttpClient _httpClient;

		public StatLoginService(HttpClient httpClient, StatConfig statConfig)
		{
			_httpClient = httpClient;
			_statConfig = statConfig;
			Initialize(_statConfig);
		}

		public void Initialize(StatConfig statConfig)
		{
		

			if (statConfig.UseProxy)
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", statConfig.BasicToken);
				_httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {statConfig.BasicToken}");
			}
			else
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", statConfig.AccessToken);

		}

		public async Task<string> StatAuthLoginCreate()
		{
			try
			{
				string url = $"{_statConfig.Api}/integration/token/";

				var values = new List<KeyValuePair<string, string>>()
				{
					new KeyValuePair<string, string>("client_id", _statConfig.ClientId),
					new KeyValuePair<string, string>("client_secret", _statConfig.ClientSecret)
				};

				var data = new FormUrlEncodedContent(values);
				data.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
				var response =  _httpClient.PostAsync(url, data).Result;

				try
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					if (!response.IsSuccessStatusCode)
					{
						AddError("$responseda hatolik StatAuthLoginCreate", "Message");
						return null;
					}
					else
					{
						var result = JsonConvert.DeserializeObject<StatAuthCreateResponseDto>(responseContent);
						if (result != null) 
						{ 
							return result.access_token.ToString();
						}
					}
				}
				catch (Exception)
				{
					AddError($"The object can not be deserialized or another error, status: {response.StatusCode}", "Message");
				}
			}
			catch (Exception ex)
			{
				AddError($"Произошла ошибка при получении данных из (sud) {ex.Message}: {ex.InnerException}", "Message");
			}

			return null!;
		}
	}
}


//curl - X POST http://192.168.1.10:7010/integration/token/ \
//-H "Content-Type: application/json" \
//-d '{

//	"client_id": "7710f3b0-cb5f-4de5-a16c-bf6b1251464d", 
//    "client_secret": "541a74fc-76ba-4753-85a8-8e88c96b3572"
//}'