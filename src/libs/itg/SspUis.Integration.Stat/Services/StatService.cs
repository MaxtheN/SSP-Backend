using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.Integration.Stat.AuthModels;
using SspUis.Integration.Stat.Configs;
using StatusGeneric;

namespace SspUis.Integration.Stat.Services
{
	public class StatService : StatusGenericHandler, IStatService
	{
		private readonly IStatLoginService _statLogin;
		private readonly HttpClient _httpClient;
		private readonly StatConfig _statConfig;
		private readonly IUnitOfWork _unitOfWork;


        public StatService(IStatLoginService statLoginService, HttpClient httpClient, StatConfig statConfig, IUnitOfWork unitOfWork)
		{
			_statLogin = statLoginService;
			_httpClient = httpClient;
			_statConfig = statConfig;
			_unitOfWork = unitOfWork;
			Initialize(statConfig);
		}

		private void Initialize(StatConfig statConfig)
		{
			if (statConfig.UseProxy)
			{
			
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", statConfig.BasicToken);
				var token = _statLogin.StatAuthLoginCreate();
				_httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Bearer {token.Result}");

			}
			else
			{ 
				 var token = _statLogin.StatAuthLoginCreate();
				_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
			}
		}

		public async Task<StatRespone> ImportAcquisition(StatRequest statRequest)
		{
			try
			{
				var dtoJson = JsonConvert.SerializeObject(statRequest);
				
				var url = $"{_statConfig.Api}/acquisition/import/";

				var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync(url, content);
				var responseContent = await response.Content.ReadAsStringAsync();
				if (!response.IsSuccessStatusCode)
				{
					AddError($"{response.IsSuccessStatusCode}, {response.Content}, {responseContent} xatolik yuz berdi", "Message");
				}
				else
				{
					var result = JsonConvert.DeserializeObject<StatRespone>(responseContent);
					if (result != null)
					{
						return result;
					}
				}
			
				return new StatRespone();

			}
			catch (Exception ex)
			{
				AddError($"Произошла ошибка при получении данных из (stat) {ex.Message}: {ex.InnerException}", "Message");
				return new StatRespone();
			}
		
		}
	}
}


//curl--location--request GET 'http://192.168.1.10:7009/e_s/api/guides/categories' \
//--header 'Justice: Token 40549772-5a1c-46b7-95e4-91cd4986e774' \
//--header 'User-Agent: insomnia/9.3.2' \
//--header 'Content-Type: application/x-www-form-urlencoded' \
//--data-urlencode 'Justice=efcbd810-1aa6-447b-8208-d63dba4173be'


//curl--location 'http://192.168.1.10:7010/acquisition/import/' \--header 'Content-Type: application/json' \
//--header 'Authorization: Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ0e                                                                                  XBlIjoiYWNjZXNzIiwianRpIjoiYTgyOTI3MzYtNTQ3NS00NDFlLWFjNDYtZDdjZDc3MzA4NmY5IiwiaXNzIjoiU0lJUyIsInN1YiI6NjU2LCJpYXQiOjE3MzI3ODU1MDcsImV4cCI6MTczMjgzODQ2MH0' \--data '{ "inn": "200000000",
// "contractorname": "Корхона номи",  "docNumber": "TVM-09-2024-0005039",
// "expireOn": "01.01.2024",  "telNumber": "+998901234567"
//}'