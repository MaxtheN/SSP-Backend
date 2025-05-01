using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CourtntegrationDlDto<Tdto> : EntityDto<Tdto, Courtntegration> 
        where Tdto : CourtntegrationDlDto<Tdto>
    {
        [LocalizedRequired]
        public long ApplicationForCourtId { get; set; }
        [LocalizedRequired]
        public Guid CourtId { get; set; }
        [LocalizedRequired]
        public string ParticipantType { get; set; }
        [LocalizedRequired]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        public Guid CategoryId { get; set; }
        [LocalizedRequired]
        public string CurrencyId { get; set; }
        [LocalizedRequired]
        public string EntityType { get; set; }
        [LocalizedRequired]
        public string ClaimKind { get; set; }
        [LocalizedRequired]
        public Guid FileId { get; set; }
        [LocalizedRequired]
        public Guid TypeId { get; set; }
        [LocalizedRequired]
        public Guid AmountCategoryId { get; set; }
        [LocalizedRequired]
        public Guid RegionId { get; set; }
        [LocalizedRequired]
        public Guid DistrictId { get; set; }
        [LocalizedRequired]
        public Guid CountryId { get; set; }
        [LocalizedRequired]
        public long PaymentAccount { get; set; }

        [LocalizedRequired]
        public string SignedData { get; set; }

        public override Courtntegration CreateEntity()
        {
            var entity = base.CreateEntity();
            return entity;
        }

        public override void UpdateEntity(Courtntegration entity)
        {
            base.UpdateEntity(entity); 
        }
    }
}
