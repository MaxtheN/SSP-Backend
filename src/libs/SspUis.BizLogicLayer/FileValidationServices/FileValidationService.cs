using Microsoft.AspNetCore.Http;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.FileValidationServices
{
    public class FileValidationService : StatusGenericHandler, IFileValidationService
    {
        private readonly FileValidationConfig _config;
        public FileValidationService(FileValidationConfig config)
        {
            _config = config;
        }
        public bool Validate(IEnumerable<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > _config.MaxSize)
                {
                    AddError($"Fayl hajmi {_config.MaxSize / 1024 / 1024} Mb dan ko'p bo'lishi mumkin emas.");
                    break;
                }
            }
            return IsValid;
        }
    }
}
