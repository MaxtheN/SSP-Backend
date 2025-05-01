using SspUis.Integration.DigitizationCenter.Models.Mehnat;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter
{
    public interface IDigitizationCenterMehnatService:IStatusGeneric
    {
        Task<Data> GetMehnatHistory(WorkPositionHistoryRequestDto dto);
        Task<NumberOfWorkersData> GetWorkersCount(NumberOfWorkersRequestDto dto);
    }
}
