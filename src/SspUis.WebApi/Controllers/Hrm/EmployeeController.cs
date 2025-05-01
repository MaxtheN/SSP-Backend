using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.Integration.MSPD.GSP;
using SspUis.BizLogicLayer.Models;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Hrm;
using WEBASE.Storage;
using SspUis.BizLogicLayer.DigitizationCenterServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("hrm/[controller]/[action]")]
    public class EmployeeController : WebaseController
    {
        private IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.EmployeeView, ModuleCode.AllEmployeeView, ModuleCode.ForOtherEmployeeManageView)]
        public PagedResult<EmployeeListDto> GetList([FromBody] EmployeeSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.EmployeeView, ModuleCode.BranchesEmployeeView, ModuleCode.AllEmployeeView)]
        [ProducesResponseType(typeof(EmployeeDto), 200)]
        public IActionResult Get()
        {
            EmployeeDto dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.EmployeeView, ModuleCode.BranchesEmployeeView, ModuleCode.AllEmployeeView)]
        [ProducesResponseType(typeof(EmployeeDto), 200)]
        public IActionResult Get(int id)
        {
            EmployeeDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList([FromBody] EmployeeSortFilterPageOptions options)
        {
            return Ok(_service.AsSelectList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.EmployeeCreate, ModuleCode.AllEmployeeCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<int> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.EmployeeCreate, ModuleCode.BranchesEmployeeCreate, ModuleCode.AllEmployeeCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult CreateByUser(CreateEmployeeByUserDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<int> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.EmployeeEdit, ModuleCode.BranchesEmployeeEdit, ModuleCode.AllEmployeeEdit)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update(UpdateEmployeeDlDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{id}")]
        [Authorize(ModuleCode.EmployeeDelete, ModuleCode.BranchesEmployeeDelete, ModuleCode.AllEmployeeDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(int id)
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
        [ProducesResponseType(typeof(CreateEmployeeDto), 200)]
        public async Task<IActionResult> GetByPassportData([FromQuery] GSPPersonInfoRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _service.GetByPassportData(dto);

                if (_service.IsValid)
                    return Ok(user);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [ProducesResponseType(typeof(EmployeeRelativeDto), 200)]
        public async Task<IActionResult> GetRelativesByPassportData([FromQuery] GSPPersonInfoRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var person = await _service.GetRelativesByPassportData(dto);

                if (_service.IsValid)
                    return Ok(person);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DownloadEmployeeCV(string pinfl, string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadCV(pinfl, lang).ConfigureAwait(false);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[ProducesResponseType(200)]
        //public async Task<IActionResult> AddOrUpdatePersonFiles([FromBody] AddPersonFileFromEmployeeDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _service.AddOrUpdatePersonFilesAsync(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}
    }
}
