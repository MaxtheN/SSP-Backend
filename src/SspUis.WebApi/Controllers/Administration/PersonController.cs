using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.Repositories;
using WEBASE.Integration.MSPD.GSP;
using WEBASE.AspNet.Security;
using WEBASE.Storage;
using SspUis.BizLogicLayer.DigitizationCenterServices;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.PositionServices;
using SspUis.BizLogicLayer.Administration.PersonLogService;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Person/[action]")]
    public class PersonController : WebaseController
    {
        private IPersonService _service;
        private IPersonLogService _servicePersonLog;
        private readonly IDigitizationCenterService _digitizationservice;

        public PersonController(IPersonService service, IPersonLogService servicePersenLog, IDigitizationCenterService digitizationservice)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _servicePersonLog = servicePersenLog;
            _digitizationservice = digitizationservice;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonDto), 200)]
        public async Task<IActionResult> GetByPassportData([FromQuery] GSPPersonInfoRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var person = await _service.GetByPassportData(dto);

                if (_service.IsValid)
                    return Ok(person);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public async Task<IActionResult> CreatePersonLog([FromQuery] CreatePersonLogDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var person =  _servicePersonLog.Create(dto);

                if (_service.IsValid)
                    return Ok(person);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PagedResult<PersonLogListDto>), 200)]
        public PagedResult<PersonLogListDto> GetList([FromBody] PersonLogSortFilterOption dto)
        {
            return _servicePersonLog.GetList(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonLogListDto), 200)]
        public async Task<IActionResult> GetByEmployeeId([FromQuery] int EmployeeId)
        {
            if (ModelState.IsValid)
            {
                var person =  _servicePersonLog.GetByEmployeeId(EmployeeId);

                if (_service.IsValid)
                    return Ok(person);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonDto), 200)]
        public async Task<IActionResult> GetByPassportDataFromDigital([FromQuery] GSPNewApiRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var person = await _service.GetByPassportDataFromDigital(dto);

                if (_service.IsValid)
                    return Ok(person);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [ProducesResponseType(typeof(PersonDto), 200)]
        public async Task<IActionResult> GetChildFromGsp([FromQuery] PersonFilterDto dto)
        {
            if (ModelState.IsValid)
            {
                var person = await _service.GetChildFromGsp(dto);

                if (_service.IsValid)
                    return Ok(person);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CheckExcelData(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var res = await _service.CheckExcelData(file);

                return File(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnApplicationAndContractTemplate.xlsx");

            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ServiceFilter(typeof(UploadEmployeeImageFileAttribute))]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public IActionResult UploadFile(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                StorageFile dto = new StorageFile(file.FileName, file.OpenReadStream());
                var result = _service.UploadFile(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

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

        [HttpPost("{fileId}")]
        public IActionResult DeleteFile(Guid fileId)
        {
            if (ModelState.IsValid)
            {
                _service.DeleteFile(fileId);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        #region Gsp
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetFromGSP(GSPNewApiRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _digitizationservice.GetFromGSP(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        #endregion
    }
}
