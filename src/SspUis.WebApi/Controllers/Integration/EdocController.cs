using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Route("edoc/[action]")]
    public class EdocController : ControllerBase
    {
        private readonly IExternalDocFromEdocService _externalDocFromEdocService;

        public EdocController(IExternalDocFromEdocService externalDocFromEdocService)
        {
            _externalDocFromEdocService = externalDocFromEdocService;
        }
                
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Ping()
        {
            if (ModelState.IsValid)
            {
                return Ok();
            }
            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[ProducesResponseType(200)]
        //public IActionResult Update([FromBody] UpdateExternalDocumentFromEdocDlDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _externalDocFromEdocService.Update(dto);

        //        if (_externalDocFromEdocService.IsValid)
        //            return Ok();
        //        _externalDocFromEdocService.CopyErrorsToModelState(ModelState);
        //    }
        //    return ValidationProblem(ModelState);
        //}
    }
}
