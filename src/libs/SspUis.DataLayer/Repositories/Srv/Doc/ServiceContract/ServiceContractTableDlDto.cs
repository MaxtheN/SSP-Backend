using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceContractTableDlDto
        : EntityDto<ServiceContractTableDlDto, ServiceContractTable>,
        IHaveIdProp<long>,
        ILinkToEntity<ServiceContractTable>
    {
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int NeedChamberServiceId { get; set; }

        [JsonIgnore]
        public long? ServicePriceId { get; set; }

        [JsonIgnore]
        public long? ServicePriceTableId { get; set; }

        [JsonIgnore]
        public string OfferServiceText { get; set; }

        public decimal? RealCoef { get; set; }
        public decimal? Price { get; set; }

        [LocalizedRequired]
        public bool CanPayDivided { get; set; }

        public override ServiceContractTable CreateEntity()
        {
            var entity = base.CreateEntity();
            return entity;
        }

        public override void UpdateEntity(ServiceContractTable entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
