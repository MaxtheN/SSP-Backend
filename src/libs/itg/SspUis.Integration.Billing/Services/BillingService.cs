using Newtonsoft.Json;
using SspUis.Integration.Billing.Configs;
using SspUis.Integration.Billing.Models;
using StatusGeneric;
using System.Net.Http.Headers;

namespace SspUis.Integration.Billing.Services;

public class BillingService : StatusGenericHandler, IBillingService
{
    private readonly BillingConfig _config;

    private HttpClient _httpClient;
    public BillingService(BillingConfig config)
    {
        _httpClient = new HttpClient();
        _config = config;
        Initialize(config);
    }
    public void Initialize(BillingConfig config)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicTokenClient);
    }
    public async Task<string> CreateDualApplication(DualApplicationCreateDto1 dto)
    {
        try
        {
            var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

            var url = $"{_config.api}/DualAppliaction/Create";

            var content = new StringContent(dtoJson, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"Ma'lumot saqlashda xatolik!!!: {response.ReasonPhrase}";
                try
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    error += $", Serverdan qaytgan xato: {errorContent}";
                }
                catch (Exception innerEx)
                {
                    error += $", Xato o'qish jarayonida xato: {innerEx.Message}";
                }
                AddError(error);
                return error;
            }

            return "Success created";
        }
        catch (Exception ex)
        {
            AddError($"Ma'lumot saqlashda xatolik!!! {ex.Message}: {ex.InnerException}");
            return "Error creating!!!";
        }
    }
    public async Task<byte[]> DownloadContract(Guid fileId)
    {
        try
        {
            var url = _config.api + $"/DualContract/PrintContractPdf/{fileId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                return fileBytes;
            }
            else
                AddError($"Edoc. Response:  {response}");
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (Edoc) {ex.Message}: {ex.InnerException}");
        }
        return null;
    }
    public async Task<List<UniversityListDto>> GetUniversityList()
    {
        try
        {
            var url = $"{_config.api}/DualAppliaction/GetListOrganizations";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"Universitet ro'yxatini olishda xatolik!!!: {response.ReasonPhrase}";
                try
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    error += $", Serverdan qaytgan xato: {errorContent}";
                }
                catch (Exception innerEx)
                {
                    error += $", Xato o'qish jarayonida xato: {innerEx.Message}";
                }
                AddError(error);
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var universityList = JsonConvert.DeserializeObject<List<UniversityListDto>>(jsonResponse);

            return universityList;
        }
        catch (Exception ex)
        {
            AddError($"Universitet ro'yxatini olishda xatolik!!! {ex.Message}: {ex.InnerException}");
            return null;
        }
    }
    public async Task<SpecialityListDto> GetSpecialityList(int organizationId)
    {
        try
        {
            var url = $"{_config.api}/DualAppliaction/GetListSpeciality/{organizationId}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"Mutaxassislik ro'yxatini olishda xatolik!!!: {response.ReasonPhrase}";
                try
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    error += $", Serverdan qaytgan xato: {errorContent}";
                }
                catch (Exception innerEx)
                {
                    error += $", Xato o'qish jarayonida xato: {innerEx.Message}";
                }
                AddError(error);
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var specialityList = JsonConvert.DeserializeObject<SpecialityListDto>(jsonResponse);

            return specialityList;
        }
        catch (Exception ex)
        {
            AddError($"Mutaxassislik ro'yxatini olishda xatolik!!! {ex.Message}: {ex.InnerException}");
            return null;
        }
    }
    public async Task<UniversityGetDto> GetUniversity(int organizationId)
    {
        try
        {
            var url = $"{_config.api}/DualAppliaction/GetOrganization/{organizationId}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"Universitet ma'lumotini olishda xatolik!!!: {response.ReasonPhrase}";
                try
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    error += $", Serverdan qaytgan xato: {errorContent}";
                }
                catch (Exception innerEx)
                {
                    error += $", Xato o'qish jarayonida xato: {innerEx.Message}";
                }
                AddError(error);
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var university = JsonConvert.DeserializeObject<UniversityGetDto>(jsonResponse);

            return university;
        }
        catch (Exception ex)
        {
            AddError($"Universitet ma'lumotini olishda xatolik!!! {ex.Message}: {ex.InnerException}");
            return null;
        }
    }
    public async Task<SpecialityGetDto> GetSpeciality(int specialityId)
    {
        try
        {
            var url = $"{_config.api}/DualAppliaction/GetSpeciality/{specialityId}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"Mutaxassislik ma'lumotini olishda xatolik!!!: {response.ReasonPhrase}";
                try
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    error += $", Serverdan qaytgan xato: {errorContent}";
                }
                catch (Exception innerEx)
                {
                    error += $", Xato o'qish jarayonida xato: {innerEx.Message}";
                }
                AddError(error);
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var speciality = JsonConvert.DeserializeObject<SpecialityGetDto>(jsonResponse);

            return speciality;
        }
        catch (Exception ex)
        {
            AddError($"Mutaxassislik ma'lumotini olishda xatolik!!! {ex.Message}: {ex.InnerException}");
            return null;
        }
    }
    public async Task<int?> HeldByContractor(int id)
    {
        try
        {
            var url = $"{_config.api}/DualContract/HeldByContractor?id={id}";

            var response = await _httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                string error = $"HeldByContractor API'da xatolik: {response.ReasonPhrase}";
                try
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    error += $", Serverdan qaytgan xato: {errorContent}";
                }
                catch (Exception innerEx)
                {
                    error += $", Xato o'qish jarayonida xato: {innerEx.Message}";
                }
                AddError(error);
                return null;
            }

            var resultString = await response.Content.ReadAsStringAsync();
            dynamic resultJson = JsonConvert.DeserializeObject(resultString);
            if (resultJson != null && resultJson.id != null)
            {
                return (int)resultJson.id;
            }
            else
            {
                AddError("Javobda `id` maydoni topilmadi.");
                return null;
            }
        }
        catch (Exception ex)
        {
            AddError($"HeldByContractor API chaqiruvida xatolik: {ex.Message}: {ex.InnerException}");
            return null;
        }
    }

}