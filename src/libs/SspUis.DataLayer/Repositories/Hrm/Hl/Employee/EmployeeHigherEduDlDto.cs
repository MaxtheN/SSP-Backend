using System;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;
public class EmployeeHigherEduDlDto : EntityDto<EmployeeHigherEduDlDto, EmployeeHigherEdu>,IHaveIdProp<int>
{
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int SpecialtyId { get; set; }
    public int? InstituteId { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(2000)]
    public string InstituteName { get; set; }
    [LocalizedStringLength(10)]
    public string DocumentSeries { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(10)]
    public string DocumentNumber { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmployeeHigherEduDegreeId { get; set; }
    [LocalizedRequired]
    public DateOnly DateOfIssue { get; set; }
}
