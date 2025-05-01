using System.Collections.Generic;
using SspUis.BizLogicLayer.Models;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.UserServices
{
    public class UserSortFilterPageOptions : SelectListSortFilterPageOptions<int>
    {
        public Dictionary<string, FilterMeta> Filters { get; set; }
        public int? OrganizationId { get; set; }
        public int? RoleId { get; set; }
        public int? ModuleId { get; set; }
    }
}
