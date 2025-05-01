using WEBASE.Models;
using WEBASE.EF;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStatusApplicationDlDto : EntityDto<UpdateStatusApplicationDlDto, Application>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        [LocalizedRequired]
        public int StatusId { get; set; }

        public string Message { get; set; }
        
        
        //public string Parameters { get; set; }
        //public string Offer { get; set; }

    }
}
