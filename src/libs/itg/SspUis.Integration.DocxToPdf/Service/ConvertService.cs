using System.Text;
using Newtonsoft.Json;
using SspUis.Core.Configurations;
using StatusGeneric;

namespace SspUis.Integration.DocxToPdf;

public class ConvertService : StatusGenericHandler, IConvertService
{
    private readonly DocxToPdfConfig _config;
    private readonly SystemConf _sys;
    private HttpClient _httpClient;

    public ConvertService(DocxToPdfConfig config, SystemConf sys)
    {
        _config = config;
        _httpClient = new();
        _sys = sys;
    }

    public async ValueTask<byte[]> DocxToPdfAsync(MemoryStream file, object data)
    {
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        var res = await ConvertToPdfAsync(file);
        if (res is not null)
            return res;

        var wordBytes = file.ToArray();
        var wordBase64 = Convert.ToBase64String(wordBytes);

        var model = new Model()
        {
            data = data,
            file = wordBase64,
        };
        var settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        //settings.Converters.Add(new )

        var jsonContent = JsonConvert.SerializeObject(model, settings);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_config.Api, content);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            if (responseData == null)
                return null;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);
            if (apiResponse == null || apiResponse.file == null)
            {
                AddError("docx2pdf api returned null");
                return null;
            }

            byte[] pdfBytes = Convert.FromBase64String(apiResponse.file);
            return pdfBytes;
        }
        else
        {
            Console.WriteLine("\n\n=====================================\n\n");
            var error = JsonConvert.SerializeObject(response);
            Console.WriteLine(error);
            AddError($"DocxToPdf api error : {response.StatusCode}. " + error);
            return null;
        }

    }

    public async Task<byte[]> ConvertToPdfAsync(MemoryStream file)
    {
        using (var content = new MultipartFormDataContent())
        {
            // Add the file content to the request
            content.Add(new StreamContent(file), "data", "tempFile.docx");
            try
            {
                // Make the HTTP POST request
                using (var response = await _httpClient.PostAsync(_config.Api2, content))
                {
                    try
                    {
                        // Check if the request was successful
                        response.EnsureSuccessStatusCode();
                    }
                    catch (Exception e)
                    {
                        AddError("Konverter hato qaytardi.");
                        AddError($"Converter Errors: {e}");
                        return null;
                    }
                    // Read the response content as a byte array
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch (Exception ex)
            {
                AddError("Konverter ishlamayapti.");
                return null;
            }
        }
    }
}
internal class ApiResponse
{
    public string status { get; set; }
    public string file { get; set; }
}
internal class Model
{
    public object data { get; set; }
    public string file { get; set; }
}

