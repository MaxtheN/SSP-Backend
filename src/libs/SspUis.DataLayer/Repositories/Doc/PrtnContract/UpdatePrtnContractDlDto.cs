using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using SspUis.Core.Security;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdatePrtnContractDlDto : PrtnContractDlDto<UpdatePrtnContractDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        //[LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        [LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        public int StatusId { get; set; }

        [LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        public bool IsRead { get; set; }

    }
}
