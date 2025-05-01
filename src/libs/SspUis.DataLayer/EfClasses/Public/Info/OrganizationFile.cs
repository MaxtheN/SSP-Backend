using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_organization_file")]
    [Index(nameof(OwnerId), Name = "ix_info_organization_files__owner")]
    public partial class OrganizationFile : FileEntity<int>, IHaveIdProp<Guid> 
    {
        [Required]
        [Column("column_name")]
        [StringLength(128)]
        public string ColumnName { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual Organization Owner { get; set; }
    }
}
