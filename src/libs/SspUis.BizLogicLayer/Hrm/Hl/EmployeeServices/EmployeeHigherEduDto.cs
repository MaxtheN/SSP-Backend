using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeHigherEduDto : EmployeeHigherEduDlDto, ILinkToEntity<EmployeeHigherEdu>
{
    public virtual string Specialty { get; set; }
    public virtual string Institute { get; set; }
    public virtual string EmployeeHigherEduDegree { get; set; }
}
