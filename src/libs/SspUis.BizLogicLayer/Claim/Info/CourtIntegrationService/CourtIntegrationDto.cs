using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Claim
{
    public class CourtIntegrationDto : UpdateCourtntegrationDlDto, ILinkToEntity<Courtntegration>
    {
        public long ApplicationForCourtId { get; set; }
        public Guid CourtId { get; set; }
        public string ParticipantType { get; set; }
        public string DocNumber { get; set; }
        public Guid CategoryId { get; set; }
        public string CurrencyId { get; set; }
        public string EntityType { get; set; }
        public string ClaimKind { get; set; }
        public Guid FileId { get; set; }
        public Guid TypeId { get; set; }
        public Guid AmountCategoryId { get; set; }
        public Guid RegionId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid CountryId { get; set; }
        public long PaymentAccount { get; set; }
    }
}
