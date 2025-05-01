using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeAcademicDegreeDto : EmployeeAcademicDegreeDlDto, ILinkToEntity<EmployeeAcademicDegree>
{
    public string AcademicDegree { get; set; }
}
