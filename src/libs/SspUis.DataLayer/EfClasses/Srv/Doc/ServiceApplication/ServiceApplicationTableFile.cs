using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_application_table_files", Schema = "srv")]
    [Index(nameof(OwnerId), Name = "ix_doc_service_application_table_files__owner")]
    public partial class ServiceApplicationTableFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ServiceApplicationTable.Files))]
        public virtual ServiceApplicationTable Owner { get; set; }
    }
}
