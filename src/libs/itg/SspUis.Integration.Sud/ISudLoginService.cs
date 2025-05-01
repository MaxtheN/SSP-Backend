using SspUis.Integration.Sud.Models.AuthModels;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud
{
    public interface ISudLoginService : IStatusGeneric
    {
        Task<string> SudAuthLoginCreate();
    }
}
