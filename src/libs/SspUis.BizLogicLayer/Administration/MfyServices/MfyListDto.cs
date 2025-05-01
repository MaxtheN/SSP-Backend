using GenericServices;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.MfyServices
{
    public class MfyListDto :
        ILinkToEntity<Mfy>,
        IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string OrderCode { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int RegionId { get; set; }
        public long ExternalId { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
