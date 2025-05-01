using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_memship_certificate_file", Schema = "memship")]
public partial class MemshipCertificateFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(MemshipCertificate.Files))]
    public virtual MemshipCertificate Owner { get; set; }
}