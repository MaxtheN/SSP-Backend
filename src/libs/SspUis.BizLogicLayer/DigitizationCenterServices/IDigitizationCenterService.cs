using SspUis.Integration.DigitizationCenter;
using SspUis.Integration.DigitizationCenter.Models.FHDYO;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.Integration.DigitizationCenter.Models.Mehnat;
using SspUis.Integration.DigitizationCenter.Soliq;
using StatusGeneric;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.DigitizationCenterServices
{
    public interface IDigitizationCenterService : IStatusGeneric
    {
        #region Mehnat
        Task<Data> GetMehnatHistory(WorkPositionHistoryRequestDto dto);
        Task<NumberOfWorkersData> GetWorkersCount(NumberOfWorkersRequestDto dto);
        #endregion

        #region Soliq
        Task<List<LegalentityDebtResponseDto>> GetLegalentityDebt(LegalentityDebtRequestDto dto);
        #endregion

        #region Gsp
        Task<List<GSPNewApiData>> GetFromGSP(GSPNewApiRequestDto dto);
        #endregion

        #region FHDYO
        Task<List<DeathInfoResponseDto>> GetDeathInfoByPinfl(string pinfl);
        Task<List<BirthInfoResponseDto>> GetBirthInfo(BirthInfoRequestDto dto);
        #endregion
    }
}
