using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateUserInfoDlDto : EntityDto<UpdateUserInfoDlDto, BusinessmanUser>
    {
        public string Email { get; set; }
    }
}
