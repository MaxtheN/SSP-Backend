using WEBASE.Models;
using WEBASE.EF;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStatusPrtnContractDlDto : EntityDto<UpdateStatusPrtnContractDlDto, PrtnContract>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        public string Message { get; set; }
        [LocalizedRequired]
        public int StatusId { get; set; }

        [LocalizedRequired]
        public bool IsRead { get; set; }
    }
}
