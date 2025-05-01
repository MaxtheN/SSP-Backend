using Microsoft.AspNetCore.Http;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.FileValidationServices
{
    public interface IFileValidationService : IStatusGeneric
    {
        bool Validate(IEnumerable<IFormFile> files);
    }
}
