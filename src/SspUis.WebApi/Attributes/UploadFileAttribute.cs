using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SspUis.Core;

namespace SspUis.WebApi;

public class UploadFileAttribute : IActionFilter
{
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var param = context.ActionArguments["files"];
        if (param == null)
        {
            context.Result = new BadRequestObjectResult("Object is null");
            return;
        }

        var formFiles = param as List<IFormFile>;
        if (formFiles.Any(formFile => formFile.Length / (1024 * 1024) > Constants.UPLOAD_FILE_SIZE_MB))
        {
            context.Result = new BadRequestObjectResult($"Размер файла превышает {Constants.UPLOAD_FILE_SIZE_MB} МБ");
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
