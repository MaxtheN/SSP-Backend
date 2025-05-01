using SspUis.BizLogicLayer.Models;
using SspUis.BizLogicLayer.OrganizationServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers.Administration
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrganizationController : WebaseController
    {

        private IOrganizationService _service;

        public OrganizationController(IOrganizationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.OrganizationView, ModuleCode.AllOrganizationView)]
        public PagedResult<OrganizationListDto> GetList([FromBody] OrganizationSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.OrganizationView, ModuleCode.AllOrganizationView)]
        [ProducesResponseType(typeof(OrganizationDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.OrganizationView, ModuleCode.AllOrganizationView)]
        [ProducesResponseType(typeof(OrganizationDto), 200)]
        public IActionResult Get(int id)
        {
            if (ModelState.IsValid)
            {
                OrganizationDto dto = _service.Get(id);

                if (_service.IsValid)
                    return Ok(dto);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet("{inn}")]
        [Authorize(ModuleCode.OrganizationView, ModuleCode.AllOrganizationView)]
        [ProducesResponseType(typeof(OrganizationDto), 200)]
        public async Task<IActionResult> GetByInn(string inn)
        {
            if (ModelState.IsValid)
            {
                var dto = await _service.GetByInn(inn);

                if (_service.IsValid)
                    return Ok(dto);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList(int? parentId = null, bool authorizedOnly = false, bool inspectionOnly = false, int? signOrganizationTypeId = null, int? organizationGroupId = null)
        {
            return Ok(_service.AsSelectList(parentId, authorizedOnly, inspectionOnly, signOrganizationTypeId, organizationGroupId));
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult AsSelectListOrgSettlementAccount()
        {
            return Ok(_service.AsSelectListOrgSettlementAccount());
        }

        [HttpPost]
        [Authorize(ModuleCode.OrganizationCreate, ModuleCode.AllOrganizationCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public async Task<IActionResult> Create(CreateOrganizationDlDto dto)
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
        [Authorize(ModuleCode.OrganizationEdit, ModuleCode.AllOrganizationEdit)]
        [ProducesResponseType(200)]
        public async Task <IActionResult> Update(UpdateOrganizationDlDto dto)
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
        [Authorize(ModuleCode.OrganizationDelete, ModuleCode.AllOrganizationDelete)]
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
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult UpdateStructure(int organizationId, int? structureId)
        {
            _service.UpdateStructure(organizationId, structureId);
            if (_service.IsValid)
                return Ok();
            _service.CopyErrorsToModelState(ModelState);
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExecel(OrganizationSortFilterPageOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Organization.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.OrganizationCreate, ModuleCode.AllOrganizationCreate)]
        public async Task<IActionResult> CheckPersonFromGsp(List<CheckPersonFromGspDlDto> listDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CheckPersonFromGsp(listDto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
            [HttpPost]
        [ServiceFilter(typeof(UploadFileAttribute))]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public IActionResult UploadFile([FromForm] List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                StorageFile[] dto = files.Select(a => new StorageFile(a.FileName, a.OpenReadStream())).ToArray();
                var result = _service.UploadFiles(dto);

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

        [HttpPost("id")]
        public IActionResult SelectListByRegionInOrganizations(int regionid)
        {
            var result = _service.SelectListByRegionInOrganizations(regionid);

            return Ok(result);
        }
    }
}
