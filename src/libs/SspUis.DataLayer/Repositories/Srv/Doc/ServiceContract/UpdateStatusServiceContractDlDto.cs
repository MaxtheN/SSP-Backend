using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStatusServiceContractDlDto
        : EntityDto<UpdateStatusServiceContractDlDto, ServiceContract>,
        IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }

        [LocalizedRequired]
        public int StatusId { get; set; }

        public string Details { get; set; }
    }
}
