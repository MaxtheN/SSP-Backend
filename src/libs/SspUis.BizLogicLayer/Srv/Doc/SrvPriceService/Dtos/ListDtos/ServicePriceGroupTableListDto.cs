using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceGroupTableListDto : ILinkToEntity<ServicePriceTable>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public int NeedChamberServiceId { get; set; }
        public string NeedChamberService { get; set; }
        public int ServicePriceTypeId { get; set; }
        public string ServicePriceType { get; set; }
        public decimal? BeginCoef { get; set; }
        public decimal? EndCoef { get; set; }
        public decimal? ConcreteCoef { get; set; }
        public bool IsConcrete { get; set; }
    }
}
