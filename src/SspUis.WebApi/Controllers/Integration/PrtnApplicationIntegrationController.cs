using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.Integration.OnlineMahalla;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Route("MfyApplication/[action]")]

    public class PrtnApplicationIntegrationController :
        WebaseController
    {
        private IApplicationService _service;
        private readonly IUnitOfWork _unitOfWork;

        public PrtnApplicationIntegrationController(IApplicationService service, IUnitOfWork unitOfWork)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        [BasicAuth("mfy")]
        [ProducesResponseType(200)]
        public IActionResult PostMfyApplication(MfyApplicationStateDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.ChangeStatusMfyApplication(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult PostAcceptTemp(AcceptStatusPrtnApplicationDto dto)
        {
            //var data = _unitOfWork.Context.Set<PrtnApplication>().Include(a => a.Application).Where(a => a.Application.StatusId == 30 && (a.PrtnContractTypeId == 1  || a.PrtnContractTypeId == 2) && a.ChooseLocation == false).ToList();

            //foreach (var item in data)
            //{
            //    var acceptDto = new AcceptStatusPrtnApplicationDto()
            //    {
            //        Id = item.ApplicationId,
            //        Message = "dfjh",
            //        Offer = "Қабул қилинди"
            //    };

            //    _service.Accept(acceptDto);
            //}
            if (ModelState.IsValid)
            {
                _service.Accept(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [BasicAuth("mfy")]
        [ProducesResponseType(200)]
        public IActionResult GetApplicationState(string applicationId)
        {

            if (ModelState.IsValid)
            {
                var result = _service.CheckApplicationStatus(applicationId);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ExternalPostToOurApp(OnlineMahallaRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                //var result = await _service.PostApplication(dto);

                if (_service.IsValid)
                    return Ok(/*result*/);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
