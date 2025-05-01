using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.UserServices
{
    public class CreateUserDto : CreateUserDlDto
    {
        public CreatePersonDlDto Person { get; set; } = new();
    }
}
