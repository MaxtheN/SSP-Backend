using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CreateUserDlDto : UserDlDto<CreateUserDlDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string UserName { get; set; }
        public int PersonId { get; set; }
        [LocalizedRequired]
        [LocalizedMinLength(6)]
        public override string Password { get => base.Password; set => base.Password = value; }
    }
}
