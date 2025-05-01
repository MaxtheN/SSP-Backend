using System.ComponentModel.DataAnnotations.Schema;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.RegionServices
{
    public class RegionListDto : ILinkToEntity<Region>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string Code { get; set; }
        public string Soato { get; set; }
        public int? BankRegionId { get; set; }
        public string RoamingCode { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public char IsGroup { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }

}
