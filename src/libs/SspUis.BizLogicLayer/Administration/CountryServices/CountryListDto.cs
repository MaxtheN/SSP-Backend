using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.CountryServices
{
    public class CountryListDto : ILinkToEntity<Country>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int StateId { get; set; }
        public string OrderCode { get; set; }
        public string Code { get; set; }
        public string TextCode { get; set; }

        public string State { get; set; }
    }

}
