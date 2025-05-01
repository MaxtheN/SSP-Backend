using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;

namespace SspUis.DataLayer.Repositories
{
    public class CreateEmployeeByUserDlDto : EmployeeDlDto<CreateEmployeeByUserDlDto>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
