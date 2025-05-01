using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;
[Table("doc_chastisement_file", Schema = "hrm")]
public class ChastisementFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Chastisement.Files))]
    public virtual Chastisement Owner { get; set; }
    [Column("is_reject")]
    public bool? IsReject { get; set; }
}
