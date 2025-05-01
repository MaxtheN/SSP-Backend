using Newtonsoft.Json;
using SspUis.Integration.Bojxona.Configs;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.Soliq;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bojxona.Services
{
    public class BojxonaService: StatusGenericHandler, IBojxonaService
    {
        private readonly BojxonaConfig _config;
        private HttpClient _httpClient;


        public BojxonaService(BojxonaConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
            Initialize(config);
        }

        public void Initialize(BojxonaConfig config)
        {
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.Basictoken);
                _httpClient.DefaultRequestHeaders.Add("Client-Auth", $"Basic {config.BasicTokenClient}");
            }
            else
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.BasicTokenClient);
        }

        public async Task<List<GetGTDByInnDataDto>> GetGTDByInn(GetGTDByInnRequestDto dto)
        {
            try
            {
                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.Api}/SSP_receive/rest/serviceGtd/getGTD";

                //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = $"Божхона тизимидан маълумот олишда хатолик рўй берди! {dto.Stir}";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch { }

                    AddError($"Божхона тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                    return null!;
                }
                var responceJson = await response.Content.ReadAsStringAsync();
                GetGTDByInnResponseDto res;
               
                try
                {
                    res = JsonConvert.DeserializeObject<GetGTDByInnResponseDto>(responceJson)!;
                    if (res == null)
                    {
                        AddError($"Божхона тизимидан маълумот топилмади! {dto.Stir}");
                        return null;   
                    }
                    if (res?.Status != 1)
                    {
                        AddError($"Божхона тизимидан маълумот олишда хатолик рўй берди! {dto.Stir}");
                    }
                }
                catch
                {
                    throw;
                }

               
                return res.Data;
            }
            catch (Exception ex)
            {
                AddError($"Божхона тизимидан маълумот олишда хатолик рўй берди! {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
    }
}
