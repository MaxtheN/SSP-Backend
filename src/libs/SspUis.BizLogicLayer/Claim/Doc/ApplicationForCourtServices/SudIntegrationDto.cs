using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Claim.Doc.ApplicationForCourtServices
{
    public class SudIntegrationDto
    {
        public long ApplicationCourtId { get; set; }
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
        //public Guid PostReasonId { get; set; }
        //public Guid DutyReasonId { get; set; }
        public Guid RegionId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid CountryId { get; set; }
        public long PaymentAccount { get; set; }
        //public string FileHash { get; set; }
        //public string SignedHash { get; set; }

        //e-imzo
        //[LocalizedRequired]
        //public string SignedData { get; set; }
        //public Guid DataFile { get; set; }
        //public Guid SignFile { get; set; }
        //public string SignedUserInfo { get; set; }
        //public bool IsPinfl { get; set; } = false;
    }
}
