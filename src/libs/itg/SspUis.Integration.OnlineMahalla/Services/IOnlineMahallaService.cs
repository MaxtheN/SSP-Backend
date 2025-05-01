using StatusGeneric;

namespace SspUis.Integration.OnlineMahalla
{
    public interface IOnlineMahallaService :
        IStatusGeneric
    {
        Task<List<OnlineMahallaDataDto>> Get(long updateId);
        Task<OnlineMahallaResponseDto<OnlineMahallaPostDataDto>> PostApplication(OnlineMahallaRequestDto dto);
        Task<MahallaApplicationStatusUpdateResponseDto> UpdateApplicationStatus(MahallaApplicationStatusUpdateRequestDto dto);
    }
}