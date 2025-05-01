using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;

namespace SspUis.DataLayer.Repositories
{
    public class CreateUserByEmployeeDlDto : UserDlDto<CreateUserByEmployeeDlDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string UserName { get; set; }
        public override string Password { get => base.Password; set => base.Password = value; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int EmployeeId { get; set; }
    }
}
