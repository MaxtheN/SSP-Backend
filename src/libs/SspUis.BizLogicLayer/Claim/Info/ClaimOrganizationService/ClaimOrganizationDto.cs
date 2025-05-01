using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationServices
{
    public class ClaimOrganizationDto : UpdateClaimOrganizationDlDto, ILinkToEntity<ClaimOrganization>, IInfoHl
    {
        public string State { get; set; } = null!;
        public new List<ClaimOrganizationTranslateDto> Translates { get; set; } = new();
    }
}
