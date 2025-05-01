using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnApplicationGraphDlDto : EntityDto<PrtnApplicationGraphDlDto, PrtnApplicationGraph>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1000, 9999)]
        public int YearIn { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, 12)]
        public int MonthIn { get; set; }
        [LocalizedRequired]
        public int NewVacanciesCount { get; set; }

    }
}
