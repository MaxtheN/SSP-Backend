using Newtonsoft.Json;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Sud.Configs;
using SspUis.Integration.Sud.Models;
using SspUis.Integration.Sud.Models.AuthModels;
using SspUis.Integration.Sud.Models.MalumotnomaModel;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Services;

public class SudService : StatusGenericHandler, ISudService
{
    private readonly SudConfig _sudConfig;
    private readonly ISudLoginService _sudLogin;
    private HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    public SudService(SudConfig sudConfig, ISudLoginService sudLogin, IUnitOfWork unitOfWork)
    {
        _sudConfig = sudConfig;
        _sudLogin = sudLogin;
        _httpClient = new HttpClient();
        _unitOfWork = unitOfWork;
    }
    public void Initialize(SudConfig config)
    {
        //var token = _sudLogin.SudAuthLoginCreate();
       // _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
       // _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
    }
    public async Task<object> SudSendingNewClaim(SendingNewClaimDto sendingNewClaimDto, CreateCourtntegrationDlDto dto)
    {
        try
        {
            var dtoJson = JsonConvert.SerializeObject(sendingNewClaimDto);
            var last = dtoJson.Replace("`", "");
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud SudSendingNewClaim: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }

            var url = $"{_sudConfig.api}/e_s/api/cases/create/economic/suit";
            var content = new StringContent(last, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                AddError($" {response.IsSuccessStatusCode}, {response.Content}, {responseContent} responseda xatolik SudSendingNewClaim()", "Message");
                var data =  await response.Content.ReadAsStringAsync();
            }
            var result = JsonConvert.DeserializeObject<SendingNewClaimResponseDto>(responseContent);
            if (result != null && result.statusCode == "200")
            {
                _unitOfWork.CourtntegrationRepository.Create(dto);
                _unitOfWork.Save();
            }
            return result ?? new SendingNewClaimResponseDto();
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (sud) {ex.Message}: {ex.InnerException}", "Message");
            return new SendingNewClaimResponseDto();
        }
    }
    public async Task<UploadFileResponseDto> SudUploadFile(UploadFileDto dto)
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud SudUploadFile: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/files/upload";

            var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);
             var  content = new StringContent(dtoJson, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.PostAsync(url,content);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik SudUploadFile()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<UploadFileResponseDto>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"The object can not be deserialized or another error, status: {response.StatusCode}", "Message");
                return null;
            }
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (sud) {ex.Message}: {ex.InnerException}", "Message");
            return null;
        }
        return null!;
    }

    public async Task<InvoiceResponseModel> SudInvoice(InvoiceModel invoiceModel)
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud SudInvoice: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/invoices/generate";

            var dtoJson = JsonConvert.SerializeObject(invoiceModel, Formatting.Indented);
            var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.PostAsync(url, content);

            try
            {

                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik SudUploadFile()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<InvoiceResponseModel>(responseContent);

                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Квитанция генерация қилиш da hatolik, status: {response.StatusCode}", "Message");
            }
        }
        catch (Exception ex)
        {
            AddError($"Квитанция генерация қилиш  Error (sud) {ex.Message}: {ex.InnerException}", "Message");
        }

        return null!;
    }

    #region Spravchiklar
    public async Task<List<CommonEntity>> GetSudAmountCategoryList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud GetSudAmountCategoryList: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }

            var url = $"{_sudConfig.api}/e_s/api/guides/amount-categories";

            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik SudAuthLoginCreate()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud AmountCategories : {response.StatusCode}", "Message");
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud AmountCategories  {ex.Message}: {ex.InnerException}", "Message");
        }
        return null!;
    }

    public async Task<List<CommonEntity>> GetSudCategoryList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/categories";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudCategoryList()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }

            }
            catch (Exception)
            {
                AddError($"Sud Categories : {response.StatusCode}", "Message2");
                return new List<CommonEntity>();
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Categories  {ex.Message}: {ex.InnerException}", "Message3");
        }
        return null!;
    }

    public async Task<List<CommonEntity>> GetSudCategoriesSecondList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/categories/second";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudCategoriesSecondList()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud Categories Second : {response.StatusCode}", "Message");
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Categories Second  {ex.Message}: {ex.InnerException}", "Message");
        }
        return null!;
    }

    public async Task<List<CommonEntity>> GetSudCategoriesSubList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }

            var url = $"{_sudConfig.api}/e_s/api/guides/categories/sub";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudCategoriesSubList()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud Categories sub : {response.StatusCode}", "Message");
                return new List<CommonEntity>();
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Categories sub  {ex.Message}: {ex.InnerException}", "Message");
            return null!;
        }
        return null!;
    }

    public async Task<List<string>> GetSudClaimKindList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }

            var url = $"{_sudConfig.api}/e_s/api/guides/claim-kinds";

            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Agar 204 holat kodi qaytsa, bo'sh ro'yxatni qaytarish
                return new List<string>();
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                AddError($"Sud ClaimKinds: {response.StatusCode}", "Message");
                return null!;
            }

            var result = JsonConvert.DeserializeObject<List<string>>(responseContent);
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud ClaimKinds: {ex.Message}", "Message");
            return new List<string>();
        }

        return null!;
    }

    public async Task<List<CommonEntity>> GetSudCountryList()
    {
        try
        {

            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/countries";

            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik SudAuthLoginCreate()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"The object can not be deserialized or another error, status: {response.StatusCode}", "Message");
                return new List<CommonEntity>();
            }
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (sud) {ex.Message}: {ex.InnerException}", "Message");
            return null!;
        }
        return null!;
    }

    public async Task<List<CommonEntity>> GetSudCourtList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/courts/ECONOMIC";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);

            try
            {

                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik SudAuthLoginCreate()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud Courts : {response.StatusCode}", "Message");
                return new List<CommonEntity>();
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Courts  {ex.Message}: {ex.InnerException}", "Message");
            return null;
        }
        return null!;
    }

    public async Task<List<CurrencyModel>> GetSudCurrencyList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/currencies";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudCurrencyList()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CurrencyModel>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud Currencies : {response.StatusCode}", "Message");
                return new List<CurrencyModel> { };
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Currencies  {ex.Message}: {ex.InnerException}", "Message");
        }
        return null!;
    }

    public async Task<List<CommonEntity>> GetSudDocumentTypesList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/document-types-list";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudDocumentTypesList()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud Document Types List : {response.StatusCode}", "Message");
                return new List<CommonEntity> { };
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Document Types List   {ex.Message}: {ex.InnerException}", "Message");
            return null;
        }
        return null!;
    }

    public async Task<List<CommonEntity>> GetSudDutyReasonList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/duty-reasons";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);

            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudDutyReasonList()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud DutyReasons : {response.StatusCode}", "Message");
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud DutyReasons  {ex.Message}: {ex.InnerException}", "Message");
        }
        return null!;
    }

    public async Task<List<string>> GetSudEntityTypeList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();

            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/entity-types";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Agar 204 holat kodi qaytsa, bo'sh ro'yxatni qaytarish
                return new List<string>();
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                AddError($"Sud GetSudEntityTypeList: {response.StatusCode}", "Message");
                return null!;
            }
            var result = JsonConvert.DeserializeObject<List<string>>(responseContent);
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud ClaimKinds: {ex.Message}", "Message");
            return new List<string>();
        }

        return null!;
    }

    public async Task<List<string>> GetSudParticipantTypeList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud GetSudParticipantTypeList: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/participant-types";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Agar 204 holat kodi qaytsa, bo'sh ro'yxatni qaytarish
                return new List<string>();
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                AddError($"Sud GetSudParticipantTypeList: {response.StatusCode}", "Message");
                return null!;
            }

            var result = JsonConvert.DeserializeObject<List<string>>(responseContent);
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud ClaimKinds: {ex.Message}", "Message");
            return new List<string>();
        }

        return null!;
    }

    public async Task<List<CommonEntity>> GetSudPostReasonList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud ClaimKinds: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/post-reasons";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);

            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudPostReasonList()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<CommonEntity>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud DutyReasons : {response.StatusCode}", "Message");
                return new List<CommonEntity> { };
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud DutyReasons  {ex.Message}: {ex.InnerException}", "Message");
        }
        return null!;
    }

    public async Task<List<CommonEntityRegion>> GetSudRegionList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud GetSudRegionList: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/regions";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudRegionList()", "Message");
                    return null!;
                }
                else
                {
                    List<CommonEntityRegion>? result = JsonConvert.DeserializeObject<List<CommonEntityRegion>>(responseContent);
                    result = result?.Where(x => x.Parent_Id == null).ToList();


                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud Regions : {response.StatusCode}", "Message");
                return new List<CommonEntityRegion>();
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Regions  {ex.Message}: {ex.InnerException}", "Message");
            return null;
        }
        return null!;
    }

    public async Task<List<CommonEntityRegion>> GetSudDistrictList(Guid regionId)
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud GetSudDistrictList: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/regions";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudDistrictList()", "Message");
                    return null!;
                }
                else
                {
                    List<CommonEntityRegion>? result = JsonConvert.DeserializeObject<List<CommonEntityRegion>>(responseContent);
                    result = result?.Where(x => x.Parent_Id != null && x.Parent_Id == regionId).ToList();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud Districts : {response.StatusCode}", "Message");
                return new List<CommonEntityRegion>();
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Districts  {ex.Message}: {ex.InnerException}", "Message");
            return null;
        }
        return null!;
    }
    public async Task<List<BankModel>> GetSudBankList()
    {
        try
        {
            var token = await _sudLogin.SudAuthLoginCreate();
            if (string.IsNullOrEmpty(token))
            {
                AddError("Sud GetSudBankList: Token olishda xatolik yuz berdi", "Message");
                return null!;
            }
            var url = $"{_sudConfig.api}/e_s/api/guides/banks";
            _httpClient.DefaultRequestHeaders.Add("Justice", $"Token {token}");
            var response = await _httpClient.GetAsync(url);
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    AddError($"responseda hatolik GetSudBankList()", "Message");
                    return null!;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<BankModel>>(responseContent);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                AddError($"Sud Regions : {response.StatusCode}", "Message");
                return new List<BankModel> { };
            }
        }
        catch (Exception ex)
        {
            AddError($"Sud Regions  {ex.Message}: {ex.InnerException}", "Message");
            return null;
        }
        return null!;
    }


    #endregion
}

