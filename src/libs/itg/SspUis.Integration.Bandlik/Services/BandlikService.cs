using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SspUis.Integration.Bandlik.Configs;
using SspUis.Integration.Bandlik.Models;
using StatusGeneric;
using System.Net.Http.Headers;
using System.Text;

namespace SspUis.Integration.Bandlik.Services
{
    public class BandlikService: StatusGenericHandler, IBandlikService
    {
        private readonly BandlikConfig _config;
        private HttpClient _httpClient;


        public BandlikService(BandlikConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
            Initialize(config);
        }

        public void Initialize(BandlikConfig config)
        {
            _httpClient.DefaultRequestHeaders.Add("Token", $"{config.Token}");
            if (config.UseProxy)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.Basictoken);
            }
        }

        public async Task<GetStatisticByInnDataDto> GetStatisticByInn(string inn)
        {
            try
            {
                GetStatisticByInnRequestDto dto = new GetStatisticByInnRequestDto();
                dto.Jsonrpc = "2.0";
                dto.Id = 5142;
                dto.Method = "enst.company.statistic";
                dto.Params = new GetStatisticByInnParams
                {
                    Query = new GetStatisticByInnQuery { Tin = inn }
                };


                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.Api}";

                //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = $"Бандлик тизимидан маълумот олишда хатолик рўй берди! {dto.Params.Query.Tin}";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch { }

                    AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                    return null!;
                }
                var responceJson = await response.Content.ReadAsStringAsync();
                GetStatisticByInnResponseDto res;
               
                try
                {
                    res = JsonConvert.DeserializeObject<GetStatisticByInnResponseDto>(responceJson, new JsonSerializerSettings 
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    })!;
                    if (res == null || res.Result == null)
                    {
                        AddError($"Бандлик тизимидан маълумот топилмади! {dto.Params.Query.Tin}");
                        return null;   
                    }
                    if (res?.Result?.Success == null || res?.Result?.Success== false)
                    {
                        AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди! {dto.Params.Query.Tin}");
                    }
                }
                catch
                {
                    throw;
                }

               
                return res.Result.Data;
            }
            catch (Exception ex)
            {
                AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди! {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
        public async Task<List<GetFreeAreaByInnDataDto>> GetFreeAreaByInn()
        {
            try
            {
                BandlikRequestDto dto = new BandlikRequestDto();
                dto.Jsonrpc = "2.0";
                dto.Id = 5142;
                dto.Method = "abkm.free.area";

                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.Api}";

                //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = $"Бандлик тизимидан маълумот олишда хатолик рўй берди! ";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch { }

                    AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                    return null!;
                }
                var responceJson = await response.Content.ReadAsStringAsync();
                GetFreeAreaByInnResponseDto res;

                try
                {
                    res = JsonConvert.DeserializeObject<GetFreeAreaByInnResponseDto>(responceJson, new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    })!;
                    if (res == null || res.Result == null)
                    {
                        AddError($"Бандлик тизимидан маълумот топилмади!");
                        return null;
                    }
                    if (res?.Result?.Success == null || res?.Result?.Success == false)
                    {
                        AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди!");
                    }
                }
                catch
                {
                    throw;
                }


                return res.Result.Data;
            }
            catch (Exception ex)
            {
                AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди! {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
        public async Task<GetDaftarBySoatoDataDto> GetDaftarBySoato(GetDaftarBySoatoQuery queryDto)
        {
            try
            {
                GetDaftarBySoatoRequestDto dto = new GetDaftarBySoatoRequestDto();
                dto.Jsonrpc = "2.0";
                dto.Id = 5142;
                dto.Method = "daftar.notebook.by.soato";
                dto.Params = new GetDaftarBySoatoParams
                {
                    Body = queryDto
                };
                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.Api}";

                //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = $"Бандлик тизимидан маълумот олишда хатолик рўй берди! ";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch { }

                    AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                    return null!;
                }
                var responceJson = await response.Content.ReadAsStringAsync();
                GetDaftarBySoatoResponseDto res;

                try
                {
                    res = JsonConvert.DeserializeObject<GetDaftarBySoatoResponseDto>(responceJson, new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    })!;
                    if (res == null || res.Result == null)
                    {
                        AddError($"Бандлик тизимидан маълумот топилмади!");
                        return null;
                    }
                    if (res?.Result?.Success == null || res?.Result?.Success == false)
                    {
                        AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди!");
                    }
                }
                catch
                {
                    throw;
                }


                return res.Result.Result;
            }
            catch (Exception ex)
            {
                AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди! {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }
        public async Task<GetInfoEmpByInnDataDto> GetInfoEmpByInn(string inn)
        {
            try
            {
                GetInfoEmpByInnRequestDto dto = new GetInfoEmpByInnRequestDto();
                dto.Jsonrpc = "2.0";
                dto.Id = 5142;
                dto.Method = "enst.company.infoemp";
                dto.Params = new GetInfoEmpByInnParams
                {
                    Query = new GetInfoEmpByInnQuery
                    {
                        Tin = inn,
                    }
                };


                var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

                var url = $"{_config.Api}";

                //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = $"Бандлик тизимидан маълумот олишда хатолик рўй берди! {dto.Params.Query.Tin}";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch { }

                    AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
                    return null!;
                }
                var responceJson = await response.Content.ReadAsStringAsync();
                GetInfoEmpByInnResponseDto res;

                try
                {
                    res = JsonConvert.DeserializeObject<GetInfoEmpByInnResponseDto>(responceJson, new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    })!;
                    if (res == null || res.Result == null)
                    {
                        AddError($"Бандлик тизимидан маълумот топилмади! {dto.Params.Query.Tin}");
                        return null;
                    }
                    if (res?.Result?.Success == null || res?.Result?.Success == false)
                    {
                        AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди! {dto.Params.Query.Tin}");
                    }
                }
                catch
                {
                    throw;
                }


                return res.Result.Data;
            }
            catch (Exception ex)
            {
                AddError($"Бандлик тизимидан маълумот олишда хатолик рўй берди! {ex.Message}: {ex.InnerException}");
            }

            return null!;
        }

        public async Task<(string, dynamic)> PostMonoApplication(BandlikRequestMonoPostDto dto)
        {
            try
            {

                BandlikRequestMonoPost newDto = new BandlikRequestMonoPost();
                newDto.Jsonrpc = "2.0";
                newDto.Id = 123456;
                newDto.Method = "abkm.mono.center.application.create";
                newDto.Params = new BandlikRequestMonoPostParams
                {
                    Body = dto
                };

                var url = $"{_config.Api}";

                var dtoJson = JsonConvert.SerializeObject(newDto, Formatting.Indented);

                //var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                //var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    return (url, jsonResponse);
                }
                else
                {
                    AddError($"(mehnat.uz). {response.StatusCode},  Response:  {jsonResponse}");
                    return ("else_url", jsonResponse);
                }
            }
            catch (Exception ex)
            {
                AddError($"Произошла ошибка при получении данных из (mehnat.uz) {ex.Message}: {ex.InnerException}");
            }

            return ("end_url", null);
        }
    }
}
