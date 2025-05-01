using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfClasses.Appeal
{
    [Table("doc_appeal_application_files", Schema = "appeal")]
    [Index(nameof(OwnerId), Name = "ix_doc_appeal_application_files__owner")]
    public partial class AppealApplicationFile : FileEntity<long>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(AppealApplication.Files))]
        public virtual AppealApplication Owner { get; set; }
    }
}
