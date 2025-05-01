using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class NeedChamberServiceGroupListDto 
        : ILinkToEntity<NeedChamberServiceGroup>,
        IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string OrderCode { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}