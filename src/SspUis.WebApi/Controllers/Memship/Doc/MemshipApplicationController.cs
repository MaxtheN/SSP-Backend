using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.MemshipApplicationServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;


namespace SspUis.WebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("Memship/[controller]/[action]")]
    public class MemshipApplicationController : WebaseController
    {
        private IMemshipApplicationService _service;

        public MemshipApplicationController(IMemshipApplicationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipApplicationView)]
        public PagedResult<MemshipApplicationListDto> GetList([FromBody] MemshipApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
       [AllowAnonymous]
        [ProducesResponseType(typeof(MemshipApplicationDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.MemshipApplicationView)]
        [ProducesResponseType(typeof(MemshipApplicationDto), 200)]
        public async Task<IActionResult> GetForErp(string inn)
        {
            var dto = _service.GetForErp(inn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet("{id}")]
        [Authorize(ModuleCode.MemshipApplicationView)]
        [ProducesResponseType(typeof(MemshipApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.MemshipApplicationCreate)]
        [ProducesResponseType(typeof(CreateMemshipApplicationDto), 200)]
        public async ValueTask<IActionResult> CreateForErp(CreateMemshipApplicationDlDto dto)
        {
            var result = await _service.CreateForErp(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet("{fileId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public IActionResult DownloadFile(Guid fileId, [FromServices] IMimeMappingService mimeMappingService)
        {
            if (ModelState.IsValid)
            {
                StorageFile file = _service.DownloadFile(fileId);

                if (_service.IsValid)
                    return File(file.GetStream(), mimeMappingService.Map(file.FileName));

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.MemshipApplicationDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(long id)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult DownloadPdf(Guid id2, string? lang = null)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult DownloadPdfCopy(MemshipApplicationForPdf model)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPdf(model);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [ProducesResponseType(typeof(Boolean), 200)]
        public IActionResult CanCreate()
        {
            if (ModelState.IsValid)
            {
                var result = _service.CanCreate();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[Authorize(ModuleCode.MemshipApplicationReject)]
        //[ProducesResponseType(200)]
        //public IActionResult Reject(RejectStatusMemshipApplicationDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Reject(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        [HttpPost]
        [Authorize(ModuleCode.MemshipApplicationAccept)]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> Accept(AcceptStatusMemshipApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Accept(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        //[HttpGet]
        //[AllowAnonymous]
        //[ProducesResponseType(200)]
        //public IActionResult FixErrorMemship()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var res = _service.FixErrorMemship();

        //        if (_service.IsValid)
        //            return Ok(res);

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}


        [HttpPost]
        [Authorize(ModuleCode.MemshipApplicationCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusMemshipApplicationDto dTo)
        {
            if (ModelState.IsValid)
            {
                _service.Cancel(dTo);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult SaveAsExcel(MemshipApplicationSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemshipApplication.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Adliya()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "Api-Token: D9368098-E061-49CC-90B8-0E6A811826E3");
                //client.DefaultRequestHeaders.Add("Client-Auth", $"Basic {Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":"))}");

                // Define the JSON-RPC request.
                string jsonRpcRequest = "{\"jsonrpc\": \"2.0\", \"id\": \"" +
                    Guid.NewGuid().ToString() +
                    "\", \"method\": \"adliya.get_legal_entity_info_v2\", \"params\": {\"tin\":\"309671235\"}}";

                // Set the base address of the remote JSON-RPC server.
                client.BaseAddress = new Uri("https://proxy.chamber.uz/http://10.190.24.138:7075/");

                // Set the Content-Type header to indicate that you are sending JSON.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Create a StringContent object to hold the JSON-RPC request.
                StringContent content = new StringContent(jsonRpcRequest, Encoding.UTF8, "application/json");

                // Send the POST request.
                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response as a string.
                    string responseString = await response.Content.ReadAsStringAsync();
                    return Ok("JSON-RPC Response: " + responseString);
                }
                else
                {
                    return Ok("HTTP Request failed with status code: " + response.StatusCode);
                }
            }
            return Ok(" :-) ");
        }
        [HttpPost]
        public async Task<IActionResult> GetQqsAosSum(string inn , int year, int n)
        {
            if (ModelState.IsValid)
            {
                var resut = await _service.SummQqsOrAos(inn,year,n);
                if (_service.IsValid)
                    return Ok(resut);

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> GetMistake(int type, int pageSize, int pageNumber)
        {
            if (ModelState.IsValid)
            {
                var resut = await _service.MistakeSetPropsInContractorList(type, pageSize, pageNumber);
                if (_service.IsValid)
                    return Ok(resut);

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
