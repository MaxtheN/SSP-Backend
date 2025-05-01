using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_prtn_contract_files", Schema = "partner")]
    public partial class PrtnContractFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(PrtnContract.Files))]
        public virtual PrtnContract Owner { get; set; }
    }
}
