using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices
{
    public class ContractorActivityTypeListDto : ILinkToEntity<ContractorActivityType>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ContractorActivityGroup { get; set; }
    }
}
