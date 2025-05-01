using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;
[Table("doc_employee_send_train_file", Schema = "hrm")]
public class EmployeeSendTrainFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(EmployeeSendTrain.Files))]
    public virtual EmployeeSendTrain Owner { get; set; }
    [Column("is_reject")]
    public bool? IsReject { get; set; }
}