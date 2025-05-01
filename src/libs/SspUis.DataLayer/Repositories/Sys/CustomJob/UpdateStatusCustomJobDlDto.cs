using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStatusCustomJobDlDto: EntityDto<UpdateStatusCustomJobDlDto, CustomJob>
    {
        [LocalizedRequired]
        [LocalizedRange(0, long.MaxValue)]
        public long Id { get; set; }
        [LocalizedRequired]
        public int StatusId { get; set; }
    }
}
