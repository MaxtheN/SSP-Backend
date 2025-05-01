using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.FileValidationServices
{
    public class FileValidationConfig
    {
        public int MaxSize { get; set; } = 10 * 1024 * 1024;
    }
}
