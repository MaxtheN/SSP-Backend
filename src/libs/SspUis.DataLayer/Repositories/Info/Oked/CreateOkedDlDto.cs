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
    public class CreateOkedDlDto : OkedDlDto<CreateOkedDlDto>
    {
        public bool IsGroup { get; set; }
        public int? ParentId { get; set; }
        public int Level { get; set; }
    }
}
