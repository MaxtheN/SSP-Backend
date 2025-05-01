using SspUis.Integration.Bandlik.Models;
using StatusGeneric;

namespace SspUis.Integration.Bandlik.Services
{
    public interface IBandlikService: IStatusGeneric
    {
        Task<GetStatisticByInnDataDto> GetStatisticByInn(string inn);
        Task<List<GetFreeAreaByInnDataDto>> GetFreeAreaByInn();
        Task<GetDaftarBySoatoDataDto> GetDaftarBySoato(GetDaftarBySoatoQuery dto);
        Task<GetInfoEmpByInnDataDto> GetInfoEmpByInn(string inn);
        Task<(string, dynamic)> PostMonoApplication(BandlikRequestMonoPostDto dto);
    }
}
