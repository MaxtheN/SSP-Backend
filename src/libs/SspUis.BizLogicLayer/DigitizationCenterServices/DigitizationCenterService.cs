using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.Integration.DigitizationCenter.Models.Mehnat;
using SspUis.Integration.DigitizationCenter.Soliq;
using SspUis.Integration.DigitizationCenter;
using System.Collections.Generic;
using System.Threading.Tasks;
using SspUis.Integration.DigitizationCenter.Services.GSP;
using StatusGeneric;
using SspUis.Integration.DigitizationCenter.Models.FHDYO;

namespace SspUis.BizLogicLayer.DigitizationCenterServices;

public class DigitizationCenterService : StatusGenericHandler, IDigitizationCenterService
{
    private readonly IDigitizationCenterMehnatService _mehnatService;
    private readonly IDigitizationCenterSoliqService _soliqService;
    private readonly IDigitizationCenterGspService _gspService;
    private readonly IDigitizationCenterFHDYOService _fHDYOService;

    public DigitizationCenterService(IDigitizationCenterMehnatService mehnatService, IDigitizationCenterSoliqService soliqService, IDigitizationCenterGspService gspService, IDigitizationCenterFHDYOService fHDYOService)
    {
        _mehnatService = mehnatService;
        _soliqService = soliqService;
        _gspService = gspService;
        _fHDYOService = fHDYOService;
    }

    #region Mehnat
    public async Task<Data> GetMehnatHistory(WorkPositionHistoryRequestDto dto)
    {
        var res = await _mehnatService.GetMehnatHistory(dto);
        CombineStatuses(_mehnatService);
        return res;
    }
    public Task<NumberOfWorkersData> GetWorkersCount(NumberOfWorkersRequestDto dto)
    {
        var res = _mehnatService.GetWorkersCount(dto);
        CombineStatuses(_mehnatService);
        return res;
    }
    #endregion

    #region Soliq
    public async Task<List<LegalentityDebtResponseDto>> GetLegalentityDebt(LegalentityDebtRequestDto dto)
    {
        var res = await _soliqService.GetLegalentityDebt(dto);
        CombineStatuses(_soliqService);
        return res;
    }
    #endregion

    #region Gsp
    public async Task<List<GSPNewApiData>> GetFromGSP(GSPNewApiRequestDto dto)
    {
        var res = await _gspService.GetFromGSP(dto);
        CombineStatuses(_gspService);
        return res;
    }
    #endregion

    #region FHDYO
    public async Task<List<DeathInfoResponseDto>> GetDeathInfoByPinfl(string pinfl)
    {
        var res = await _fHDYOService.GetDeathInfoByPinfl(new DeathInfoRequestDto
        {
            Id = "111",
            Pin = pinfl
        });
        CombineStatuses(_fHDYOService);
        return res;
    }
    public async Task<List<BirthInfoResponseDto>> GetBirthInfo(BirthInfoRequestDto dto)
    {
        var res = await _fHDYOService.GetBirthInfo(dto);
        CombineStatuses(_fHDYOService);
        return res;
    }
    #endregion
}
