using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Integration.MSPD.GSP;

namespace SspUis.BizLogicLayer.UserServices
{
    public class GSPPersonInfoRequestForUserDto : GSPPersonInfoRequestDto
    {
        public int OrganizationId { get; set; }
    }
}
