using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;
using System;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationTypeServices
{
    public class ClaimOrganizationTypeListDto :  ILinkToEntity<ClaimOrganizationType>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Details { get; set; } = null!;
        public string State { get; set; } = null!;
    }
}
