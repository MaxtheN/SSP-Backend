using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class BusinessActivityTypeTableDlDto : EntityDto<BusinessActivityTypeTableDlDto, BusinessActivityTypeTable>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long OwnerId { get; set; }
        [LocalizedRequired]
        public decimal Amount { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public int CurrencyId { get; set; }
    }
}
