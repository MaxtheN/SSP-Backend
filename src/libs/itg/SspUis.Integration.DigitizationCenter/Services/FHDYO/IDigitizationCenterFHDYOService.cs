using SspUis.Integration.DigitizationCenter.Models.FHDYO;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter
{
    public interface IDigitizationCenterFHDYOService:IStatusGeneric
    {
        Task<List<DeathInfoResponseDto>> GetDeathInfoByPinfl(DeathInfoRequestDto dto);
        Task<List<BirthInfoResponseDto>> GetBirthInfo(BirthInfoRequestDto dto);
    }
}
