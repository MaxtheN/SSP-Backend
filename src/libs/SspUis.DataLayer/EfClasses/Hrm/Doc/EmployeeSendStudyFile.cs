using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;
[Table("doc_employee_send_study_file", Schema = "hrm")]
public class EmployeeSendStudyFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(EmployeeSendStudy.Files))]
    public virtual EmployeeSendStudy Owner { get; set; }
    [Column("is_reject")]
    public bool? IsReject { get; set; }
}