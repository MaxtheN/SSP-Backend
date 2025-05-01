using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Claim
{
    [Table("doc_claim_application_files", Schema = "claim")]
    [Index(nameof(OwnerId), Name = "ix_doc_claim_application_files__owner")]
    public partial class ClaimApplicationFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ClaimApplication.Files))]
        public virtual ClaimApplication Owner { get; set; }

    }
}
