using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Arbitration/[controller]/[action]")]
    public class ArbitrationJudgeController : WebaseController
    {
        private IArbitrationJudgeService _service;

        public ArbitrationJudgeController(IArbitrationJudgeService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.ArbitrationJudgeView)]
        public PagedResult<ArbitrationJudgeListDto> GetList([FromBody] ArbitrationJudgeSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.ArbitrationJudgeView)]
        [ProducesResponseType(typeof(ArbitrationJudgeDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ArbitrationJudgeView)]
        [ProducesResponseType(typeof(ArbitrationJudgeDto), 200)]
        public IActionResult Get(int id)
        {
            ArbitrationJudgeDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationJudgeCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public async Task<IActionResult> Create(CreateArbitrationJudgeDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<int> result = await _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationJudgeEdit)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update(UpdateArbitrationJudgeDlDto dto)
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
        [Authorize(ModuleCode.ArbitrationJudgeDelete)]
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
    }
}
