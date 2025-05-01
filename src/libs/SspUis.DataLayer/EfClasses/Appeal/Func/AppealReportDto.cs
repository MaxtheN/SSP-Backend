using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses;

[Keyless]
public class AppealReportDto
{
	[Column("region_name")]
	public string? RegionName { get; set; }
	[Column("district_name")]
	public string? DistrictName { get; set; }
	[Column("region_id")]
	public int? RegionId { get; set; }
	[Column("district_id")]
	public int? DistrictId { get; set; }
    [Column("full_name_st")]
    public string? StatusFullname { get; set; }
	[Column("status_id")]
	public int? StatusId { get; set; }
	public string? TypeFullname { get; set; }
	[Column("app_type_id")]
	public int? TypeId { get; set; }
    [Column("total_count")]
	public int? TotalCount { get; set; }
}
