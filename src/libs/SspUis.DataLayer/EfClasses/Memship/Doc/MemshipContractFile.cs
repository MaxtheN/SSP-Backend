using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;
[Table("doc_memship_contract_file", Schema = "memship")]
public class MemshipContractFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(MemshipContract.Files))]
    public virtual MemshipContract Owner { get; set; }
    [Column("is_reject")]
    public bool? IsReject { get; set; }
}
