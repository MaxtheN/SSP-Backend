using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class StateAssetApplicationListDto : DocumentListDto<long>, ILinkToEntity<Application>
    {
        public Guid Id2 { get; set; }
        public int ApplicationTypeId { internal get; set; }
        public string DocNumber { get; set; }
        public string Status { get; set; }
        public long ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorPhoneNumber { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public string OkedCode { get; set; }
        public string Oked { get; set; }

        public DateOnly? AuctionDocOn { get; set; }
        public string AuctionDocNumber { get; set; }
        public string StateAssetName { get; set; }

    }
}
