using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_need_chamber_service_files", Schema = "public")]
    [Index(nameof(OwnerId), Name = "ix_info_need_chamber_service_files__owner")]
    public class NeedChamberServiceFile : FileEntity<int>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(NeedChamberService.Files))]
        public virtual NeedChamberService Owner { get; set; }
    }
}