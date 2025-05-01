using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLanguageProficiencyDto : EmployeeLanguageProficiencyDlDto, ILinkToEntity<EmployeeLanguageProficiency>
{
    public string Languagperoficiency { get; set; }
    public string? LanguageDegrees { get; set; }
}
