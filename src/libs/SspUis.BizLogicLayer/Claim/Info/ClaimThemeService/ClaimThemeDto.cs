using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Claim.ClaimThemeServices
{
    public class ClaimThemeDto : UpdateClaimThemeDlDto, ILinkToEntity<ClaimTheme>, IInfoHl
    {
        public string State { get; set; } = null!;
        public new List<ClaimThemeTranslateDto> Translates { get; set; } = new();
    }
}
