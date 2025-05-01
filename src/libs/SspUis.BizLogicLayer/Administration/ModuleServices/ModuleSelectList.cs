using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ModuleServices
{
    public class ModuleGroupSelectListDto
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public IEnumerable<ModuleSubGroupSelectListDto> SubGroups { get; set; }
    }

    public class ModuleSubGroupSelectListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public IEnumerable<ModuleSelectListDto> Modules { get; set; }
    }

    public class ModuleSelectListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }
}
