using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.OkedServices
{
    public class OkedListDto : ILinkToEntity<Oked>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int Level { get; set; }
        public bool IsGroup { get; set; }
        public int ParentId { get; set; }
        public int StateId { get; set; }
        public string OkedType { get; set; }

        public string Parent { get; set; }
        public string State { get; set; }
    }

}
