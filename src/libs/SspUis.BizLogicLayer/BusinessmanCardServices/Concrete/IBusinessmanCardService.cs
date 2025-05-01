using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.BusinessmanCardServices
{
    public interface IBusinessmanCardService : IStatusGeneric
    {
        Task<BusinessmanCardDto> GetByInn(string inn);
    }
}
