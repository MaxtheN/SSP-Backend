using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStatusCompletedServiceDlDto 
        : EntityDto<UpdateStatusCompletedServiceDlDto, CompletedService>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }

        [LocalizedRequired]
        public int StatusId { get; set; }

        public string Details { get; set; }
    }
}