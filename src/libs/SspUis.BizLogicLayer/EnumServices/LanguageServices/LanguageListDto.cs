using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.EnumServices
{
    public class LanguageSelectListDto : SelectListItem<int>
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }
}
