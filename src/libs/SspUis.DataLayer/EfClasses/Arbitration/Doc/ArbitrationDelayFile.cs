using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

//[Keyless]
[Table("doc_arbitration_delay_file", Schema = "arbitration")]
public partial class ArbitrationDelayFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [Column("can_sign")]
    public bool CanSign { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual ArbitrationDelay Owner { get; set; }
}
