using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Info.OrganizationalStructureServices
{
    public class OrganizationalStructureFilterDto: SortFilterPageOptions
    {
        public string Code { get; set; } 
        public string FullName { get; set; }
    }
}
