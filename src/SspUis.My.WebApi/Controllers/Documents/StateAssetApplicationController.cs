using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.Job.BizLogicLayer.Services;
using SspUis.Core.Configurations;
using WEBASE.Integration.MSPD.Sud;
using SspUis.Integration.DigitizationCenter.Models.Mehnat;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class StateAssetApplicationController : WebaseController
    {
        private IStateAssetApplicationService _service;
        private IStateAssetApplicationJobService _jobService;
        private readonly SystemConf _systemConf;

        public StateAssetApplicationController(
            IStateAssetApplicationService service, IStateAssetApplicationJobService jobService, SystemConf systemConf)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _jobService = jobService;
            _systemConf = systemConf;
        }

        [HttpPost]
        public PagedResult<StateAssetApplicationListDto> GetList([FromBody] StateAssetDocumentSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }
        [HttpPost]
        public IActionResult GetCount()
        {
            var data = _service.GetCount();
            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(StateAssetApplicationDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StateAssetApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateStateAssetApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.Create(dto);
                if (_service.IsValid)
                {

                    if (!_systemConf.IsTest)
                    {
                        _jobService.RequestToSent(result.Id);
                    }
                    return Ok(result?.Id);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SendToDavaktivByJob(int startId, int endId)
        {
            if (!_systemConf.IsTest)
            {
                for (int i = startId; i <= endId; i++)
                {
                    _jobService.RequestToSent(i);
                }
            }
            return Ok();
        }

       
        


        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateStateAssetApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Update(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
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

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult GetAsHtml(StateAssetApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = _service.GetAsHtml(dto);
                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetAsHtml()
        {
            if (ModelState.IsValid)
            {
                var res = _service.GetAsHtml();
                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet("id2")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetAsPdf(Guid id2)
        {
            if (ModelState.IsValid)
            {
                var res = _service.GetAsPdf(id2);
                if (_service.IsValid)
                    return File(res, "application/pdf");
            }

            return NotFound();
        }


    }
}
