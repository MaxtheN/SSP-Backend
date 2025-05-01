using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer.Hrm.InstituteServices
{
    public class InstituteListDto : ILinkToEntity<Institute>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string ShortName { get; set; }
        public string CodeAndText { get; set; }
        public string FullName { get; set; }
        public string Inn { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Code { get; set; }

        public int HemisId { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
