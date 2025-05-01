using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class NeedChamberServiceFileDlDto : EntityDto<NeedChamberServiceFileDlDto, NeedChamberServiceFile>,
        IHaveIdProp<Guid>,
        ILinkToEntity<NeedChamberServiceFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
