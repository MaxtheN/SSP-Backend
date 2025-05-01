using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices
{
    public class NeedChamberServiceDto : UpdateNeedChamberServiceDlDto, ILinkToEntity<NeedChamberService>, IInfoHl
    {
        public string State { get; set; } = null!;
        public string ServicePriceType { get; set; } = null!;
        public new List<NeedChamberServiceTranslateDto> Translates { get; set; } = new();
        public new List<NeedChamberServiceFileDto> Files { get; set; } = new();
    }
}
