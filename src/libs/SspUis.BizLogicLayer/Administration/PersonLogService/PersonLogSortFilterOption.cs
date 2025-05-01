using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Administration.PersonLogService
{
    public class PersonLogSortFilterOption : SortFilterPageOptions
    {
        public int EmployeeId { get; set; }
    }
}
