using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SspUis.DataLayer;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;

namespace SspUis.Integration.Soliq
{
    public class SoliqContractorService : StatusGenericHandler, IDisposable, ISoliqContractorService
    {
        private readonly SoliqConfig _config;
        private readonly IUnitOfWork _unitOfWork;

        private HttpClient _httpClient;
        public SoliqContractorService(SoliqConfig config, IUnitOfWork unitOfWork)
        {
            _httpClient = new HttpClient();
			_httpClient.Timeout = TimeSpan.FromMinutes(3);
			_config = config;
            Initialize(config);
            _unitOfWork = unitOfWork;
		}
        
        public void Initialize(SoliqConfig config)
        {
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.basictoken);
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicTokenClient}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicTokenClient);
        }

        public async Task<SoliqContractorByTinDto> GetByInn(string inn)
        {
            var url = $"{_config.api}/hunarmand/api/get/yur-info?tin={inn}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! {inn}";
                try
                {
                    error += await response.Content.ReadAsStringAsync();
                }
                catch { }
                AddError(error);
                //AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            SoliqContractorByTinResponseDto contractor;
            try
            {
                contractor = JsonConvert.DeserializeObject<SoliqContractorByTinResponseDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (!contractor.Success)
            {
                //AddError($"Солиқ тизимидан маълумот топилмади! {inn} ({contractor.Message})");
                return null!;
            }
            return contractor.Data;
        }
        public async Task<SoliqContractorDebtByPinflDto> GetByPinfl(string pinfl)
        {
            var url = $"{_config.api}/hunarmand/api/get/fiz-info?pinfl={pinfl}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! {pinfl}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                //AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            SoliqContractorByPinflResponseDto contractor;
            try
            {
                contractor = JsonConvert.DeserializeObject<SoliqContractorByPinflResponseDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (!contractor.Success)
            {
                //AddError($"Солиқ тизимидан маълумот топилмади! {inn} ({contractor.Message})");
                return null!;
            }
            return contractor.Data.Data;
        }
        public async Task<SoliqContractorEmployeeCountByTinDataDto> GetEmployeeCountByInn(string inn, int year, int month)
        {
            var url = $"{_config.api}/trade-api/api/company/get-company-finance-data?tin={inn}&year={year}&month={month}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! {inn}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            SoliqContractorEmployeeCountByTinDto data;
            try
            {
                data = JsonConvert.DeserializeObject<SoliqContractorEmployeeCountByTinDto>(responceJson)!;
                data.Data.StatusCode = response.StatusCode;
            }
            catch
            {
                throw;
            }

            if (!data.Success)
            {
                //AddError($"Солиқ тизимидан маълумот топилмади! {inn} ({contractor.Message})");
                return null!;
            }
            return data.Data;
        }
        public async Task<SoliqContractorDebtByTinDataDto> GetDebtByInn(string inn, int year)
        {
            var url = $"{_config.api}/trade-api/api/company/get-company-tax-debt?tin={inn}&year={year}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! {inn}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            SoliqContractorDebtByTinDto data;
            try
            {
                data = JsonConvert.DeserializeObject<SoliqContractorDebtByTinDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (!data.Success)
            {
                AddError($"Солиқ тизимидан маълумот топилмади! {inn} ({data.Message})");
                return null!;
            }
            return data.Data;
        }
        public async Task<SoliqContractorFinanceBenefitByTinDataDto> GetFinanceBenefitByInn(string inn, int year, int period)
        {
            try
            {
                var url = $"{_config.api}/trade-api/api/company/get-finance-benefit-damage-report?tin={inn}&year={year}&period={period}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! {inn}";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                    return null!;
                }
                var responceJson = await response.Content.ReadAsStringAsync();
                SoliqContractorFinanceBenefitByTinDto data;
                try
                {
                    data = JsonConvert.DeserializeObject<SoliqContractorFinanceBenefitByTinDto>(responceJson)!;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (!data.Success)
                {
                    //AddError($"Солиқ тизимидан маълумот топилмади! {inn} ({contractor.Message})");
                    return null!;
                }
                if (data.Data.NetIncome == null)
                {
                    data.Data.NetIncome = 0;
                }
                return data.Data;
            }
            catch (Exception ex)
            {
                AddError($"Soliq: {ex.Message}: {ex.InnerException}");
            }
            return null;
        }
        public async Task<FarmerRefundByInnDataDto> GetFarmerRefundByInn(string inn, int year)
        {
            var url = $"{_config.api}/trade-api/test/company/get-company-farmer-refund?tin={inn}&year={year}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! {inn}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"{response.StatusCode} Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            FarmerRefundByInnResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<FarmerRefundByInnResponseDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (!data.Success)
            {
                AddError($"Солиқ тизимидан маълумот топилмади! {inn} ({data.Reason})");
                return null!;
            }
            return data.Data;
        }
        public async Task<List<ImtiyozDataByInnDataDto>> GetImtiyozDataByInn(string inn, int year)
        {
            var url = $"{_config.api}/trade-api/test/company/get-imtiyoz-data?tin={inn}&year={year}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! {inn}";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            ImtiyozDataByInnResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<ImtiyozDataByInnResponseDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (data.Data.Count == 0 || data.Data == null)
            {
                AddError($"Солиқ тизимидан маълумот топилмади! {inn} ({data.Reason})");
                return null!;
            }
            return data.Data;
        }
        public async Task<CompanyInfo> GetCompanyCriteries(string tin)
        {
            var url = $"{_config.api}/trade-api/api/company/get-company-criteries-info?tin={tin}";

				try
				{
					var response = await _httpClient.GetAsync(url);

					if (!response.IsSuccessStatusCode)
					{
						string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! {tin}";
						try
						{
							error = await response.Content.ReadAsStringAsync();
						}
						catch { }

						AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
						return null!;
					}

					var responceJson = await response.Content.ReadAsStringAsync();
					CompanyCriteriesInfoResponseDto data;

					try
					{
						data = JsonConvert.DeserializeObject<CompanyCriteriesInfoResponseDto>(responceJson)!;

						if (data?.data == null)
						{
							AddError($"Солиқ тизимидан маълумот топилмади! {tin} ({data.reason})");
							return null!;
						}

						var org = await GetByInn(tin);

						if (org == null)
						{
							AddError($"Солиқ тизимидан маълумот топилмади! {tin} ({data.reason})");
							return null!;
						}

						if (org != null)
						{
							data.data.CompanyInfo = new CompanyInfo()
							{
								OkedDetail = org.Company.OkedDetail,
								districtID = org.CompanyBillingAddress.District.DistrictId,
								regionID = org.CompanyBillingAddress.District.RegionId,
								regionNameUz = org.CompanyBillingAddress.Region.NameUzCyrl,
								regionNameLat = org.CompanyBillingAddress.Region.NameUzLatn,
								regionNameRu = org.CompanyBillingAddress.Region.NameRu,
								districtNameLat = org.CompanyBillingAddress.District.NameUzLatn,
								districtNameRu = org.CompanyBillingAddress.District.NameRu,
								districtNameUz = org.CompanyBillingAddress.District.Name,
								tin = tin,
								name = org.Company.Name,
								nameLat = org.Company.Name,
								nameRu = org.Company.Name,
								nameUz = org.Company.Name,
								criteriaAll = data.data.criteriaAll,
								taxpayerType = org.Company.TaxpayerType,
								type = data.data.type
							};
						}

						if (data?.data != null)
						{
							if (data.data.CompanyInfo.taxpayerType == 3)
							{
								data.data.CompanyInfo.taxpayername = "ISTIFLT";
								data.data.CompanyInfo.taxpayer_name_uz_latn = "YSTBHDSI";
								data.data.CompanyInfo.taxpayername_uz_cyrl = "ЙСТБХДСИ";
								data.data.CompanyInfo.taxpayername_ru = "МРИПКН";
							}
							else if (data.data.CompanyInfo.taxpayerType == 2)
							{
								data.data.CompanyInfo.taxpayername = "SDT";
								data.data.CompanyInfo.taxpayer_name_uz_latn = "DSB";
								data.data.CompanyInfo.taxpayername_uz_cyrl = "ДСБ";
								data.data.CompanyInfo.taxpayername_ru = "ГНУ";
							}
							else if (data.data.CompanyInfo.taxpayerType == 1)
							{
								data.data.CompanyInfo.taxpayername = "STI";
								data.data.CompanyInfo.taxpayer_name_uz_latn = "DSI";
								data.data.CompanyInfo.taxpayername_uz_cyrl = "ДСИ";
								data.data.CompanyInfo.taxpayername_ru = "ГНИ";
							}
						}

						return data.data.CompanyInfo;
					}
					catch (JsonException jsonEx)
					{
						AddError($"JSONni deserializatsiya qilishda xato: {jsonEx.Message}");
						return null!;
					}
					catch (TaskCanceledException ex)
					{
						AddError($"So'rov vaqti tugadi: {ex.Message}");
						return null!;
					}
					catch (Exception ex)
					{
						AddError($"Boshqa xato: {ex.Message}");
						return null!;
					}
				}
				catch (TaskCanceledException ex)
				{
					AddError($"So'rov vaqti tugadi (global): {ex.Message}");
					return null!;
				}
				catch (Exception ex)
				{
					AddError($"Boshqa xato: {ex.Message}");
					return null!;
				}
           
        }


        public async Task<List<GetCompanyHighNewInfo>> GetCompanyHighNewInfo(int isBusiness, int ns10Code, int ns11Code)
        {
            return null;
            //var url = $"{_config.api}/trade-api/api/company/get-company-high-new-info?isBusiness={isBusiness}&ns10Code={ns10Code}&ns11Code={ns11Code}";

            //var response = await _httpClient.GetAsync(url);
            //if (!response.IsSuccessStatusCode)
            //{
            //    string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди! ";
            //    try
            //    {
            //        error = await response.Content.ReadAsStringAsync();
            //    }
            //    catch { }

            //    AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
            //    return null!;
            //}
            //var responceJson = await response.Content.ReadAsStringAsync();
            //GetCompanyHighNewInfoResponseDto data;
            //try
            //{
            //    data = JsonConvert.DeserializeObject<GetCompanyHighNewInfoResponseDto>(responceJson)!;
            //}
            //catch
            //{
            //    throw;
            //}

            //if (data.Data.Count == 0 || data.Data == null)
            //{
            //    AddError($"Солиқ тизимидан маълумот топилмади! ({data.Reason})");
            //    return null!;
            //}
            //return data.Data;
        }
        public async Task<List<CompanyStateNewInfoData>> GetCompanyStateNewInfoData(int isBusiness, int ns10Code, int ns11Code)
        {
            return null;
            var url = $"{_config.api}/trade-api/api/company/get-company-stat-new-info?isBusiness={isBusiness}&ns10Code={ns10Code}&ns11Code={ns11Code}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            CompanyStateNewInfoResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<CompanyStateNewInfoResponseDto>(responceJson)!;
            }
            catch
            {
                throw;
            }

            if (data.Data.Count == 0 || data.Data == null)
            {
                AddError($"Солиқ тизимидан маълумот топилмади!  ({data.Reason})");
                return null!;
            }
            return data.Data;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null!;
        }

        public async Task<SoliqQqsAylanmaData> GetQqsAylanmaData(int month, long inn, int year)
        {

            var url = $"{_config.api}/trade-api/api/company/get-absolute-income-tax-report?period={month}&tin={inn}&year={year}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string responseContent = string.Empty;
                try
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                }
                catch
                {
                    responseContent = "Response content is empty";
                }
                throw new DataNotFoundInSoliqException($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: ReasonPhrase: {response.ReasonPhrase} ResponseContent: {responseContent}");
            }

            var responceJson = await response.Content.ReadAsStringAsync();
            SoliqQqsAylanmaResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<SoliqQqsAylanmaResponseDto>(responceJson, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                })!;

                if (data.Data == null)
                {
                    return null!;
                }
            }
            catch (Exception ex)
            {
                throw new ResponseConvertToModelException(ex.Message);
            }

            return data.Data;
        }

        public async Task<SoliqAosAylanmaData> GetAosAylanmaData(int inn, int year, int? month = null)
        {
            var url = $"{_config.api}/trade-api/api/company/get-from-aos-tax-income-sum?tin={inn}&year={year}";

            if (month != null)
            {
                url = $"{_config.api}/trade-api/api/company/get-from-aos-tax-income-sum?period={month}&tin={inn}&year={year}";

            }

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string responseContent = string.Empty;
                try
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                }
                catch
                {
                    responseContent = "Response content is empty";
                }
                throw new DataNotFoundInSoliqException($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: ReasonPhrase: {response.ReasonPhrase} ResponseContent: {responseContent}");
            }

            var responceJson = await response.Content.ReadAsStringAsync();
            SoliqAosAylanmaResponseDto data;
            try
            {
                data = JsonConvert.DeserializeObject<SoliqAosAylanmaResponseDto>(responceJson)!;

                if (data.Data == null || !data.Success)
                {
                    return null!;
                }
            }
            catch (Exception ex)
            {
                throw new ResponseConvertToModelException(ex.Message);
            }

            return data.Data;
        }

        public async Task<SoliqImtiyozResponseDto> GetSoliqImtiyozlari(decimal STIR, int year)
        {
            var url = $"{_config.api}/trade-api/service/ministry-trade/get/vat-reimbursed-info?tin={STIR}&year={year}";
            //https://proxy.chamber.uz/http://192.168.1.10:7000/trade-api/service/ministry-trade/get/vat-reimbursed-info?tin=206611883&year=2024

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = $"Солиқ тизимидан маълумот олишда хатолик рўй берди!";
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch { }

                AddError($"Солиқ тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                return null!;
            }
            var responceJson = await response.Content.ReadAsStringAsync();
            SoliqImtiyozResponseDto data = new SoliqImtiyozResponseDto();

            try
            {
                 data = JsonConvert.DeserializeObject<SoliqImtiyozResponseDto>(responceJson)!;
                if (data.Data == null)
                {
                    AddError($"Солиқ тизимидан маълумот топилмади!  ({data.Reason})");
                    return null!;
                }
            }
            catch
            {
                throw;
            }

            return  data;

        }
    }
    public class DataNotFoundInSoliqException : Exception
    {
        public DataNotFoundInSoliqException(string message) : base(message) { }
    }

    public class ResponseConvertToModelException : Exception
    {
        public ResponseConvertToModelException(string message) : base(message) { }
    }
}
