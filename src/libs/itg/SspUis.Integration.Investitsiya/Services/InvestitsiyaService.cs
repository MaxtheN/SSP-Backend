using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.Integration.Investitsiya.Configs;
using SspUis.Integration.Investitsiya.Models;
using StatusGeneric;
using System.Net.Http.Headers;
using WEBASE.EF;

namespace SspUis.Integration.Investitsiya.Services
{
    public class InvestitsiyaService : StatusGenericHandler, IInvestitsiyaService
    {
        private readonly InvestitsiyaConfig _config;
        private HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public InvestitsiyaService(HttpClient httpClient, InvestitsiyaConfig config, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _config = config;
            Initialize(config);
            _unitOfWork = unitOfWork;
        }
        public void Initialize(InvestitsiyaConfig config)
        {
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.Basictoken);
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicTokenClient}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicTokenClient);
        }

        public async Task<List<InvestmentContract>> GetInvestmentContracts(InvestitsiyaRequestDto dto)
        {
            string docDateFrom = dto.DocDateFrom == null ? "" : $"&docDateFrom={dto.DocDateFrom}";
            string docDateTo = dto.DocDateTo == null ? "" : $"&docDateTo={dto.DocDateTo}";
            //string cntrStatus = dto.CntrStatus == null ? "" : $"&cntrStatus={dto.CntrStatus}";
            //string cntrtype = dto.CntrType == null ? "" : $"&cntrType={dto.CntrType}";

            var url = $"{_config.Api}/yeisvo_integration_service/api/contract/list?contractorUzInn={dto.ContractorUzInn}";
            url += docDateFrom;
            url += docDateTo;
            //url += cntrtype;
            //url += cntrStatus;
            
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Инвестиция тизимидан маълумот олишда хатолик рўй берди! {dto.ContractorUzInn}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Инвестиция тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            InvestitsiyaResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<InvestitsiyaResponseDto>(responseJson, new JsonSerializerSettings { })!;

                var cntrTypes =  await GetInvestmentContractTypes();
                var cntrAgreementStates =  await GetInvestmentAgreementStates();
                var crtnSubjects = await GetCntrSubject();
                var banks = _unitOfWork.Context.Banks;
                var countries = _unitOfWork.Context.Countries;
                var curriencies = _unitOfWork.Context.Currencys;

                foreach(var item in data.ContractList)
                {
                    var bank = banks.FirstOrDefault(a => a.Code == item.BankId);
                    var country = countries.FirstOrDefault(x => x.Code == item.ContractorForCountryCode);
                    var currency1 = curriencies.FirstOrDefault(x => x.Code == item.CurrCode1);
                    var currency2 = curriencies.FirstOrDefault(x => x.Code == item.CurrCode2);
                    if (item.CntrType != null && item.CntrStatus != null)
                    {
                        item.CntrTypeName = cntrTypes.FirstOrDefault(x => x.Id == item.CntrType)?.Name;
                        item.CntrStatusName = cntrAgreementStates.FirstOrDefault(x => x.Id == item.CntrStatus)?.Name;
                        item.ContractSubject = crtnSubjects.FirstOrDefault(x => x.Id == item.CntrSubject)?.Name;
                        item.BankName = bank.BankName;
                        item.ContractorCountry = country.FullName;
                        item.CurrencyCodeName1 = currency1?.FullName;
                        item.CurrencyCodeName2 = currency2?.FullName;
                    }
                }
            }
            catch
            {
                throw;
            }
            return data.ContractList;
        }

        public async Task<List<InvestitsiyaCntrTypeRecord>> GetInvestmentContractTypes()
        {
            var url = $"{_config.Api}/yeisvo_integration_service/api/dic/contract_type";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Инвестиция тизимидан маълумот олишда хатолик рўй берди!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Инвестиция тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            InvestitsiyaCntrResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<InvestitsiyaCntrResponseDto>(responseJson, new JsonSerializerSettings { })!;
            }
            catch
            {
                throw;
            }
            if (data.Records == null || data.Records.Count == 0)
            {
                AddError($"Инвестиция тизимидан маълумот топилмади! {data.Records} ({data.ResultNote})");
                return null!;
            }
            return data.Records;

        }

        public async Task<List<InvestitsiyaAgreementStates>> GetInvestmentAgreementStates()
        {
            var url = $"{_config.Api}/yeisvo_integration_service/api/dic/agreement_state";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Инвестиция тизимидан маълумот олишда хатолик рўй берди!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Инвестиция тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            InvestitsiyaAgreementStateResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<InvestitsiyaAgreementStateResponseDto>(responseJson, new JsonSerializerSettings { })!;
            }
            catch
            {
                throw;
            }
            if (data.Records == null || data.Records.Count == 0)
            {
                AddError($"Инвестиция тизимидан маълумот топилмади! {data.Records} ({data.ResultNote})");
                return null!;
            }
            return data.Records;
        }

        public async Task<List<CntrSubjectDto>> GetCntrSubject()
        {
            var url = $"{_config.Api}/yeisvo_integration_service/api/dic/contract_subject";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Инвестиция тизимидан маълумот олишда хатолик рўй берди!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Инвестиция тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            InvestitsiyaCntrSubjectResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<InvestitsiyaCntrSubjectResponseDto>(responseJson, new JsonSerializerSettings { })!;
            }
            catch
            {
                throw;
            }
            if (data.Records == null || data.Records.Count == 0)
            {
                AddError($"Инвестиция тизимидан маълумот топилмади! {data.Records} ({data.ResultNote})");
                return null!;
            }
            return data.Records;
        }
    }
}