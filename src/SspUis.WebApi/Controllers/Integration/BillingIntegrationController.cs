using Microsoft.AspNetCore.Mvc;
using SspUis.Integration.Billing.Models;
using SspUis.Integration.Billing.Services;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers.Integration;
[ApiController]
[Route("billingIntegration/[action]")]
public class BillingIntegrationController : WebaseController
{
    private readonly IBillingService _service;

    public BillingIntegrationController(IBillingService service)
    {
        _service = service;
    }
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateDualApplication(DualApplicationCreateDto1 dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.CreateDualApplication(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetBillingUniversityList()
    {
        var result = await _service.GetUniversityList();
        if (result == null || result.Count == 0)
            return BadRequest(new { success = false, message = "Ma'lumot topilmadi!" });
        return Ok(result);
    }
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetBillingSpecialityList([FromQuery] int organizationId)
    {
        if (organizationId <= 0)
        {
            return BadRequest("Notog'ri organizationId.");
        }

        var specialityList = await _service.GetSpecialityList(organizationId);

        if (specialityList == null)
        {
            return NotFound("Maxsusliklar ro'yxati topilmadi");
        }

        return Ok(specialityList);
    }
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetUniversity([FromQuery] int organizationId)
    {
        if (organizationId <= 0)
        {
            return BadRequest("Notog'ri organizationId.");
        }

        var university = await _service.GetUniversity(organizationId);

        if (university == null)
            return NotFound($"{organizationId} bu id bilan Universitet topilmadi!");
        
        return Ok(university);
    }
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetSpeciality([FromQuery] int specialityId)
    {
        if (specialityId <= 0)
        {
            return BadRequest("Notog'ri organizationId.");
        }

        var speciality = await _service.GetSpeciality(specialityId);

        if (speciality == null)
            return NotFound("Maxsusliklar topilmadi");

        return Ok(speciality);
    }

}