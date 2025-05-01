using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm.StaffingServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using System.Runtime.Intrinsics.X86;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [Route("hrm/[controller]/[action]")]
    [ApiController]
    public class StaffingController : WebaseController
    {
        private readonly IStaffingService _service;

        public StaffingController(IStaffingService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingView)]
        public PagedResult<StaffingListDto> GetList([FromBody] StaffingSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }
        [HttpPost]
        [Authorize(ModuleCode.StaffingHeaderView)]
        public PagedResult<StaffingListDto> GetListForHeader([FromBody] StaffingSortFilterPageOptions dto)
        {
            return _service.GetListForHeader(dto);
        }
        [HttpPost]
        [Authorize(ModuleCode.StaffingForOwnOrgView)]
        public PagedResult<StaffingListDto> GetListForOwnOrg([FromBody] StaffingSortFilterPageOptions dto)
        {
            return _service.GetListForOwnOrg(dto);
        }
        [HttpPost]
        public StaffingPostionForNow GetAllStaffingPositionsForQuantity(int positionId, int departmentId, int? organizationId)
        {
            return _service.GetAllStaffingPositionsForQuantity(positionId, departmentId, organizationId);
        }

        [HttpGet]
        //[Authorize(ModuleCode.StaffingView)]
        [ProducesResponseType(typeof(List<StaffingPositionDto>), 200)]
        public IActionResult GetAllStaffingPositions(DateTime? date = null, int? positionId = null, int? positionClassificationId = null, int? departmentId = null, int? organizationId = null)
        {
            if (_service.IsValid)
                return Ok(_service.GetAllStaffingPositions(date, positionId, positionClassificationId, departmentId, organizationId));

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        //[Authorize(ModuleCode.StaffingView)]
        [ProducesResponseType(typeof(List<PositionDto>), 200)]
        public IActionResult GetAllStaffingPositionClassifications(DateTime? date = null, int? positionClassificationId = null, int? departmentId = null)
        {
            if (_service.IsValid)
                return Ok(_service.GetAllStaffingPositions(date, departmentId));

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.StaffingView)]
        [ProducesResponseType(typeof(List<StaffingPositionDto>), 200)]
        public IActionResult FillStaffingPositions(int staffingTemplateId)
        {
            if (ModelState.IsValid)
            {
                if (_service.IsValid)
                    return Ok(_service.FillStaffingPosition(staffingTemplateId));

                _service.CopyErrorsToModelState(ModelState);


            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingView)]
        [ProducesResponseType(typeof(StaffingPositionDto), 200)]
        public IActionResult GetStaffingPosition(GetStaffingPositionDto dto)
        {
            var result = _service.GetStaffingPosition(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.StaffingView)]
        [ProducesResponseType(typeof(StaffingDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingView)]
        [ProducesResponseType(typeof(StaffingDto), 200)]
        public IActionResult FillIndicator(CreateStaffingDlDto dto)
        {
            var fillDto = _service.FillIndicator(dto);
            if (_service.IsValid)
                return Ok(fillDto);
            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet("{id}")]
        [Authorize(ModuleCode.StaffingView)]
        [ProducesResponseType(typeof(StaffingDto), 200)]
        public IActionResult Get(long id)
        {
            StaffingDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.StaffingCreate, ModuleCode.StaffingClone)]
        [ProducesResponseType(typeof(StaffingDto), 200)]
        public IActionResult GetClone(long id)
        {
            StaffingDto dto = _service.GetClone(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingCreate, ModuleCode.AllStaffingCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        //[UserAction("Кадрлар билан таъминлаш ҳужжатини яратиш")]
        public IActionResult Create(CreateStaffingDlDto dto)
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

        [HttpPost]
        [Authorize(ModuleCode.StaffingCreate, ModuleCode.StaffingCreate)]
        [ProducesResponseType(typeof(StaffingPositionDto), 200)]
        public IActionResult RecalcStaffingCalcKindTables([FromBody] StaffingPositionDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.RecalcStaffingCalcKindTables(dto);
                if (_service.IsValid)
                    return Ok(result);
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingEdit)]
        [ProducesResponseType(200)]
        //[UserAction("Кадрлар билан таъминлаш ҳужжатини таҳрирлаш")]
        public IActionResult Update(UpdateStaffingDlDto dto)
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
        [Authorize(ModuleCode.StaffingAccept)]
        [ProducesResponseType(200)]
        //[UserAction("Кадрлар билан таъминлаш ҳужжатини тасдиқлаш")]
        public IActionResult Accept(UpdateStatusStaffingDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Accept(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingCancel)]
        [ProducesResponseType(200)]
        //[UserAction("Кадрлар билан таъминлаш ҳужжатини бекор қилиш")]
        public IActionResult Cancel(UpdateStatusStaffingDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Cancel(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost("{id}")]
        [Authorize(ModuleCode.StaffingSend)]
        [ProducesResponseType(200)]
        public IActionResult Send(long id)
        {
            if (ModelState.IsValid)
            {
                _service.Send(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost("{id}")]
        [Authorize(ModuleCode.StaffingArchive)]
        [ProducesResponseType(200)]
        public IActionResult SendToArchive(long id)
        {
            if (ModelState.IsValid)
            {
                _service.SendToArchive(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost("{id}")]
        [Authorize(ModuleCode.StaffingReArchive)]
        [ProducesResponseType(200)]
        public IActionResult RecallFromArchive(long id)
        {
            if (ModelState.IsValid)
            {
                _service.RecallFromArchive(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.StaffingReject)]
        [ProducesResponseType(200)]
        public IActionResult Reject(UpdateStatusStaffingDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Reject(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.StaffingRevoke)]
        [ProducesResponseType(200)]
        public IActionResult Revoke(UpdateStatusStaffingDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Revoke(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.StaffingReceieved)]
        [ProducesResponseType(200)]
        public IActionResult Receieved(UpdateStatusStaffingDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Receieved(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost("{id}")]
        [Authorize(ModuleCode.StaffingDelete)]
        [ProducesResponseType(200)]
        //[UserAction("Кадрлар билан таъминлаш ҳужжатини ўчириш")]
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
    }
}