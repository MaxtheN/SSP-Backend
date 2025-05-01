using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeePlaceOfWorkDto : EmployeePlaceOfWorkDlDto, ILinkToEntity<EmployeePlaceOfWork>
{
    public string Contractor { get; set; }
    public string EmploymentType { get; set; }
}
