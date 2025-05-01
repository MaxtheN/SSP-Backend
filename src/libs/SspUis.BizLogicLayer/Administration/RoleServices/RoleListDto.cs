using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.RoleServices
{
    public class RoleListDto : ILinkToEntity<Role>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string OrderCode { get; set; }
        public bool IsDefault { get; set; }
        public bool IsHr { get; set; }
        public string State { get; set; }
    }

}
