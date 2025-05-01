using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.Integration.DigitizationCenter.Soliq;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter.Services.GSP
{
    public interface IDigitizationCenterGspService : IStatusGeneric
    {
        Task<List<GSPNewApiData>> GetFromGSP(GSPNewApiRequestDto dto);
    }
}
