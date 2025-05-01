using System;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;
public class EmployeeStateAwardDlDto : EntityDto<EmployeeStateAwardDlDto, EmployeeStateAward>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    public DateOnly Year { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int StateAwardsId { get; set; }
}
