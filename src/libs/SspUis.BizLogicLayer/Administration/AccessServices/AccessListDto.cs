using SspUis.DataLayer.EfClasses;
using GenericServices;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AccessServices
{
    public class AccessListDto : ILinkToEntity<Accessibility>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public bool HasAccess { get; set; }
    }
}