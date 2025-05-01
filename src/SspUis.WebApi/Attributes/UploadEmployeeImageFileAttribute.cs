using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SspUis.Core;

namespace SspUis.WebApi;

public class UploadEmployeeImageFileAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var param = context.ActionArguments["file"];
        if (param == null)
        {
            context.Result = new BadRequestObjectResult("Object is null");
            return;
        }

        var formFile = param as IFormFile;
        if (formFile.Length / 1024 > Constants.UPLOAD_EMPLOYEE_IMAGE_SIZE_KB)
        {
            context.Result = new BadRequestObjectResult($"Размер файла превышает {Constants.UPLOAD_EMPLOYEE_IMAGE_SIZE_KB} KБ");
            return;
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
