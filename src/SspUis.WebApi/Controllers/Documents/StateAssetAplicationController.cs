using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer;
using SspUis.Core.Configurations;
using SspUis.Job.BizLogicLayer.Services;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.PrtnContractServices;
using WEBASE.AspNet;
using Humanizer;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.DataLayer;

namespace SspUis.WebApi.Controllers.Documents
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class StateAssetApplicationController : WebaseController
    {
        private IStateAssetApplicationService _service;
        private readonly SystemConf _systemConf;
        private IStateAssetApplicationJobService _jobService;
        private IDocumentJobHistoryService _documentJobHistoryService;


        public StateAssetApplicationController(
            IStateAssetApplicationService service, SystemConf systemConf, IDocumentJobHistoryService documentJobHistoryService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
            _documentJobHistoryService = documentJobHistoryService;
        }

        [HttpPost]
        [Authorize(ModuleCode.StateAssetApplicationView, ModuleCode.StateAssetApplicationViewAll)]
        public PagedResult<StateAssetApplicationListDto> GetList([FromBody] StateAssetDocumentSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id2}")]
        [Authorize(ModuleCode.StateAssetApplicationView)]
        [ProducesResponseType(typeof(PrtnContractDto), 200)]
        public IActionResult Get(Guid id2)
        {
            StateAssetApplicationDto dto = _service.Get(id2);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.StateAssetApplicationResendToDavAktiv, ModuleCode.StateAssetApplicationView, ModuleCode.StateAssetApplicationViewAll)]
        public IActionResult SendToDavaktivCustom(long id)
        {
            if (!_systemConf.IsTest)
            {
                _service.SetModifiedStatus(new ModifiedStatusStateAssetApplicationDto
                {
                    Id = id,
                });

                if (_service.IsValid)
                {
                    _jobService.RequestToSent(id);

                    if (_jobService.IsValid)
                        return Ok();

                    _service.CopyErrorsToModelState(ModelState);

                    return ValidationProblem(ModelState);
                }

                _service.CopyErrorsToModelState(ModelState);

                return ValidationProblem(ModelState);
            }
            
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.StateAssetApplicationView, ModuleCode.StateAssetApplicationViewAll)]
        public IActionResult GetJobStatus(long id)
        {
            var res = _documentJobHistoryService.GetByTableId(TableIdConst.DOC_APPLICATION, id).Select(a=> new
            {
                a.Id,
                a.StatusId,
                a.CreatedAt,
                a.StartAt,
                a.EndAt,
                a.IsSucceed,
                a.Message,
            }).OrderByDescending(a=> a.CreatedAt).ToList();

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
