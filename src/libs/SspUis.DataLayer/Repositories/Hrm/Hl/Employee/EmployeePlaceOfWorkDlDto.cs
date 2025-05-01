using System;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class EmployeePlaceOfWorkDlDto : EntityDto<EmployeePlaceOfWorkDlDto, EmployeePlaceOfWork>,IHaveIdProp<int>
{
    public int Id { get; set; }
    public long? ContractorId { get; set; }
    public int? PositionId { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(2000)]
    public string PositionName { get; set; }
    public string DepartmentName { get; set; }
    [LocalizedRequired] 
    [LocalizedStringLength(250)]
    public string ContractorName { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int EmploymentTypeId { get; set; }
	public long? AdditionId { get; set; }
	[LocalizedRequired]
    public DateOnly StartOn { get; set; }
    public DateOnly? EndOn { get; set; }
    [JsonIgnore]
    public bool IsImported { get; set; }
}
