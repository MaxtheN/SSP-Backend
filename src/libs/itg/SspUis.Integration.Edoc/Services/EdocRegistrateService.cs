using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SspUis.Core.Configurations;
using SspUis.Integration.Edoc.Configs;
using SspUis.Integration.Edoc.Models;
using StatusGeneric;
using WEBASE.Storage;

namespace SspUis.Integration.Edoc.Services;

public class EdocRegistrateService : StatusGenericHandler, IEdocRegistrateService
{
    private readonly EdocRegistrationConfig _config;
    private readonly SystemConf _systemConf;
    private readonly string _url;
    private HttpClient _httpClient;

    public EdocRegistrateService(EdocRegistrationConfig config, SystemConf systemConf, HttpClient httpClient)
    {
        _config = config;
        _systemConf = systemConf;
        _httpClient = httpClient;
        _url = _systemConf.IsLocalHost
            ? "http://localhost:50005/integration/erp"
            : _systemConf.IsTest ? "http://localhost:5961/integration/erp"
                : "http://192.168.1.3:5005/integration/erp";
        Initialize(config);
    }

    public void Initialize(EdocRegistrationConfig config)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicToken);
    }

    public async Task<ExternalIncomingDocumentDto?> RegistrateEdoc(EdocRegisterRequestDto dto)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var url = _systemConf.IsLocalHost
                ? "http://localhost:50005/integration/erp/Register"
                : _systemConf.IsTest ? "http://localhost:5961/integration/erp/Register"
                    : "http://192.168.1.3:5005/integration/erp/Register";



            var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

            var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);


            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ExternalIncomingDocumentDto>(responseContent);

                    if (result != null)
                    {
                        return result;
                    }
                    return null;
                }
                catch (Exception)
                {
                    AddError("The object can not be deserialized");
                }
            }
            else
                AddError($"Edoc. Response:  {responseObject}");
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (Edoc) {ex.Message}: {ex.InnerException}");
        }

        return null;
    }

    public async Task UpdateEdoc(EdocRegisterRequestDto dto)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var url = _systemConf.IsLocalHost
                ? "http://localhost:50005/integration/erp/Update"
                : _systemConf.IsTest ? "http://localhost:5961/integration/erp/Update"
                    : "http://192.168.1.3:5005/integration/erp/Update";

            var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

            var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                AddError($"Edoc. StatusCode:  {response.StatusCode}");
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (Edoc) {ex.Message}: {ex.InnerException}");
        }

    }

    public async Task<List<FileResultResponse>> UploadFile(byte[] file)
    {
        try
        {
            var url = _systemConf.IsLocalHost
               ? "http://localhost:50005/integration/erp/UploadFile"
               : _systemConf.IsTest ? "http://localhost:5961/integration/erp/UploadFile"
                   : "http://192.168.1.3:5005/integration/erp/UploadFile";

            using (var content = new MultipartFormDataContent())
            {
                MemoryStream stream = new MemoryStream(file);
                StreamContent fileContent = new StreamContent(stream);

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "files",
                    FileName = "yourfilenam.pdf"
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                content.Add(fileContent);

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<List<FileResultResponse>>(responseContent);

                        if (result != null)
                        {
                            return result;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                    AddError($"Edoc. Response:  {response}");
            }
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (Edoc) {ex.Message}: {ex.InnerException}");
        }
        return null;
    }
    public async Task<ExternalIncomingDocumentDto> Get(long id)
    {
        try
        {
            var url = _url + "/Get?externalDocumentId=" + id;

            using (var content = new MultipartFormDataContent())
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<ExternalIncomingDocumentDto>(responseContent);

                        if (result != null)
                        {
                            return result;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                    AddError($"Edoc. Response:  {response}");
            }
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (Edoc) {ex.Message}: {ex.InnerException}");
        }
        return null;
    }
    public async Task<ExternalIncomingDocumentDto> ForCallCenterGet(long id)
    {
        try
        {
            var url = _url + "/GetCallCenterAppeal?callCenterAppealId=" + id;

            using (var content = new MultipartFormDataContent())
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<ExternalIncomingDocumentDto>(responseContent);

                        if (result != null)
                        {
                            return result;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                    AddError($"Edoc. Response:  {response}");
            }
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (Edoc) {ex.Message}: {ex.InnerException}");
        }
        return null;
    }

    public async Task<List<FileResultResponse>> UploadAttachment(StorageFile file)
    {
        try
        {

            var url = _systemConf.IsLocalHost
               ? "http://localhost:50005/integration/erp/UploadAttachment"
               : _systemConf.IsTest ? "http://localhost:5961/integration/erp/UploadAttachment"
                   : "http://192.168.1.3:5005/integration/erp/UploadAttachment";

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(file.GetStream())
                {
                    Headers =
                    {
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "files",
                            FileName = file.FileName
                        },
                        //ContentType = new MediaTypeHeaderValue(file.GetStream().GetType().ToString())
                    }
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(fileContent);

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<List<FileResultResponse>>(responseContent);

                        if (result != null)
                        {
                            return result;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        AddError("The object can not be deserialized");
                    }
                }
                else
                    AddError($"Edoc. Response:  {response}");
            }
        }
        catch (Exception ex)
        {
            AddError($"Произошла ошибка при получении данных из (Edoc) {ex.Message}: {ex.InnerException}");
        }
        return null;
    }

    public async Task<byte[]> DownloadAttachment(Guid fileId, bool isView)
    {
        try
        {
            var url = _url + $"/DownloadAttachment/{fileId}?isView={isView}";
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
}

