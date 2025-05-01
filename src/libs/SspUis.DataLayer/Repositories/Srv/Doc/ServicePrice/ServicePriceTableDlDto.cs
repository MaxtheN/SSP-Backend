using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ServicePriceTableDlDto
        : EntityDto<ServicePriceTableDlDto, ServicePriceTable>,
        IHaveIdProp<long>,
        ILinkToEntity<ServicePriceTable>
    {
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int NeedChamberServiceId { get; set; }

        public decimal? BeginCoef { get; set; }
        public decimal? EndCoef { get; set; }
        public decimal? ConcreteCoef { get; set; }
        [JsonIgnore]
        public bool IsConcrete { get; set; }

        public override ServicePriceTable CreateEntity()
        {
            var entity = base.CreateEntity();
            return entity;
        }

        public override void UpdateEntity(ServicePriceTable entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
