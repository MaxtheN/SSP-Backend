using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingSortFilterPageOptions : DocumentSortFilterOptions
    {
        public int? OrganizationId { get; set; } = null;
        public int? FinanceYear { get; set; }
    }
}
