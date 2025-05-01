using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using SspUis.BizLogicLayer.PersonServices;

namespace SspUis.BizLogicLayer.UserServices
{
    public class UserDto : UpdateUserDlDto, ILinkToEntity<User>
    {
        public string Organization { get; set; }
        public string State { get; set; }
        public int PersonId { get; set; }
        public PersonDto Person { get; set; } = new();
        public string? EmployeeManage { get; set; } = null;
    }
}
