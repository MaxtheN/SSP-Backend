using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class JoinAntiCorruptionApplicationFileDlDto : EntityDto<JoinAntiCorruptionApplicationFileDlDto, JoinAntiCorruptionApplicationFile>, 
        IHaveIdProp<Guid>,ILinkToEntity<JoinAntiCorruptionApplicationFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
