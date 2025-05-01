using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class SrvApplicationYearlyPlanTableDlDto : EntityDto<SrvApplicationYearlyPlanTableDlDto, SrvApplicationYearlyPlanTable>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DistrictId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmployeeCount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int FreeCount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int PaidCount { get; set; }
    [LocalizedRequired]
    public decimal Amount { get; set; }
    [LocalizedRequired]
    public decimal LegalAmount { get; set; }
    [LocalizedRequired]
    public decimal EconomyAmount { get; set; }
}
