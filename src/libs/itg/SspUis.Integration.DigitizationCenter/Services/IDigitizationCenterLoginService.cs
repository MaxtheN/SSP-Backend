using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter
{
    public interface IDigitizationCenterLoginService: IStatusGeneric
    {
        Task Login();
    }
}
