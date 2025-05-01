using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer.RelativeDegreeServices
{
    public class RelativeDegreeListDto : ILinkToEntity<RelativeDegree>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int GenderId { get; set; }
        public string Gender{ get; set; }
    }
}
