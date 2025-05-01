using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class KpiRatingEmployeeTableDlDto : EntityDto<KpiRatingEmployeeTableDlDto, KpiRatingEmployeeTable>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public long EmployeeManageId { get; set; }
    public List<KpiRatingEmployeePointDlDto> Points { get; set; }
    protected override Action<IMappingExpression<KpiRatingEmployeeTableDlDto, KpiRatingEmployeeTable>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Points, c => c.Ignore());
}
