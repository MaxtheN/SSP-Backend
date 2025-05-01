using DocumentFormat.OpenXml.Office2010.ExcelAc;
using SspUis.Core.Configurations;
using SspUis.Core;
using SspUis.DataLayer;
using StatusGeneric;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;
using WEBASE;
using System.Text;
using SspUis.DataLayer.EfClasses;
using WEBASE.Utility;
using Newtonsoft.Json;
using System;
using SspUis.Integration.Finance.Models;
using System.Collections.Generic;
using Humanizer;
using SspUis.Integration.Bojxona.Models;

namespace SspUis.BizLogicLayer.IntegrationServices;

public class IntegrationService : StatusGenericHandler, IIntegrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SystemConf _systemConf;
    private HttpClient _httpClient;

    public IntegrationService(IUnitOfWork unitOfWork, SystemConf systemConf)
    {
        _unitOfWork = unitOfWork;
        _httpClient = new HttpClient();
        _systemConf = systemConf;
    }
    public async Task CheckAllIntegrations()
    {
        Console.WriteLine("Quartz job is working....");
        bool isForTestServer = _systemConf.IsTest || _systemConf.IsLocalHost;
        var apis = _unitOfWork.Context.IntegrationApiAddresses
            .Where(a =>
                a.StateId == StateIdConst.ACTIVE
                && (a.ForTestServer == isForTestServer)) // tushunishga urunib ko'r :)
            .ToList();
        foreach (var api in apis)
        {
            foreach (var header in api.Headers)
            {
                string[] headerParts = header.Split(":");
                _httpClient.DefaultRequestHeaders.Add(headerParts[0], headerParts[1]);
            }



            var entity = new IntegrationApiTestLog();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                if (api.HttpMethodCode == HttpMethodCodeConst.GET)
                    response = await _httpClient.GetAsync(api.Url);

                else if (api.HttpMethodCode == HttpMethodCodeConst.POST)
                {

                    var content = new StringContent(api.PostData, Encoding.UTF8, "application/json");
                    response = await _httpClient.PostAsync(api.Url, content);
                }
            }
            catch (HttpRequestException ex)
            {
                entity.Exception = ex.Message;
            }
            finally
            {
                entity.AddressId = api.Id;
                entity.HttpStatus = response.StatusCode.ToString();
                entity.Message = response.ReasonPhrase;
                entity.Status = response.IsSuccessStatusCode ? "Succes" : "Error";
                entity.DateAt = DateTime.Now;
                _unitOfWork.Context.IntegrationApiTestLogs.Add(entity);
                _unitOfWork.Save();
            }
        }
        return;

    }

}






