using GenericServices;
using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices
{
    public class NeedChamberServiceListDto : ILinkToEntity<NeedChamberService>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public bool CanPayDivided { get; set; }
        public bool IsOffer { get; set; }
        public string State { get; set; } = null!;
        public int StateId { get; set; }
        public string Details { get; set; } = string.Empty;
        public int? MeetingTypeId { get; set; }
        public string MeetingType { get; set; }
        public int ServicePriceTypeId { get; set; }
        public string ServicePriceType { get; set; }
        public long? EmployeeManageId { get; set; }
		public int? NeedChamberServiceGroupId { get; set; }

		public List<NeedChamberServiceFileDto> Files { get; set; } = new();
    }
}
