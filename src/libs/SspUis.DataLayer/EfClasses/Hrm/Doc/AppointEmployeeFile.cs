using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;
[Table("doc_appoint_employee_file", Schema = "hrm")]
public class AppointEmployeeFile : FileEntity<long>, IHaveIdProp<Guid>
{
  
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(AppointEmployee.Files))]
    public virtual AppointEmployee Owner { get; set; }

    [Column("is_reject")]
    public bool? IsReject { get; set; }
}
