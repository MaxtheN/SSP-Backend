using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class EmployeeSortFilterPageOptions : SelectListSortFilterPageOptions<int>
{
    public int? ParentOrganizationId { get; set; }
    public int? OrganizationId { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
}
