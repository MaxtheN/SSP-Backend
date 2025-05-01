using System;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;
public class EmployeeLanguageProficiencyDlDto : EntityDto<EmployeeLanguageProficiencyDlDto, EmployeeLanguageProficiency>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int LanguagperoficiencyId { get; set; }
    public int? LanguageDegreeId { get; set; }
    public string LanguageDegree { get; set; }
}
