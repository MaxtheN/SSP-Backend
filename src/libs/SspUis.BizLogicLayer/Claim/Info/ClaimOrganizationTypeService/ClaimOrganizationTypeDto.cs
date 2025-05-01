using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationTypeServices
{
    public class ClaimOrganizationTypeDto : UpdateClaimOrganizationTypeDlDto, ILinkToEntity<ClaimOrganizationType>, IInfoHl
    {
        public string State { get; set; } = null!;
        public new List<ClaimOrganizationTypeTranslateDto> Translates { get; set; } = new();
    }
}
