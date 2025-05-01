using SspUis.BizLogicLayer.CustomJobServices;
using SspUis.BizLogicLayer.RoleServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core;
using WEBASE.Integration.MSPD.Sud;
using SspUis.Core.Configurations;


namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomJobController : WebaseController
    {
        private ICustomJobService _service;
        private readonly SystemConf _systemConf;

        public CustomJobController(ICustomJobService service,  SystemConf systemConf)
        : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
        }

        [HttpPost]
        [Authorize(ModuleCode.CustomJobCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateCustomJobDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.CustomJobView)]
        [ProducesResponseType(typeof(CustomJobDto), 200)]
        public IActionResult Get() =>
            Ok(_service.Get());

        [HttpGet("{id}")]
        [Authorize(ModuleCode.CustomJobView)]
        [ProducesResponseType(typeof(CustomJobDto), 200)]
        public IActionResult Get(long id)
        {
            CustomJobDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CustomJobViewAll)]
        public PagedResult<CustomJobListDto> GetList([FromBody] CustomJobSortFilterOptions dto) =>
            _service.GetList(dto);
        
        [HttpPost]
        [ProducesResponseType(200)]
        [Authorize(ModuleCode.CustomJobEdit)]
        public IActionResult Update(UpdateCustomJobDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = _service.Update(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        [Authorize(ModuleCode.CustomJobDelete)]
        public IActionResult Delete(long id)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(id);

                if (_service.IsValid)
                    return Ok(HaveId.Create(id));

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CustomJobApprove)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Approve(UpdateStatusCustomJobDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = await _service.Approve(dto);

                if (_service.IsValid)
                { 
                    return Ok(res);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CustomJobApprove)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Cancel(long id)
        {
            if (ModelState.IsValid)
            {
                var res = await _service.Cancel(id);

                if (_service.IsValid)
                {
                    return Ok(res);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CustomJobViewAll)]
        public PagedResult<CustomJobListDto> GetTable([FromBody] CustomJobSortFilerByIdOptions dto) =>
          _service.GetTable(dto);

    }
}
