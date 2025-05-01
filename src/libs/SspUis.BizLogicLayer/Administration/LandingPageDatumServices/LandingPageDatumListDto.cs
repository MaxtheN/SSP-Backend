using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.LandingPageDatumServices
{
    public class LandingPageDatumListDto : ILinkToEntity<LandingPageDatum>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
