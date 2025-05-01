using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Appeal;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;

namespace SspUis.WebApi.Controllers.Integration
{
    //[BasicAuth("edoc")]
    [ApiController]
    [Route("EdocIntegration/[action]")]
    public class EdocIntegrationController : WebaseController
    {
        private IExternalDocFromEdocService _externalDocFromEdocService;

        public EdocIntegrationController(IExternalDocFromEdocService externalDocFromEdocService)
        {
            _externalDocFromEdocService = externalDocFromEdocService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Ping()
        {
            return Ok("ok");
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AppealSetAsExecuted(long id, [FromServices] IAppealApplicationService service)
        {
            if (ModelState.IsValid)
            {
                await service.Executed(new()
                {
                    Id = id
                });

                if (service.IsValid)
                    return Ok();

                service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CallCenterAppealSetAsExecuted(long id, [FromServices] ICallCenterAppealService service)
        {
            if (ModelState.IsValid)
            {
                await service.Executed(new()
                {
                    Id = id
                });

                if (service.IsValid)
                    return Ok();

                service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> HasEdocResponce(long id, [FromServices] IAppealApplicationService service)
        {
            if (ModelState.IsValid)
            {
                await service.HasEdocResponce(new()
                {
                    Id = id
                });

                if (service.IsValid)
                    return Ok();

                service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
      
        [HttpPost]
        [ProducesResponseType(200)]
        public  IActionResult EdocInfoCreate(CreateExternalDocumentFromEdocDlDto dto,
            [FromServices] IExternalDocFromEdocService service)
        {
            if (ModelState.IsValid)
            { 
                service.Create(dto);

                if (service.IsValid)
                    return Ok();

                service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EdocInfoUpdate([FromBody] UpdateExternalDocumentFromEdocDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _externalDocFromEdocService.Update(dto);

                if (_externalDocFromEdocService.IsValid)
                    return Ok();
                _externalDocFromEdocService.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CallCenterHasEdocResponce(long id, [FromServices] ICallCenterAppealService service)
        {
            if (ModelState.IsValid)
            {
                await service.HasEdocResponce(new()
                {
                    Id = id
                });

                if (service.IsValid)
                    return Ok();

                service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
