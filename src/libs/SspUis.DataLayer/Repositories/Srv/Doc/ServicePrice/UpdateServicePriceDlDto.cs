using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateServicePriceDlDto : ServicePriceDlDtoo<UpdateServicePriceDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long Id { get; set; }
    }
}
