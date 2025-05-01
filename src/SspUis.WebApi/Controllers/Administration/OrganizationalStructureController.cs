using SspUis.BizLogicLayer.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using SspUis.BizLogicLayer.OrganizationalStructureServices;
using SspUis.BizLogicLayer.Info.OrganizationalStructureServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrganizationalStructureController : WebaseController
    {
        private IOrganizationalStructureService _service;

        public OrganizationalStructureController(IOrganizationalStructureService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.OrganizationalStructureView)]
        public PagedResult<OrganizationalStructureListDto> GetList([FromBody] OrganizationalStructureFilterDto dto)
        {
            return _service.GetList(dto);
        }
        [HttpGet]
        [Authorize(ModuleCode.OrganizationalStructureView)]
        public IActionResult GetOrganizationCount()
        {
            return Ok(_service.GetOrganizationCount());
        }

        [HttpGet]
        [Authorize(ModuleCode.OrganizationalStructureView)]
        [ProducesResponseType(typeof(OrganizationalStructureDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.OrganizationalStructureView)]
        [ProducesResponseType(typeof(OrganizationalStructureDto), 200)]
        public IActionResult Get(int id)
        {
            OrganizationalStructureDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpGet()]
        [ProducesResponseType(typeof(decimal), 200)]
        public IActionResult GetCorrCoef()
        {
            return Ok(_service.GetCorrCoef());
        }

        [HttpPost]
        [Authorize(ModuleCode.OrganizationalStructureCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        //[UserAction("Ташкилий тузилма ҳужжатини яратиш")]
        public IActionResult Create(CreateOrganizationalStructureDlDto dto)
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
        [Authorize(ModuleCode.OrganizationalStructureEdit)]
        [ProducesResponseType(200)]
        //[UserAction("Ташкилий тузилма ҳужжатини таҳрирлаш")]
        public IActionResult Update(UpdateOrganizationalStructureDlDto dto)
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

        [HttpPost("{id}")]
        [Authorize(ModuleCode.OrganizationalStructureDelete)]
        [ProducesResponseType(200)]
        //[UserAction("Ташкилий тузилма ҳужжатини ўчириш")]
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
        //[HttpGet]
        //[ProducesResponseType(200)]
        //[Authorize(new[] { ModuleCode.OrganizationalStructureView })]
        //public IActionResult PrintOrganizationStructure(int id, bool isFullOrganization)
        //{
        //    var file = _service.PrintOrganizationStructure(id, isFullOrganization);
        //    if (_service.IsValid)
        //        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrganizationStructure.xlsm");

        //    _service.CopyErrorsToModelState(ModelState);

        //    return ValidationProblem(ModelState);
        //}
        //[HttpPost]
        //[ProducesResponseType(200)]
        //[Authorize(new[] { ModuleCode.OrganizationalStructureView })]
        //public IActionResult PrintOrganizationalStructureList([FromBody] OrganizationalStructureFilterDto dto)
        //{
        //    var file = _service.PrintOrganizationalStructureList(dto);
        //    if (_service.IsValid)
        //        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrganizationalStructureList.xlsm");

        //    _service.CopyErrorsToModelState(ModelState);

        //    return ValidationProblem(ModelState);
        //}

        //[HttpPost]
        //[ProducesResponseType(200)]
        //[Authorize(new[] { ModuleCode.OrganizationalStructureView })]
        //public IActionResult PrintOrganizationalStructureDashboard()
        //{
        //    var file = _service.PrintOrganizationalStructureDashboard();
        //    if (_service.IsValid)
        //        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrganizationalStructureList.xlsm");

        //    _service.CopyErrorsToModelState(ModelState);

        //    return ValidationProblem(ModelState);
        //}
        [HttpGet]
        [ProducesResponseType(200)]
        [Authorize(new[] { ModuleCode.OrganizationalStructureView })]
        public IActionResult GetListForDashbord(string? parentCode, int? regionId)
        {

            var data = _service.GetListDashboard(parentCode, regionId);
            return Ok(data);
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [Authorize(ModuleCode.OrganizationalStructureView)]
        public IActionResult GetListForDashbord2()
        {

            var data = _service.GetListDashboard2();
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ModuleCode.OrganizationalStructureEdit)]
        [ProducesResponseType(200)]
        //[UserAction("Ташкилий тузилма ҳужжатини таҳрирлаш")]
        public IActionResult UploadEcxel(IFormFile file)
        {
            _service.UploadEcxel(file);
            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
